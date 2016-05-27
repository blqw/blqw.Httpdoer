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
            var timer = Stopwatch.StartNew();
            var www = GetRequest(request);
            long init = timer.ElapsedMilliseconds;
            timer.Restart();
            long send = 0;
            long end = 0;
            long err = 0;
            try
            {
                var response = (HttpWebResponse)www.GetResponse();
                send = timer.ElapsedMilliseconds;
                timer.Restart();
                return Transfer(response).WriteLog();
            }
            catch (WebException ex)
            {
                err = timer.ElapsedMilliseconds;
                timer.Restart();
                var res = Transfer((HttpWebResponse)ex.Response);
                res.Exception = ex;
                return res.WriteLog();
            }
            finally
            {
                end = timer.ElapsedMilliseconds;
                timer.Stop();
                if (send > 0)
                {
                    Trace.WriteLine($"init:{init} ms, send:{send} ms, end:{end} ms", "HttpRequest.Timing");
                }
                else
                {
                    Trace.WriteLine($"init:{init} ms, err:{err} ms, end:{end} ms", "HttpRequest.Timing");
                }
            }
        }

        private static HttpWebRequest GetRequest(IHttpRequest request)
        {
            var data = new HttpRequestData(request);

            Trace.WriteLine(data.Url.ToString(), "HttpRequest.Url");
            var www = WebRequest.CreateHttp(data.Url);
            if (request.Version != null)
            {
                www.ProtocolVersion = request.Version;
            }
            else if (data.Url.Scheme == Uri.UriSchemeHttps)
            {
                www.ProtocolVersion = HttpVersion.Version10;
            }
            www.CookieContainer = request.Cookie;
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

        private HttpResponse Transfer(HttpWebResponse response)
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
                res.Cookie = new CookieContainer();
                res.Cookie.Add(response.Cookies);
                res.StatusCode = response.StatusCode;
                res.IsSuccessStatusCode = (int)response.StatusCode >= 200 && (int)response.StatusCode <= 299;
            }
            return res;
        }

        private byte[] GetBytes(HttpWebResponse response)
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

        /// <summary> 
        /// 读取流中的所有字节
        /// </summary>
        /// <param name="stream"></param>
        private static IEnumerable<byte> ReadAll(Stream stream)
        {
            int length = 1024;
            byte[] buffer = new byte[length];
            int index = 0;
            while ((index = stream.Read(buffer, 0, length)) > 0)
            {
                for (int i = 0; i < index; i++)
                {
                    yield return buffer[i];
                }
            }
        }















        public IAsyncResult BeginSend(IHttpRequest request, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IHttpResponse EndSend(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }
        
        public Task<IHttpResponse> SendAsync(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IHttpResponse> SendAsync(IHttpRequest request, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
