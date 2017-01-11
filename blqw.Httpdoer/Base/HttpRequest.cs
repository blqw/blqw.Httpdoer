using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using blqw.IOC;

namespace blqw.Web
{
    /// <summary>
    /// 表示一个 HTTP 请求
    /// </summary>
    public class HttpRequest : IHttpRequest, IFormattable
    {
        /// <summary>
        /// 参数容器
        /// </summary>
        private readonly IHttpParameterContainer _paramContainer;

        /// <summary>
        /// 用于为 <see cref="Cookies" />属性提供数据
        /// </summary>
        private CookieContainer _cookies;

        /// <summary>
        /// 用于为 <see cref="HttpMethod" />属性提供数据
        /// </summary>
        private string _httpMethod;

        /// <summary>
        /// 用于为 <see cref="Method" />属性提供数据
        /// </summary>
        private HttpRequestMethod _method;


        /// <summary>
        /// 用于为 <see cref="Response" />属性提供数据
        /// </summary>
        private IHttpResponse _response;


        /// <summary>
        /// 用于为 <see cref="Trackings" />属性提供数据
        /// </summary>
        private List<IHttpTracking> _trackings;

        /// <summary>
        /// 初始化 HTTP 请求
        /// </summary>
        public HttpRequest()
        {
            var @params = new HttpParameterContainer();
            Body = new HttpBody(@params);
            Headers = new HttpHeaders(@params);
            Query = new HttpStringParams(@params, HttpParamLocation.Query);
            PathParams = new HttpStringParams(@params, HttpParamLocation.Path);
            Params = new HttpParams(@params);
            _paramContainer = @params;
            Timeout = new TimeSpan(0, 0, 15);
            Logger = new IOC.LoggerSource(Httpdoer.DefaultLogger.Name, Httpdoer.DefaultLogger.Switch.Level);
            Logger.Listeners.AddRange(Httpdoer.DefaultLogger.Listeners);
            CookieMode = HttpCookieMode.ApplicationCache;
            Proxy = WebRequest.DefaultWebProxy;
        }

        /// <summary>
        /// 初始化 HTTP 请求,并设定基路径
        /// </summary>
        /// <param name="baseUrl"> 基路径 </param>
        public HttpRequest(string baseUrl)
            : this()
        {
            if (baseUrl == null)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }
            if ((baseUrl.Length <= 8) || (baseUrl[0] == ':'))
            {
                baseUrl = "http://" + baseUrl;
            }
            else if ((baseUrl[3] != ':') && (baseUrl[4] != ':') && (baseUrl[5] != ':'))
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

        /// <summary>
        /// 本地 Cookies 缓存
        /// </summary>
        public static CookieContainer LocalCookies { get; private set; } = new CookieContainer();

        /// <summary>
        /// 请求操作中最后一次异常
        /// </summary>
        public Exception Exception => Response?.Exception;

        /// <summary>
        /// HTTP 请求正文
        /// </summary>
        public HttpBody Body { get; }

        /// <summary>
        /// HTTP 头信息
        /// </summary>
        public HttpHeaders Headers { get; }

        /// <summary>
        /// HTTP 请求路径参数
        /// </summary>
        public HttpStringParams PathParams { get; }

        /// <summary>
        /// HTTP 请求查询参数
        /// </summary>
        public HttpStringParams Query { get; }

        /// <summary>
        /// 代理设置
        /// </summary>
        public IWebProxy Proxy
        {
            get { return _proxy; }
            set
            {
                if (_proxy != value)
                {
                    _proxy = value;
                    if (_client != null && !HttpClientProvider.IsCached(_client)) //如果没有被缓存则释放
                    {
                        _client.Dispose();
                    }
                    _client = null;
                }
            }
        }

        /// <summary>
        /// HTTP 参数,根据 Method 和 Path 来确定参数位置
        /// </summary>
        public HttpParams Params { get; }

        /// <summary>
        /// 基路径
        /// </summary>
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        public CookieContainer Cookies
        {
            get
            {
                switch (CookieMode)
                {
                    case HttpCookieMode.None:
                        return null;
                    case HttpCookieMode.ApplicationCache:
                        return LocalCookies;
                    case HttpCookieMode.UserCustom:
                    case HttpCookieMode.CustomOrCache:
                        return _cookies ?? (_cookies = new CookieContainer());
                    default:
                        throw new ArgumentOutOfRangeException(nameof(CookieMode));
                }
            }
            set
            {
                switch (CookieMode)
                {
                    case HttpCookieMode.None:
                    case HttpCookieMode.ApplicationCache:
                        throw new NotSupportedException($"无法设置Cookie,请先设置{nameof(CookieMode)}为{HttpCookieMode.UserCustom}或{HttpCookieMode.CustomOrCache}");
                    case HttpCookieMode.UserCustom:
                    case HttpCookieMode.CustomOrCache:
                        _cookies = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 请求方式
        /// </summary>
        public HttpRequestMethod Method
        {
            get
            {
                if (_method == HttpRequestMethod.Delete || _method == HttpRequestMethod.Get)
                {
                    if (Body.HasAny())
                    {
                        return HttpRequestMethod.Post;
                    }
                }
                return _method;
            }
            set
            {
                _method = value;
                switch (_method)
                {
                    case HttpRequestMethod.Custom:
                        break;
                    case HttpRequestMethod.Get:
                        _method = HttpRequestMethod.Get;
                        Body.ContentType = HttpContentType.Undefined;
                        break;
                    case HttpRequestMethod.Post:
                        if (Body.ContentType.IsUndefined)
                        {
                            Body.ContentType = HttpContentType.Form;
                        }
                        break;
                    case HttpRequestMethod.Head:
                        break;
                    case HttpRequestMethod.Trace:
                        break;
                    case HttpRequestMethod.Put:
                        if (Body.ContentType.IsUndefined)
                        {
                            Body.ContentType = HttpContentType.Form;
                        }
                        break;
                    case HttpRequestMethod.Delete:
                        Body.ContentType = HttpContentType.Undefined;
                        break;
                    case HttpRequestMethod.Options:
                        break;
                    case HttpRequestMethod.Connect:
                        break;
                    case HttpRequestMethod.Patch:
                        if (Body.ContentType.IsUndefined)
                        {
                            Body.ContentType = HttpContentType.Form;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 请求方式的字符串形式
        /// </summary>
        public string HttpMethod
        {
            get
            {
                if (_httpMethod != null)
                {
                    return _httpMethod;
                }
                switch (Method)
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
                    case HttpRequestMethod.Patch:
                        return "PATCH";
                    case HttpRequestMethod.Custom:
                        return "CUSTOM";
                    default:
                        return Method.ToString().ToUpperInvariant();
                }
            }
            set
            {
                _httpMethod = value?.ToUpperInvariant();
                switch (_httpMethod)
                {
                    case "GET":
                        Method = HttpRequestMethod.Get;
                        break;
                    case "POST":
                        Method = HttpRequestMethod.Post;
                        break;
                    case "HEAD":
                        Method = HttpRequestMethod.Head;
                        break;
                    case "TRACE":
                        Method = HttpRequestMethod.Trace;
                        break;
                    case "PUT":
                        Method = HttpRequestMethod.Put;
                        break;
                    case "DELETE":
                        Method = HttpRequestMethod.Delete;
                        break;
                    case "OPTIONS":
                        Method = HttpRequestMethod.Options;
                        break;
                    case "CONNECT":
                        Method = HttpRequestMethod.Connect;
                        break;
                    case "PATCH":
                        Method = HttpRequestMethod.Patch;
                        break;
                    default:
                        Method = HttpRequestMethod.Custom;
                        break;
                }
            }
        }

        /// <summary>
        /// 基路径的相对路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 消息版本。默认值为 1.1。
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// 枚举所有请求参数
        /// </summary>
        public IEnumerator<HttpParamValue> GetEnumerator() => _paramContainer.GetEnumerator();

        private HttpClient _client;
        private IWebProxy _proxy;

        /// <summary>
        /// 获取一个用于异步请求的
        /// </summary>
        HttpMessageInvoker IHttpRequest.GetAsyncInvoker() => _client ?? (_client = HttpClientProvider.GetClient(Proxy));

        /// <summary>
        /// 最后一次响应
        /// </summary>
        public IHttpResponse Response
        {
            get { return _response; }
            set
            {
                _response = value;
                if (value?.IsSuccessStatusCode == false)
                {
                    Logger?.Write(TraceEventType.Warning, $"状态码:{(int)_response.StatusCode}");
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
            set { CookieMode = value ? HttpCookieMode.CustomOrCache : HttpCookieMode.None; }
        }

        /// <summary>
        /// 缓存模式
        /// </summary>
        public HttpCookieMode CookieMode { get; set; }

        /// <summary>
        /// 自动302跳转
        /// </summary>
        public bool AutoRedirect { get; set; } = true;


        /// <summary>
        /// 获取或设置日志记录器
        /// </summary>
        public TraceSource Logger { get; set; }

        /// <summary>
        /// 获取用于触发一系列事件的跟踪对象
        /// </summary>
        public List<IHttpTracking> Trackings => _trackings ?? (_trackings = new List<IHttpTracking>());

        /// <summary>
        /// 完整路径
        /// </summary>
        public Uri FullUrl => BaseUrl.Combine(Path);

        /// <summary>
        /// 清理本地Cooker缓存
        /// </summary>
        public static void ClearLocalCookies() => LocalCookies = new CookieContainer();


        /// <summary>
        /// 返回当前请求的完成路径
        /// </summary>
        /// <returns> </returns>
        public override string ToString() => FullUrl?.ToString() ?? "http://";

        /// <summary>
        /// 使用指定的格式格式化当前实例的值。
        /// </summary>
        /// <param name="format">"q" 包含query参数</param>
        /// <param name="formatProvider">要用于设置值格式的提供程序。- 或 -null 引用（Visual Basic 中为 Nothing）将从操作系统的当前区域设置中获取数字格式信息。</param>
        public string ToString(string format, IFormatProvider formatProvider = null)
            => format?.ToLowerInvariant() == "q" ? new HttpRequestData(this).Url : FullUrl?.ToString() ?? "http://";


        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="name"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        /// <param name="location"> 参数位置 </param>
        protected void SetParam(string name, object value, HttpParamLocation location)
            => _paramContainer.SetValue(location, name, value);
    }
}