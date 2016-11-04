using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using blqw.IOC;

namespace blqw.Web
{
    /// <summary>
    /// 客户端请求的异步实现
    /// </summary>
    public sealed class HttpClientAsync : IHttpClient
    {
        /// <summary>
        /// 表示 <seealso cref="byte" /> 类型的空数组。此字段为只读。
        /// </summary>
        private static readonly byte[] _EmptyBytes = new byte[0];

        /// <summary>
        /// 表示一个CONNECT的请求方法,此字段为只读
        /// </summary>
        private static readonly HttpMethod _HttpMethodConnect = new HttpMethod("CONNECT");
        /// <summary>
        /// 表示一个 PATCH 的请求方法,此字段为只读
        /// </summary>
        private static readonly HttpMethod _HttpMethodPatch = new HttpMethod("PATCH");

        /// <summary>
        /// Client 包装对象
        /// </summary>
        struct HttpClientWrapper : IDisposable
        {
            private readonly HttpMessageInvoker _client;
            private readonly bool _shouldDicpose;

            public HttpClientWrapper(IHttpRequest request)
            {
                _client = request.GetAsyncInvoker();
                _shouldDicpose = false;
                if (_client == null)
                {
                    _shouldDicpose = true;
                    _client = HttpClientProvider.GetClient(request.Proxy);
                }
            }

            public void Dispose()
            {
                if (_shouldDicpose && !HttpClientProvider.IsCached(_client))
                {
                    _client.Dispose();
                }
            }

            /// <summary>
            /// 以异步操作发送 HTTP 请求。
            /// </summary>
            public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => _client.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 发送异步请求
        /// </summary>
        /// <param name="request"> 请求对象 </param>
        /// <param name="cancellationToken"> 用于取消请求的通知对象 </param>
        /// <returns> </returns>
        public async Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken)
        {
            var timer = HttpRequestTimer.OnStart();
            var data = default(HttpRequestData);
            try
            {
                request.OnInitialize();
                data = new HttpRequestData(request);
                var www = Convert(data);
                request.Logger?.Write(TraceEventType.Verbose, () => data.Raw);

                timer.OnReady();
                request.OnSending();
                using (var source1 = new CancellationTokenSource(data.Timeout.TotalMilliseconds >= int.MaxValue ? int.MaxValue : (int)data.Timeout.TotalMilliseconds))
                using (var source2 = CancellationTokenSource.CreateLinkedTokenSource(source1.Token, cancellationToken))
                using (var client = new HttpClientWrapper(request)) //为了解决动态代理的问题
                {
                    var response = await client.SendAsync(www, source2.Token);
                    var cookies = data.Cookies;
                    while (request.AutoRedirect && response.StatusCode == HttpStatusCode.Redirect) //手动处理302的请求
                    {
                        request.Logger?.Write(TraceEventType.Verbose, () => request.Response?.ResponseRaw);
                        request.Logger?.Write(TraceEventType.Information, "StatusCode=302; 正在重定向...");
                        if (cookies == null)
                        {
                            cookies = new CookieContainer(); //302时必须使用 cookie
                        }
                        SetCookies(response, cookies);
                        www = Convert(data, response.Headers.Location); //构建新的请求
                        request.Logger?.Write(TraceEventType.Verbose, () => data.Raw);
                        response = await client.SendAsync(www, source2.Token);
                    }
                    timer.OnSend();
                    request.Response = await Convert(response, request.CookieMode != HttpCookieMode.None);
                    SetCookies(response, cookies);
                    request.OnEnd(request.Response);
                }
            }
            catch (Exception ex)
            {
                timer.OnError();
                if (ex is TaskCanceledException)
                {
                    ex = new TimeoutException("请求已超时");
                }
                var res = new HttpResponse { Exception = ex };
                request.OnError(res);
                request.Response = res;
                request.Logger?.Write(TraceEventType.Error, "异步请求中出现错误", ex);
            }
            finally
            {
                timer.OnEnd();
                request.Logger?.Write(TraceEventType.Verbose, () => request.Response?.ResponseRaw);
                request.Logger?.Write(TraceEventType.Information, timer.ToString());
            }
            ((HttpResponse)request.Response).RequestData = data;
            return request.Response;
        }

        /// <summary>
        /// 将<seealso cref="HttpResponseMessage"/>转为<seealso cref="HttpResponse"/>
        /// </summary>
        /// <param name="response">待转换的对象</param>
        /// <param name="useCookies">是否使用Cookie</param>
        /// <returns></returns>
        private async Task<HttpResponse> Convert(HttpResponseMessage response, bool useCookies)
        {
            if (response == null)
            {
                return new HttpResponse { StatusCode = 0 };
            }
            var res = new HttpResponse { Headers = new HttpHeaders() };
            using (response)
            {
                foreach (var header in response.Headers)
                {
                    foreach (var value in header.Value)
                    {
                        res.Headers.Add(header.Key, value);
                    }
                }
                foreach (var header in response.Content.Headers)
                {
                    foreach (var value in header.Value)
                    {
                        res.Headers.Add(header.Key, value);
                    }
                }
                var contentType = (HttpContentType)response.Content.Headers.ContentType?.ToString();
                var body = await response.Content.ReadAsByteArrayAsync();
                res.Body = new HttpBody(contentType, body);

                if (useCookies)
                {
                    var cookies = new CookieContainer();
                    SetCookies(response, cookies);
                    res.Cookies = cookies.GetCookies(response.RequestMessage.RequestUri);
                }

                res.StatusCode = response.StatusCode;
                res.Status = response.ReasonPhrase;
                res.SchemeVersion = $"{response.RequestMessage.RequestUri.Scheme.ToUpperInvariant()}/{response.Version}";
                res.IsSuccessStatusCode = response.IsSuccessStatusCode;
            }
            return res;
        }

        /// <summary>
        /// 根据cookie操作模式获取响应中的cookie信息
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private void SetCookies(HttpResponseMessage response, CookieContainer cookies)
        {
            if (cookies == null)
            {
                return;
            }
            var url = response.RequestMessage.RequestUri;
            IEnumerable<string> cookieHeader;
            if (response.Headers.TryGetValues("Set-Cookie", out cookieHeader))
            {
                foreach (var cookie in cookieHeader)
                {
                    try
                    {
                        cookies.SetCookies(url, cookie);
                    }
                    catch (CookieException ex)
                    {
                        //有可能返回的Cookie值有错误,写入会失败
                    }
                }
            }
        }

        /// <summary>
        /// 将<seealso cref="HttpRequestData"/>转换为<seealso cref="HttpRequestMessage"/>对象
        /// </summary>
        /// <param name="data">待转换的对象</param>
        /// <param name="redirect">重定向地址,用于重写<see cref="HttpRequestData.Url"/></param>
        /// <returns></returns>
        private HttpRequestMessage Convert(HttpRequestData data, Uri redirect = null)
        {
            var url = data.Url;
            if (redirect != null)
            {
                url = new Uri(new Uri(data.Url), redirect).ToString();
            }
            var request = data.Request;
            request.Logger?.Write(TraceEventType.Information, url);
            var www = new HttpRequestMessage(GetHttpMethod(request), url) { Version = data.Version };
            if (data.Body != null)
            {
                www.Content = new ByteArrayContent(data.Body ?? _EmptyBytes);
            }
            foreach (var header in data.Headers)
            {
                //防止中文引起的头信息乱码
                var transfer = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.UTF8.GetBytes(header.Value));
                if (!www.Headers.TryAddWithoutValidation(header.Key, transfer))
                {
                    www.Content?.Headers.TryAddWithoutValidation(header.Key, transfer);
                }
            }

            var cookieHeader = data.Cookies?.GetCookieHeader(redirect?.IsAbsoluteUri == true ? redirect : data.Host);
            if (!string.IsNullOrWhiteSpace(cookieHeader))
            {
                www.Headers.Add("Cookie", cookieHeader);
            }

            return www;
        }

        /// <summary>
        /// 从 <seealso cref="IHttpRequest"/> 中获取 <seealso cref="HttpMethod"/>
        /// </summary>
        public HttpMethod GetHttpMethod(IHttpRequest request)
        {
            switch (request.Method)
            {
                case HttpRequestMethod.Get:
                    return HttpMethod.Get;
                case HttpRequestMethod.Post:
                    return HttpMethod.Post;
                case HttpRequestMethod.Head:
                    return HttpMethod.Head;
                case HttpRequestMethod.Trace:
                    return HttpMethod.Trace;
                case HttpRequestMethod.Put:
                    return HttpMethod.Put;
                case HttpRequestMethod.Delete:
                    return HttpMethod.Delete;
                case HttpRequestMethod.Options:
                    return HttpMethod.Options;
                case HttpRequestMethod.Connect:
                    return _HttpMethodConnect;
                case HttpRequestMethod.Custom:
                    return new HttpMethod(request.HttpMethod);
                case HttpRequestMethod.Patch:
                    return _HttpMethodPatch;
                default:
                    return HttpMethod.Get;
            }
        }

        #region 异步客户端不需要实现以下方法

        IAsyncResult IHttpClient.BeginSend(IHttpRequest request, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        IHttpResponse IHttpClient.EndSend(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        IHttpResponse IHttpClient.Send(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}