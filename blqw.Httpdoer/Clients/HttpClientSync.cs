using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using blqw.IOC;

namespace blqw.Web
{
    /// <summary>
    /// 客户端请求的同步与Begin实现
    /// </summary>
    public sealed class HttpClientSync : IHttpClient
    {
        /// <summary>
        /// 反射系统类内部实现方法
        /// </summary>
        private static readonly Action<WebHeaderCollection, string, string> HeaderAddInternal = (Action<WebHeaderCollection, string, string>)
        (
            typeof(WebHeaderCollection).GetMethod("ChangeInternal", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(string), typeof(string) }, null)
            ??
            typeof(WebHeaderCollection).GetMethod("AddWithoutValidate", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(string), typeof(string) }, null)
        ).CreateDelegate(typeof(Action<WebHeaderCollection, string, string>));

        /// <summary>
        /// 字符串缓冲
        /// </summary>
        [ThreadStatic]
        private static byte[] _Buffer;

        static HttpClientSync()
        {
            //用于https的请求验证票据
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
        }


        /// <summary>
        /// 同步发送 HTTP 请求
        /// </summary>
        /// <param name="request"> 请求对象 </param>
        /// <returns> </returns>
        public IHttpResponse Send(IHttpRequest request)
        {
            var timer = HttpRequestTimer.OnStart();
            var data = default(HttpRequestData);
            try
            {
                request.OnInitialize();
                data = new HttpRequestData(request);
                request.Logger?.Write(TraceEventType.Verbose, () => data.Raw);
                var www = Convert(data);
                timer.OnReady();
                request.OnSending();
                var response = (HttpWebResponse)www.GetResponse();
                timer.OnSend();
                request.Response = Convert(response, request.CookieMode != HttpCookieMode.None);
                request.OnEnd(request.Response);
            }
            catch (WebException ex)
            {
                timer.OnError();
                var res = Convert((HttpWebResponse)ex.Response, request.CookieMode != HttpCookieMode.None);
                res.Exception = ex;
                request.Response = res;
                request.Logger?.Write(TraceEventType.Error, "请求中出现错误", ex);
                request.OnError(res);
            }
            finally
            {
                timer.OnEnd();
                request.Logger?.Write(TraceEventType.Verbose, () => request.Response.ResponseRaw);
                request.Logger?.Write(TraceEventType.Information, timer.ToString());
            }
            ((HttpResponse)request.Response).RequestData = data;
            return request.Response;
        }

        #region 同步客户端不需要实现这些方法

        Task<IHttpResponse> IHttpClient.SendAsync(IHttpRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 发送 HTTP 请求,并设置回调方法
        /// </summary>
        /// <param name="request"> 请求对象 </param>
        /// <param name="callback"> 异步回调 </param>
        /// <param name="state"> 回调参数 </param>
        /// <returns> </returns>
        public IAsyncResult BeginSend(IHttpRequest request, AsyncCallback callback, object state) => new HttpClientBeginResult(request, callback, state);

        /// <summary>
        /// 完成HTTP请求,并获取响应
        /// </summary>
        /// <param name="asyncResult"> 异步操作对象 </param>
        /// <returns> </returns>
        /// <exception cref="ArgumentException"> <paramref name="asyncResult" />类型错误或值为null </exception>
        public IHttpResponse EndSend(IAsyncResult asyncResult)
        {
            var result = asyncResult as HttpClientBeginResult;
            if (result == null)
            {
                throw new ArgumentException("类型错误或值为null", nameof(asyncResult));
            }
            if (result.IsCompleted == false)
            {
                result.OnCallback(asyncResult);
            }
            return result.Response;
        }

        /// <summary>
        /// 将<seealso cref="HttpRequestData" />转换为<seealso cref="HttpWebRequest" />对象
        /// </summary>
        /// <param name="data"> 待转换的对象 </param>
        /// <param name="redirect"> 重定向地址,用于重写<see cref="HttpRequestData.Url" /> </param>
        /// <returns> </returns>
        private static HttpWebRequest Convert(HttpRequestData data, Uri redirect = null)
        {
            var url = redirect?.ToString() ?? data.Url;
            var request = data.Request;

            request.Logger.Write(TraceEventType.Information, url);
            var www = WebRequest.CreateHttp(url);
            request.Version = data.Version;
            www.ContinueTimeout = 3000;
            www.ReadWriteTimeout = 3000;
            www.Timeout = data.Timeout.TotalMilliseconds >= int.MaxValue ? int.MaxValue : (int)data.Timeout.TotalMilliseconds;
            www.Method = data.Method;
            www.AllowAutoRedirect = request.AutoRedirect;
            www.AutomaticDecompression = DecompressionMethods.GZip;
            www.Proxy = data.Proxy; //?? GlobalProxySelection.GetEmptyWebProxy();
            //必须要先设置头再设置body,否则头会被清掉
            foreach (var header in data.Headers)
            {
                if (header.Key == nameof(www.Connection))
                {
                    var connection = request.Headers[nameof(www.Connection)];
                    if (string.Equals(connection, "Keep-Alive", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(connection, "Close", StringComparison.OrdinalIgnoreCase))
                    {
                        www.KeepAlive = request.Headers.KeepAlive;
                        continue;
                    }
                }
                if (header.Key == "Cookie")
                {
                    continue;
                }
                //防止中文引起的头信息乱码
                var decode = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.UTF8.GetBytes(header.Value));
                HeaderAddInternal(www.Headers, header.Key, decode);
            }
            if (data.Body?.Length > 0)
            {
                www.ContentLength = data.Body.Length;
                using (var req = www.GetRequestStream())
                {
                    req.Write(data.Body, 0, data.Body.Length);
                }
            }

            www.CookieContainer = data.Cookies;
            return www;
        }

        /// <summary>
        /// 将<seealso cref="HttpWebResponse" />转换为<seealso cref="HttpResponse" />
        /// </summary>
        /// <param name="response"> 待转换的对象 </param>
        /// <param name="useCookies"> 是否使用Cookie </param>
        /// <returns> </returns>
        private static HttpResponse Convert(HttpWebResponse response, bool useCookies)
        {
            if (response == null)
            {
                return new HttpResponse { StatusCode = 0 };
            }
            var res = new HttpResponse
            {
                Headers = new HttpHeaders()
            };
            using (response)
            {
                var headers = response.Headers;
                foreach (var key in headers.AllKeys)
                {
                    var values = headers.GetValues(key);
                    if (values == null)
                    {
                        continue;
                    }
                    foreach (var value in values)
                    {
                        res.Headers.Add(key, value);
                    }
                }
                var contentType = (HttpContentType)response.ContentType;
                if (contentType.Charset == null && string.IsNullOrWhiteSpace(response.ContentEncoding) == false)
                {
                    contentType = contentType.ChangeCharset(Encoding.GetEncoding(response.ContentEncoding));
                }
                res.Body = new HttpBody(contentType, GetBytes(response));
                if (useCookies)
                {
                    res.Cookies = response.Cookies;
                    //res.Cookies = new CookieCollection();
                    //foreach (var cookie in response.Cookies.Cast<Cookie>().Where(it=>!it.Expired))
                    //{
                    //    res.Cookies.Add(cookie);
                    //}
                }
                res.StatusCode = response.StatusCode;
                res.Status = response.StatusDescription;
                res.SchemeVersion = $"{response.ResponseUri.Scheme.ToUpperInvariant()}/{response.ProtocolVersion}";
                res.IsSuccessStatusCode = ((int)response.StatusCode >= 200) && ((int)response.StatusCode <= 299);
            }
            return res;
        }

        private static byte[] GetBytes(HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                return ReadAll(stream);
            }
        }

        /// <summary>
        /// 读取流中的所有字节
        /// </summary>
        /// <param name="stream"> </param>
        private static byte[] ReadAll(Stream stream)
        {
            var length = 1024;
            if (_Buffer == null)
            {
                _Buffer = new byte[length];
            }
            length = _Buffer.Length;
            var bytes = new List<byte>();
            int count;
            do
            {
                if ((count = stream.Read(_Buffer, 0, length)) == length)
                {
                    bytes.AddRange(_Buffer);
                }
                else
                {
                    bytes.AddRange(_Buffer.Take(count));
                }
            } while (count > 0);
            return bytes.ToArray();
        }

        /// <summary>
        /// 异步请求的返回对象
        /// </summary>
        private class HttpClientBeginResult : IAsyncResult
        {
            /// <summary>
            /// 异步回调委托
            /// </summary>
            private readonly AsyncCallback _asyncCallback;

            /// <summary>
            /// 由<seealso cref="HttpWebRequest.BeginGetResponse" />产生的异步对象
            /// </summary>
            private readonly IAsyncResult _asyncResult;

            /// <summary>
            /// 请求对象
            /// </summary>
            private readonly IHttpRequest _request;

            /// <summary>
            /// 请求数据
            /// </summary>
            private readonly HttpRequestData _requestData;

            /// <summary>
            /// 真实请求对象
            /// </summary>
            private readonly HttpWebRequest _webRequest;

            /// <summary>
            /// 计时器
            /// </summary>
            private HttpRequestTimer _timer;

            /// <summary>
            /// 初始化异步请求对象
            /// </summary>
            /// <param name="request"> </param>
            /// <param name="callback"> </param>
            /// <param name="state"> </param>
            public HttpClientBeginResult(IHttpRequest request, AsyncCallback callback, object state)
            {
                _asyncCallback = callback;
                _timer = HttpRequestTimer.OnStart();

                _request = request;
                request.OnInitialize();
                _requestData = new HttpRequestData(request);
                _webRequest = Convert(_requestData);
                _timer.OnReady();
                request.OnSending();

                _asyncResult = _webRequest.BeginGetResponse(OnCallback, state);
            }

            /// <summary>
            /// 用于保存异步返回的响应对象
            /// </summary>
            public IHttpResponse Response { get; private set; }


            /// <summary>
            /// 异步参数
            /// </summary>
            public object AsyncState => _asyncResult.AsyncState;

            /// <summary>
            /// 获取用于等待异步操作完成的 <see cref="T:System.Threading.WaitHandle" />。
            /// </summary>
            /// <returns> 用于等待异步操作完成的 <see cref="T:System.Threading.WaitHandle" />。 </returns>
            /// <filterpriority> 2 </filterpriority>
            public WaitHandle AsyncWaitHandle => _asyncResult.AsyncWaitHandle;

            /// <summary>
            /// 获取一个值，该值指示异步操作是否同步完成。
            /// </summary>
            /// <returns> 如果异步操作同步完成，则为 true；否则为 false。 </returns>
            /// <filterpriority> 2 </filterpriority>
            public bool CompletedSynchronously => _asyncResult.CompletedSynchronously;

            /// <summary>
            /// 获取一个值，该值指示异步操作是否已完成。
            /// </summary>
            /// <returns> 如果操作完成则为 true，否则为 false。 </returns>
            /// <filterpriority> 2 </filterpriority>
            public bool IsCompleted { get; private set; }

            /// <summary>
            /// 异步回调
            /// </summary>
            /// <param name="ar"> </param>
            public void OnCallback(IAsyncResult ar)
            {
                if (IsCompleted)
                {
                    return;
                }
                lock (this)
                {
                    if (IsCompleted)
                    {
                        return;
                    }
                    try
                    {
                        var response = (HttpWebResponse)_webRequest.EndGetResponse(ar);
                        _timer.OnSend();
                        Response = _request.Response = Convert(response, _request.CookieMode != HttpCookieMode.None);
                        _request.OnEnd(Response);
                    }
                    catch (WebException ex)
                    {
                        _timer.OnError();
                        var res = Convert((HttpWebResponse)ex.Response, _request.CookieMode != HttpCookieMode.None);
                        res.Exception = ex;
                        Response = _request.Response = res;
                        _request.Logger?.Write(TraceEventType.Error, "异步请求中出现错误", ex);
                        _request.OnError(Response);
                    }
                    finally
                    {
                        _timer.OnEnd();
                        _request.Logger?.Write(TraceEventType.Information, _timer.ToString());
                        IsCompleted = true;
                        var res = Response as HttpResponse;
                        if (res != null)
                        {
                            res.RequestData = _requestData;
                        }
                    }
                }
                try
                {
                    _asyncCallback(this);
                }
                catch (Exception ex)
                {
                    _request.Logger?.Write(TraceEventType.Error, "异步回调中出现错误", ex);
                    Debug.Assert(Response != null, "Response != null");
                    ((HttpResponse)Response).Exception = ex;
                    _request.OnError(Response);
                }
            }
        }
    }
}