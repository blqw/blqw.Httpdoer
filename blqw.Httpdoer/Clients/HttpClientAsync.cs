using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Web
{
    public sealed class HttpClientAsync : IHttpClient
    {
        static readonly HttpClient _Client = GetOnlyHttpClient();

        private static System.Net.Http.HttpClient GetOnlyHttpClient()
        {
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            handler.MaxAutomaticRedirections = 10;
            handler.UseCookies = false;
            handler.AutomaticDecompression = DecompressionMethods.GZip;
            handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
            var www = new HttpClient(handler);
            www.Timeout = new TimeSpan(0, 0, 30);
            www.MaxResponseContentBufferSize = int.MaxValue;
            return www;
        }

        public async Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken)
        {
            var timer = HttpTimer.Start();
            var data = default(HttpRequestData);
            try
            {
                request.OnInitialize();
                data = new HttpRequestData(request);
                var www = GetRequest(data);
                timer.Readied();
                request.OnSending();
                using (var source1 = new CancellationTokenSource(data.Timeout.TotalMilliseconds >= int.MaxValue ? int.MaxValue : (int)data.Timeout.TotalMilliseconds))
                using (var source2 = CancellationTokenSource.CreateLinkedTokenSource(source1.Token, cancellationToken))
                {
                    var response = await _Client.SendAsync(www, source2.Token);
                    timer.Sent();
                    var cookies = data.Cookies;
                    while (request.AutoRedirect && response.StatusCode == HttpStatusCode.Redirect) //手动处理302的请求
                    {
                        request.Debug("StatusCode=302; 正在重定向...");
                        if (cookies == null)
                        {
                            cookies = new CookieContainer(); //302时必须使用 cookie
                        }
                        SetCookies(response, cookies);
                        www = GetRequest(data, response.Headers.Location); //构建新的请求
                        response = await _Client.SendAsync(www, source2.Token);
                    }
                    request.Response = await Transfer(response, request.CookieMode != HttpCookieMode.None);
                    SetCookies(response, cookies);
                    request.OnEnd(request.Response);
                }
            }
            catch (Exception ex)
            {
                timer.Error();
                if (ex is TaskCanceledException)
                {
                    ex = new TimeoutException("请求已超时");
                }
                var res = new HttpResponse();
                res.Exception = ex;
                request.Response = res;
                request.OnError(res);
            }
            finally
            {
                timer.Ending();
                request.Debug(timer.ToString());
            }
            ((HttpResponse)request.Response).RequestData = data;
            return request.Response;
        }


        private async Task<HttpResponse> Transfer(HttpResponseMessage response, bool useCookies)
        {
            if (response == null)
            {
                return new HttpResponse { StatusCode = 0 };
            }
            var contentType = (HttpContentType)response.Content.Headers.ContentType?.ToString();
            var res = new HttpResponse
            {
                Headers = new HttpHeaders(),
            };
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

        private HttpRequestMessage GetRequest(HttpRequestData data, Uri redirect = null)
        {
            var url = data.Url;
            if (redirect != null)
            {
                url = new Uri(new Uri(data.Url), redirect).ToString();
            }
            var request = data.Request;
            request.Debug(url);
            var www = new HttpRequestMessage(GetHttpMethod(request), url)
            {
                Version = data.Version
            };
            if (data.Body != null)
            {
                www.Content = new ByteArrayContent(data.Body ?? _BytesEmpty);
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


        static readonly byte[] _BytesEmpty = new byte[0];
        static readonly HttpMethod _HttpMethod_CONNECT = new HttpMethod("CONNECT");

        /// <summary> 获取 HttpMethod
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
                    return _HttpMethod_CONNECT;
                case HttpRequestMethod.Custom:
                    return new HttpMethod(request.HttpMethod);
                default:
                    return HttpMethod.Get;
            }
        }


        #region NotImplementedException

        public IAsyncResult BeginSend(IHttpRequest request, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IHttpResponse EndSend(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }

        public IHttpResponse Send(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
