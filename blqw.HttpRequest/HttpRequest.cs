using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
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
            //用于https的请求验证票据
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            //并发请求数
            ServicePointManager.DefaultConnectionLimit = 1024;
            try
            {
                //用于Get提交正文的请求
                var t = typeof(HttpWebRequest).GetField("_Verb", BindingFlags.Instance | BindingFlags.NonPublic).FieldType;
                var list = (IDictionary)t.GetField("NamedHeaders", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                list["Get+"] = t.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)[0].Invoke(new object[] { "GET", true, false, false, false });
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
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


        HttpHeaders _Headers;
        HttpQueryString _QueryString;
        HttpFormBody _FormBody;
        CookieContainer _Cookie;

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
        /// <remarks>周子鉴 2016.02.01</remarks>
        public async Task<byte[]> GetBytes()
        {
            using (var response = await GetResponse())
            {
                if (response == null)
                {
                    return new byte[0];
                }
                var timer = Stopwatch.StartNew();
                var bytes = GetBytes(response);
                timer.Stop();
                Trace.WriteLine($"timing: {timer.ElapsedMilliseconds}; length:{bytes.Length}", "HttpRequest.ReadBytes");
                return bytes;
            }
        }

        /// <summary> 获取响应的字节流
        /// </summary>
        /// <param name="response">响应体</param>
        /// <returns></returns>
        /// <remarks>周子鉴 2016.02.01</remarks>
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
        /// 获取相应对象
        /// </summary>
        /// <returns></returns>
        public async Task<HttpWebResponse> GetResponse()
        {
            var timer = Stopwatch.StartNew();
            string[] ms = new string[4];
            if (Encoding == null)
            {
                Encoding = Encoding.UTF8;
            }
            if (PreRequest != null)
            {
                await PreRequest.ConfigureAwait(false);
                PreRequest = null;
                ms[0] = "pre:" + timer.ElapsedMilliseconds;
                timer.Restart();
            }
            ResponseCode = 0;
            Exception = null;
            Uri uri = GetFullUrl();

            Trace.WriteLine(uri.ToString(), "HttpRequest.Url");

            var www = WebRequest.CreateHttp(uri);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                www.ProtocolVersion = HttpVersion.Version10;
            }
            www.CookieContainer = Cookie;
            www.Timeout = (int)Timeout.TotalMilliseconds;
            Headers.SetHeaders(www);
            www.Method = GetMethod();
            www.KeepAlive = false;

            try
            {
                if (_FormBody != null)
                {
                    FormBody.SetHeaders(www, Encoding);
                    var formdata = FormBody.GetBytes(Encoding);
                    www.ContentLength = formdata.Length;
                    if (formdata.Length > 0)
                    {
                        using (var req = await www.GetRequestStreamAsync())
                        {
                            await req.WriteAsync(formdata, 0, formdata.Length).ConfigureAwait(false);
                        }
                    }
                }
                else
                {
                    www.ContentLength = 0;
                }

                ms[1] = "set data:" + timer.ElapsedMilliseconds;
                timer.Restart();
                var res = (HttpWebResponse)await www.GetResponseAsync();
                ms[2] = "response:" + timer.ElapsedMilliseconds;
                timer.Restart();
                www.CookieContainer = new CookieContainer();
                www.CookieContainer.Add(res.Cookies);
                Headers.Clear();
                Headers.Add(res.Headers);
                ResponseCode = res.StatusCode;
                return res;

            }
            catch (WebException ex)
            {
                Exception = ex;
                Trace.WriteLine(ex.Message, "HttpRequest.Error");
                var res = ex.Response as HttpWebResponse;
                ms[2] = "error:" + timer.ElapsedMilliseconds;
                timer.Restart();
                if (res != null)
                {
                    www.CookieContainer = new CookieContainer();
                    www.CookieContainer.Add(res.Cookies);
                    Headers.Clear();
                    Headers.Add(res.Headers);
                    ResponseCode = res.StatusCode;
                    return res;
                }
                else
                {
                    ResponseCode = (HttpStatusCode)ex.Status;
                    return null;
                }
                throw;
            }
            finally
            {
                ms[3] = "end:" + timer.ElapsedMilliseconds;
                Trace.WriteLine(ResponseCode, "HttpRequest.StatusCode");
                Trace.WriteLine(string.Join("; ", ms), "HttpRequest.Timing");
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

        /// <summary> 获取 HttpMethod 枚举的字符串
        /// </summary>
        public string GetMethod()
        {
            switch (Method)
            {
                case HttpRequestMethod.GET:
                    return IsInitialized ? "GET+" : "GET";
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
