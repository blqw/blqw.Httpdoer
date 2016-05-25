using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary> 发起 Http 请求的工具类
    /// </summary>
    public sealed class HttpRequest : IFormattable
    {

        public static bool IsInitialized { get; } = Initialize();

        private static bool Initialize()
        {
            //并发请求数
            ServicePointManager.DefaultConnectionLimit = 1024;
            return false;
        }

        /// <summary> 初始化http请求
        /// </summary>
        public HttpRequest()
        {
            Method = HttpRequestMethod.GET;
            Encoding = Encoding.UTF8;
            Timeout = new TimeSpan(0, 0, 15); //默认15秒超时
        }

        /// <summary> 初始化http请求,并设定基路径
        /// </summary>
        /// <param name="baseUrl">基路径</param>
        public HttpRequest(string baseUrl)
            : this()
        {
            if (baseUrl == null)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }
            if (baseUrl.Length > 8 &&
                (baseUrl[0] == ':' || //::1 :81 这种地址
                (baseUrl[3] != ':' && baseUrl[4] != ':' && baseUrl[5] != ':')))
            {
                baseUrl = "http://" + baseUrl;
            }
            Uri uri;
            if (Uri.TryCreate(baseUrl, UriKind.Absolute, out uri) == false)
            {
                throw new UriFormatException(nameof(baseUrl) + "有误");
            }
            BaseUrl = uri;
        }

        /// <summary> 基路径
        /// </summary>
        public Uri BaseUrl { get; set; }
        /// <summary> 基路径的相对路径
        /// </summary>
        public string Path { get; set; }
        /// <summary> 请求方式
        /// </summary>
        public HttpRequestMethod Method { get; set; }
        /// <summary> 请求编码
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary> 超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// 是否需要保持连接
        /// </summary>
        public bool KeepAlive { get; set; }

        HttpHeaders _Headers;
        HttpQueryString _QueryString;
        HttpFormBody _FormBody;
        CookieContainer _Cookie;
        Action _Abort;

        /// <summary> 请求头
        /// </summary>
        public HttpHeaders Headers
        {
            get { return _Headers ?? (_Headers = new HttpHeaders()); }
            set { _Headers = value; }
        }


        /// <summary> Url参数
        /// </summary>
        public HttpQueryString QueryString
        {
            get { return _QueryString ?? (_QueryString = new HttpQueryString()); }
            set { _QueryString = value; }
        }
        /// <summary> From参数
        /// </summary>
        public HttpFormBody FormBody
        {
            get { return _FormBody ?? (_FormBody = new HttpFormBody()); }
            set { _FormBody = value; }
        }
        /// <summary> Cookie
        /// </summary>
        public CookieContainer Cookie
        {
            get { return _Cookie ?? (_Cookie = new CookieContainer()); }
            set { _Cookie = value; }
        }

        /// <summary> 请求返回状态
        /// </summary>
        public HttpStatusCode ResponseCode { get; private set; }

        /// <summary> 异常
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary> 获取请求的字符串
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetString()
        {
            var str = (Encoding ?? Encoding.UTF8).GetString(await GetBytes().ConfigureAwait(false));
            Trace.WriteLine(str, "HttpRequest.Result");
            return str;
        }

        /// <summary> 获取请求字节 
        /// </summary>
        public async Task<byte[]> GetBytes()
        {
            using (var response = await GetResponse())
            {
                if (response == null)
                {
                    return new byte[0];
                }
                var timer = Stopwatch.StartNew();
                var bytes = await response.Content.ReadAsByteArrayAsync();
                timer.Stop();
                Trace.WriteLine($"timing: {timer.ElapsedMilliseconds}; length:{bytes.Length}", "HttpRequest.ReadBytes");
                return bytes;
            }
        }

        static readonly HttpClient HttpClient = GetOnlyHttpClient();

        private static HttpClient GetOnlyHttpClient()
        {
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = true;
            handler.MaxAutomaticRedirections = 10;
            handler.UseCookies = false;
            handler.AutomaticDecompression = DecompressionMethods.GZip;
            handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
            var www = new HttpClient(handler);
            www.Timeout = new TimeSpan(0, 0, 30);
            www.MaxResponseContentBufferSize = int.MaxValue;
            www.DefaultRequestHeaders.Add("User-Agent", HttpHeaders.DefaultUserAgent);
            return www;
        }

        /// <summary>
        /// 获取相应对象
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetResponse()
        {
            var timer = Stopwatch.StartNew();
            long pre = 0,
                setheader = 0,
                setbody = 0,
                res = 0,
                acceptcookie = 0,
                acceptheader = 0,
                error = 0,
                end = 0;

            if (Encoding == null)
            {
                Encoding = Encoding.UTF8;
            }
            if (PreRequest != null)
            {
                await PreRequest.ConfigureAwait(false);
                PreRequest = null;
                pre = timer.ElapsedMilliseconds;
                timer.Restart();
            }
            ResponseCode = 0;
            Exception = null;
            Uri uri = GetFullUrl();

            Trace.WriteLine(uri.ToString(), "HttpRequest.Url");

            var cancel = new CancellationTokenSource(Timeout);
            try
            {
                _Abort = cancel.Cancel;
                var message = new HttpRequestMessage(GetHttpMethod(), uri);

                if (Cookie?.Count > 0) message.Headers.Add("Cookie", Cookie.GetCookieHeader(uri));
                if (KeepAlive) message.Headers.Connection.Add("keep-alive");

                if (_FormBody != null)
                {
                    var formdata = FormBody.GetBytes(Encoding);
                    message.Content = new ByteArrayContent(formdata);

                    message.Content.Headers.Add("Content-Type", FormBody.ContentType);
                    message.Content.Headers.ContentType.CharSet = Encoding.WebName;
                    //message.Headers.TryAddWithoutValidation("Content-Type", "charset=" + Encoding.WebName);

                    setbody = timer.ElapsedMilliseconds;
                    timer.Restart();
                }

                if (_Headers?.Count > 0)
                {
                    foreach (var item in _Headers)
                    {
                        var arr = item.Value as IEnumerable<string>;
                        if (arr != null)
                        {
                            if (!message.Headers.TryAddWithoutValidation(item.Key, arr)
                                && message.Content != null)
                            {
                                message.Content.Headers.TryAddWithoutValidation(item.Key, arr);
                            }
                        }
                        else
                        {

                            var str = item.Value as string ?? item.Value + "";
                            if (!message.Headers.TryAddWithoutValidation(item.Key, str)
                                && message.Content != null)
                            {
                                message.Content.Headers.TryAddWithoutValidation(item.Key, str);
                            }
                        }

                    }
                    setheader = timer.ElapsedMilliseconds;
                    timer.Restart();
                }


                var response = await HttpClient.SendAsync(message, cancel.Token);
                _Abort = null;
                res = timer.ElapsedMilliseconds;
                timer.Restart();
                if (AcceptCookie && response.Headers.Contains("Set-Cookie"))
                {
                    var setcookies = response.Headers.GetValues("Set-Cookie");
                    foreach (var cookie in setcookies)
                    {
                        Cookie.SetCookies(uri, cookie);
                    }
                    acceptcookie = timer.ElapsedMilliseconds;
                    timer.Restart();
                }
                if (AcceptHeader)
                {
                    Headers.Clear();
                    foreach (var head in response.Headers)
                    {
                        Headers.Add(head.Key, head.Value);
                    }
                    foreach (var head in response.Content.Headers)
                    {
                        Headers.Add(head.Key, head.Value);
                    }
                    acceptheader = timer.ElapsedMilliseconds;
                    timer.Restart();
                }
                ResponseCode = response.StatusCode;
                return response;

            }
            catch (Exception ex)
            {
                error = timer.ElapsedMilliseconds;
                timer.Restart();
                if (ex is TaskCanceledException && _Abort != null)
                {
                    ex = new TimeoutException("请求已超时");
                }
                Trace.WriteLine(ex.Message, "HttpRequest.Error");
                Exception = ex;
                ResponseCode = 0;
                return null;
            }
            finally
            {
                cancel?.Dispose();
                _Abort = null;
                setbody = timer.ElapsedMilliseconds;
                timer.Stop();
                Trace.WriteLine(ResponseCode, "HttpRequest.StatusCode");
                Trace.WriteLine(
                    $"pre={pre}; setheader={setheader}; setbody={setbody}; response={res}; acceptcookie={acceptcookie}; acceptheader={acceptheader}; error={error}; end={end}",
                    "HttpRequest.Timing");
            }
        }

        /// <summary>
        /// 取消请求
        /// </summary>
        public void Abort()
        {
            var abort = _Abort;
            if (abort != null)
            {
                _Abort = null;
                abort();
            }
        }

        /// <summary> 
        /// 获取完全的请求路径
        /// </summary>
        private Uri GetFullUrl()
        {
            Uri baseUrl = BaseUrl;
            var url = Path;
            Uri uri;
            if (url == null)
            {
                if (baseUrl == null)
                {
                    throw new ArgumentNullException("BaseUrl + Path");
                }
                uri = BaseUrl;
            }
            else if (BaseUrl == null)
            {
                if (Uri.TryCreate(url, UriKind.Absolute, out uri) == false)
                {
                    throw new UriFormatException("UrlError");
                }
            }
            else if (Uri.TryCreate(BaseUrl + url, UriKind.Absolute, out uri) == false)
            {
                throw new UriFormatException("UrlError");
            }

            if (_QueryString == null)
            {
                return uri;
            }
            var query = QueryString.ToString();
            if (query.Length > 0)
            {
                query = uri.Query + (uri.Query.Length > 1 ? "&" : "?") + query;
            }

            return new Uri(uri, query);
        }

        /// <summary> 读取流中的所有字节
        /// </summary>
        /// <param name="stream"></param>
        private static IEnumerable<byte> ReadAll(Stream stream)
        {
            if (stream.CanTimeout)
            {
                stream.ReadTimeout = 3000;
            }
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

        static readonly HttpMethod HttpMethod_CONNECT = new HttpMethod("CONNECT");
        /// <summary> 获取 HttpMethod
        /// </summary>
        public HttpMethod GetHttpMethod()
        {
            switch (Method)
            {
                case HttpRequestMethod.GET:
                    return HttpMethod.Get;
                case HttpRequestMethod.POST:
                    return HttpMethod.Post;
                case HttpRequestMethod.HEAD:
                    return HttpMethod.Head;
                case HttpRequestMethod.TRACE:
                    return HttpMethod.Trace;
                case HttpRequestMethod.PUT:
                    return HttpMethod.Put;
                case HttpRequestMethod.DELETE:
                    return HttpMethod.Delete;
                case HttpRequestMethod.OPTIONS:
                    return HttpMethod.Options;
                case HttpRequestMethod.CONNECT:
                    return HttpMethod_CONNECT;
                default:
                    return HttpMethod.Get;
            }
        }

        /// <summary> 获取 HttpMethod 枚举的字符串
        /// </summary>
        public string GetMethod()
        {
            switch (Method)
            {
                case HttpRequestMethod.GET:
                    return "GET";
                case HttpRequestMethod.POST:
                    return "POST";
                case HttpRequestMethod.HEAD:
                    return "HEAD";
                case HttpRequestMethod.TRACE:
                    return "TRACE";
                case HttpRequestMethod.PUT:
                    return "PUT";
                case HttpRequestMethod.DELETE:
                    return "DELETE";
                case HttpRequestMethod.OPTIONS:
                    return "OPTIONS";
                case HttpRequestMethod.CONNECT:
                    return "CONNECT";
                default:
                    return "GET";
            }
        }

        /// <summary> 用于简单的Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        [Export("HttpGet")]
        [ExportMetadata("Priority", 100)]
        public static Task<string> Get(string url, object data)
        {
            var request = new HttpRequest();
            request.Path = url;
            request.QueryString.Add(data);
            request.Method = HttpRequestMethod.GET;
            return request.GetString();
        }

        /// <summary> 用于简单的Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        [Export("HttpPost")]
        [ExportMetadata("Priority", 100)]
        public static Task<string> Post(string url, object data)
        {
            var request = new HttpRequest();
            request.Path = url;
            request.FormBody.Add(data);
            request.Method = HttpRequestMethod.POST;
            return request.GetString();
        }

        /// <summary> 发出请求前执行的任务
        /// </summary>
        internal Task PreRequest { get; set; }
        public bool AcceptHeader { get; set; }
        public bool AcceptCookie { get; set; }

        /// <summary>
        /// 返回当前请求的url
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetFullUrl().OriginalString;
        }

        /// <summary>
        /// 返回当前请求的url
        /// </summary>
        /// <param name="format">默认null 返回url不包含QueryString, F/f 表示返回完整路径,Q/q 返回QueryString </param>
        /// <returns></returns>
        public string ToString(string format)
        {
            if (format == null || (format = format.Trim()).Length != 1)
            {
                return GetFullUrl().OriginalString;
            }
            switch (format[0])
            {
                case 'f':
                case 'F':
                    return GetFullUrl().ToString();
                case 'Q':
                case 'q':
                    return GetFullUrl().Query;
                default:
                    break;
            }
            var url = GetFullUrl();
            return string.Concat(url.Scheme, "://", url.Authority, url.LocalPath);
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format);
        }
    }
}
