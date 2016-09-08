using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 表示一个 HTTP 请求
    /// </summary>
    public class HttpRequest : IHttpRequest
    {
        public static IHttpLogger DefaultLogger = HttpDefaultLogger.Instance;

        /// <summary>
        /// 本地 Cookies 缓存
        /// </summary>
        public static CookieContainer LocalCookies { get; } = new CookieContainer();

        /// <summary>
        /// 初始化http请求
        /// </summary>
        public HttpRequest()
        {
            var @params = new HttpParameterCollection();
            Body = new HttpBody(@params);
            Headers = new HttpHeaders(@params);
            Query = new HttpStringParams(@params, HttpParamLocation.Query);
            PathParams = new HttpStringParams(@params, HttpParamLocation.Path);
            Params = new HttpParams(@params);
            _AllParams = @params;
            Timeout = new TimeSpan(0, 0, 15);
            if (DefaultLogger != null)
            {
                Loggers.Add(DefaultLogger);
            }
        }

        /// <summary>
        /// 初始化http请求,并设定基路径
        /// </summary>
        /// <param name="baseUrl">基路径</param>
        public HttpRequest(string baseUrl)
            : this()
        {
            if (baseUrl == null)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }
            if (baseUrl.Length <= 8 || baseUrl[0] == ':')
            {
                baseUrl = "http://" + baseUrl;
            }
            else if (baseUrl[3] != ':' && baseUrl[4] != ':' && baseUrl[5] != ':')
            {
                baseUrl = "http://" + baseUrl;
            }

            Uri uri;
            if (Uri.TryCreate(baseUrl, UriKind.Absolute, out uri) == false)
            {
                throw new UriFormatException(nameof(baseUrl) + " 不是一个有效的Url字符串");
            }
            BaseUrl = uri;
        }

        private IHttpParameterCollection _AllParams;

        public HttpBody Body { get; }
        public HttpHeaders Headers { get; }
        public HttpStringParams PathParams { get; }
        public HttpStringParams Query { get; }
        public HttpParams Params { get; }

        public Uri BaseUrl { get; set; }
        CookieContainer _Cookies;
        public CookieContainer Cookies
        {
            get
            {
                return _Cookies ?? (_Cookies = new CookieContainer());
            }
            set
            {
                _Cookies = value;
            }
        }
        public Encoding Encoding { get; set; }
        HttpRequestMethod _Method;
        public HttpRequestMethod Method
        {
            get { return _Method; }
            set
            {
                _Method = value;
                switch (_Method)
                {
                    case HttpRequestMethod.Custom:
                    case HttpRequestMethod.Get:
                        HttpMethod = "GET";
                        _Method = HttpRequestMethod.Get;
                        break;
                    case HttpRequestMethod.Post:
                        HttpMethod = "POST";
                        if (_AllParams.Contains("Content-Type", HttpParamLocation.Header) == false)
                        {
                            Body.ContentType = HttpContentType.Form;
                        }
                        break;
                    case HttpRequestMethod.Head:
                        HttpMethod = "Head";
                        break;
                    case HttpRequestMethod.Trace:
                        HttpMethod = "Trace";
                        break;
                    case HttpRequestMethod.Put:
                        HttpMethod = "PUT";
                        break;
                    case HttpRequestMethod.Delete:
                        HttpMethod = "DELETE";
                        break;
                    case HttpRequestMethod.Options:
                        HttpMethod = "OPTIONS";
                        break;
                    case HttpRequestMethod.Connect:
                        HttpMethod = "CONNECT";
                        break;
                    default:
                        break;
                }
            }
        }
        string _HttpMethod;
        public string HttpMethod
        {
            get
            {
                return _HttpMethod;
            }
            set
            {
                _HttpMethod = value?.ToUpperInvariant();
                switch (_HttpMethod)
                {
                    case "GET":
                        _Method = HttpRequestMethod.Get;
                        break;
                    case "POST":
                        _Method = HttpRequestMethod.Post;
                        if (_AllParams.Contains("Content-Type", HttpParamLocation.Header) == false)
                        {
                            Body.ContentType = HttpContentType.Form;
                        }
                        break;
                    case "HEAD":
                        _Method = HttpRequestMethod.Head;
                        break;
                    case "TRACE":
                        _Method = HttpRequestMethod.Trace;
                        break;
                    case "PUT":
                        _Method = HttpRequestMethod.Put;
                        break;
                    case "DELETE":
                        _Method = HttpRequestMethod.Delete;
                        break;
                    case "OPTIONS":
                        _Method = HttpRequestMethod.Options;
                        break;
                    case "CONNECT":
                        _Method = HttpRequestMethod.Connect;
                        break;
                    default:
                        _Method = HttpRequestMethod.Custom;
                        break;
                }
            }
        }
        public string Path { get; set; }
        public TimeSpan Timeout { get; set; }
        public Version Version { get; set; }


        static readonly char[] _TrimChars = new[] { '&' };

        public override string ToString()
        {
            return URIEx.GetFullURL(BaseUrl, Path)?.ToString() ?? "http://";
        }

        public IEnumerator<HttpParamValue> GetEnumerator()
        {
            return _AllParams.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IHttpResponse _Response;
        /// <summary>
        /// 最后一次响应
        /// </summary>
        public IHttpResponse Response
        {
            get
            {
                return _Response;
            }
            set
            {
                _Response = value;
                if (value != null)
                {
                    if (value.IsSuccessStatusCode == false)
                    {
                        this.Debug(((int)StatusCode).ToString());
                    }
                    if (value.Exception != null)
                    {
                        this.Error(value.Exception.Message);
                    }
                }
            }
        }


        /// <summary>
        /// 是否使用 Cookie
        /// </summary>
        [Obsolete("使用新属性CookieMode来设置,默认为 HttpCookieMode.CustomOrCache ")]
        public bool UseCookies
        {
            get { return CookieMode != HttpCookieMode.None; }
            set
            {
                CookieMode = value ? HttpCookieMode.CustomOrCache : HttpCookieMode.None;
            }
        }

        /// <summary>
        /// 缓存模式
        /// </summary>
        public HttpCookieMode CookieMode { get; set; }

        public Exception Exception
        {
            get
            {
                return Response?.Exception;
            }
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return Response?.StatusCode ?? 0;
            }
        }

        List<IHttpLogger> _Loggers;
        public List<IHttpLogger> Loggers
        {
            get
            {
                return _Loggers
                    ?? (_Loggers = new List<IHttpLogger>());
            }
        }

        List<IHttpTracking> _Trackings;

        public List<IHttpTracking> Trackings
        {
            get
            {
                return _Trackings
                    ?? (_Trackings = new List<IHttpTracking>());
            }
        }

        public Uri FullUrl
        {
            get
            {
                return URIEx.GetFullURL(BaseUrl, Path);
            }
        }

        protected void SetParam(string name, object value, HttpParamLocation location)
        {
            _AllParams.SetValue(name, value, location);
        }

    }
}
