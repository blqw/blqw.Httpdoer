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
            (typeof(WebHeaderCollection).GetMethod("ChangeInternal", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(string) }, null)
            ??
            typeof(WebHeaderCollection).GetMethod("AddWithoutValidate", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(string) }, null)).CreateDelegate(typeof(Action<WebHeaderCollection, string, string>));


        public IHttpResponse Send(IHttpRequest request)
        {
            var timer = HttpTimer.Start();
            try
            {
                (request as IHttpTracking)?.OnInitialize(request);
                var www = GetRequest(request);
                timer.Readied();
                (request as IHttpTracking)?.OnSending(request);
                var response = (HttpWebResponse)www.GetResponse();
                timer.Sent();
                request.Response = Transfer(request.UseCookies, response);
                (request as IHttpTracking)?.OnEnd(request, request.Response);
            }
            catch (WebException ex)
            {
                timer.Error();
                var res = Transfer(request.UseCookies, (HttpWebResponse)ex.Response);
                res.Exception = ex;
                request.Response = res;
                (request as IHttpTracking)?.OnError(request, res);
            }
            finally
            {
                timer.Ending();
                (request as IHttpLogger)?.Debug(timer.ToString());
            }
            return request.Response;
        }

        private static HttpWebRequest GetRequest(IHttpRequest request)
        {
            var data = new HttpRequestData(request);

            (request as IHttpLogger)?.Debug(data.Url.ToString());
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
            www.Method = request.HttpMethod;

            //必须要先设置头再设置body,否则头会被清掉
            foreach (var header in data.Headers)
            {
                //防止中文引起的头信息乱码
                var transfer = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.UTF8.GetBytes(header.Value));
                HeaderAddInternal(www.Headers, header.Key, transfer);
            }

            if (data.Body?.Length > 0)
            {
                www.ContentLength = data.Body.Length;
                using (var req = www.GetRequestStream())
                {
                    req.Write(data.Body, 0, data.Body.Length);
                }
            }

            return www;
        }
        
        private static HttpResponse Transfer(bool useCookies, HttpWebResponse response)
        {
            if (response == null)
            {
                return new HttpResponse() { StatusCode = 0 };
            }
            var contentType = (HttpContentType)response.ContentType;
            var res = new HttpResponse();
            using (response)
            {
                var headers = response.Headers;
                foreach (var key in headers.AllKeys)
                {
                    foreach (var value in headers.GetValues(key))
                    {
                        res.Headers.Add(key, value);
                    }
                }
                res.Body = new HttpBody(contentType, GetBytes(response));
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
                    (value as IHttpTracking)?.OnInitialize(value);
                    _WebRequest = GetRequest(value);
                    _Timer.Readied();
                    (value as IHttpTracking)?.OnSending(value);
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
                        Response = Request.Response = Transfer(Request.UseCookies, response);
                        (Request as IHttpTracking)?.OnEnd(Request, Response);
                    }
                    catch (WebException ex)
                    {
                        _Timer.Error();
                        var res = Transfer(Request.UseCookies, (HttpWebResponse)ex.Response);
                        res.Exception = ex;
                        Response = Request.Response = res;
                        (Request as IHttpTracking)?.OnError(Request, Response);
                    }
                    finally
                    {
                        _Timer.Ending();
                        (Request as IHttpLogger)?.Debug(_Timer.ToString());
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
