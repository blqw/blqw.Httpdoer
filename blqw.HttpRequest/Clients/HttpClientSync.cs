using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Web
{
    public sealed class HttpClientSync : IHttpClient
    {
        static HttpClientSync()
        {
            //用于https的请求验证票据
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
        }

        static readonly Action<WebHeaderCollection, string, string> HeaderAddInternal = (Action<WebHeaderCollection, string, string>)
            (typeof(WebHeaderCollection).GetMethod("AddInternal", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(string) }, null)
            ??
            typeof(WebHeaderCollection).GetMethod("AddWithoutValidate", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(string) }, null)).CreateDelegate(typeof(Action<WebHeaderCollection, string, string>));


        public IHttpResponse Send(IHttpRequest request)
        {
            var timer = HttpTimer.Start();
            var www = GetRequest(request);
            timer.Readied();
            try
            {
                var response = (HttpWebResponse)www.GetResponse();
                timer.Sent();
                return request.Response = Transfer(request.UseCookies, response);
            }
            catch (WebException ex)
            {
                timer.Error();
                var res = Transfer(request.UseCookies, (HttpWebResponse)ex.Response);
                res.Exception = ex;
                return request.Response = res;
            }
            finally
            {
                timer.Ending();
                request.Logger.Debug(timer.ToString());
            }
        }

        private static HttpWebRequest GetRequest(IHttpRequest request)
        {
            var data = new HttpRequestData(request);

            request.Logger.Debug(data.Url.ToString());
            var www = WebRequest.CreateHttp(data.Url);
            if (request.Version != null)
            {
                www.ProtocolVersion = request.Version;
            }
            else if (data.Url.Scheme == Uri.UriSchemeHttps)
            {
                www.ProtocolVersion = HttpVersion.Version10;
            }

            if (request.UseCookies)
            {
                www.CookieContainer = request.Cookies;
            }

            www.ContinueTimeout = 3000;
            www.ReadWriteTimeout = 3000;
            www.Timeout = (int)request.Timeout.TotalMilliseconds;
            www.Method = GetMethodName(request.Method);

            if (data.Body?.Length > 0)
            {
                www.ContentLength = data.Body.Length;
                using (var req = www.GetRequestStream())
                {
                    req.Write(data.Body, 0, data.Body.Length);
                }
            }
            foreach (var header in request.Headers)
            {
                //防止中文引起的头信息乱码
                var transfer = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.UTF8.GetBytes(header.Value));
                HeaderAddInternal(www.Headers, header.Key, transfer);
            }

            return www;
        }

        /// <summary> 获取 HttpMethod
        /// </summary>
        /// <summary> 获取 HttpMethod 枚举的字符串
        /// </summary>
        private static string GetMethodName(HttpRequestMethod method)
        {
            switch (method)
            {
                case HttpRequestMethod.Get:
                    return "GET";
                case HttpRequestMethod.Post:
                    return "POST";
                case HttpRequestMethod.Head:
                    return "HEAD";
                case HttpRequestMethod.Trace:
                    return "TRACE";
                case HttpRequestMethod.Put:
                    return "PUT";
                case HttpRequestMethod.Delete:
                    return "DELETE";
                case HttpRequestMethod.Options:
                    return "OPTIONS";
                case HttpRequestMethod.Connect:
                    return "CONNECT";
                default:
                    return "GET";
            }
        }

        private static HttpResponse Transfer(bool useCookies, HttpWebResponse response)
        {
            var contentType = (HttpContentType)response.ContentType;
            var parser = contentType.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            if (parser == null)
            {
                throw new FormatException($"无法获取{nameof(IHttpBodyParser)}");
            }
            var res = new HttpResponse();
            using (response)
            {
                res.Body = parser.Deserialize(GetBytes(response), contentType);
                if (useCookies)
                {
                    res.Cookies = response.Cookies;
                }
                res.StatusCode = response.StatusCode;
                res.IsSuccessStatusCode = (int)response.StatusCode >= 200 && (int)response.StatusCode <= 299;
            }
            return res;
        }

        private static byte[] GetBytes(HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                if ("gzip".Equals(response.ContentEncoding, StringComparison.OrdinalIgnoreCase))
                {
                    using (var gzip = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        return ReadAll(gzip).ToArray();
                    }
                }
                return ReadAll(stream).ToArray();
            }
        }

        [ThreadStatic]
        static byte[] _buffer;

        /// <summary> 
        /// 读取流中的所有字节
        /// </summary>
        /// <param name="stream"></param>
        private static IEnumerable<byte> ReadAll(Stream stream)
        {
            int length = 1024;
            if (_buffer == null)
            {
                _buffer = new byte[length];
            }
            int index = 0;
            while ((index = stream.Read(_buffer, 0, length)) > 0)
            {
                for (int i = 0; i < index; i++)
                {
                    yield return _buffer[i];
                }
            }
        }

        #region NotImplemented

        public Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion

        public IAsyncResult BeginSend(IHttpRequest request, AsyncCallback callback, object state)
        {
            var result = new HttpClientBeginResult(callback, state);
            result.Request = request;
            return result;
        }

        public IHttpResponse EndSend(IAsyncResult asyncResult)
        {
            var result = asyncResult as HttpClientBeginResult;
            if (result == null)
            {
                throw new ArgumentException("类型错误或值为null",nameof(asyncResult));
            }
            if (result.IsCompleted == false)
            {
                result.Callback(asyncResult);
            }
            return result.Response;
        }



        class HttpClientBeginResult : IAsyncResult
        {
            private object _State;
            private IAsyncResult _AsyncResult;
            private AsyncCallback _AsyncCallback;
            private HttpTimer _Timer;
            private HttpWebRequest _WebRequest;
            private IHttpRequest _Request;
            public IHttpResponse Response { get; private set; }
            public HttpClientBeginResult(AsyncCallback callback, object state)
            {
                _AsyncCallback = callback;
                _State = state;
                _Timer = HttpTimer.Start();
            }

            public IHttpRequest Request
            {
                get
                {
                    return _Request;
                }
                set
                {
                    _Request = value;
                    _WebRequest = GetRequest(value);
                    _Timer.Readied();
                    _AsyncResult = _WebRequest.BeginGetResponse(Callback, _State);
                }
            }

            public void Callback(IAsyncResult ar)
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
                        var response = (HttpWebResponse)_WebRequest.EndGetResponse(ar);
                        _Timer.Sent();
                        Response = _Request.Response = Transfer(_Request.UseCookies, response);
                    }
                    catch (WebException ex)
                    {
                        _Timer.Error();
                        var res = Transfer(_Request.UseCookies, (HttpWebResponse)ex.Response);
                        res.Exception = ex;
                        Response = _Request.Response = res;
                    }
                    finally
                    {
                        _Timer.Ending();
                        Request.Logger.Debug(_Timer.ToString());
                        IsCompleted = true;
                    }
                }
                _AsyncCallback(this);
            }

            

            public void Readied()
            {
                _Timer.Readied();
            }

            public object AsyncState
            {
                get
                {
                    return _AsyncResult.AsyncState;
                }
            }

            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    return _AsyncResult.AsyncWaitHandle;
                }
            }

            public bool CompletedSynchronously
            {
                get
                {
                    return _AsyncResult.CompletedSynchronously;
                }
            }

            public bool IsCompleted { get; set; }

        }
    }
}
