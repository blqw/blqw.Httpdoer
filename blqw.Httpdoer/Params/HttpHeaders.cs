using System;

namespace blqw.Web
{
    /// <summary>
    /// 表示 HTTP 请求或响应的头信息
    /// </summary>
    public class HttpHeaders : HttpParamsBase<string>
    {
        /// <summary>
        /// 默认 User-Agent 信息
        /// </summary>
        internal static readonly string DefaultUserAgent = ((Func<string>) delegate
        {
            var windows = Environment.OSVersion.ToString();
            if (Environment.Is64BitOperatingSystem)
            {
                windows += "; WOW64";
            }
            var nf = ".NET-Framework/" + Environment.Version;
            var name = typeof(HttpRequest).Assembly.GetName();
            return $"{nf} ({windows}) {name.Name}/{name.Version} ({Environment.MachineName}; {Environment.UserName})";
        })();
        
        /// <summary>
        /// 初始化响应头
        /// </summary>
        internal HttpHeaders()
            : base(new HttpParameterContainer(), HttpParamLocation.Header)
        {
        }

        /// <summary>
        /// 初始化请求头
        /// </summary>
        /// <param name="params">请求参数</param>
        internal HttpHeaders(IHttpParameterContainer @params)
            : base(@params, HttpParamLocation.Header)
        {
        }

        /// <summary>
        /// 插入标准头
        /// </summary>
        public void AddDefaultHeaders()
        {
            if (Contains("Accept") == false)
            {
                Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            }
            if (Contains("Accept-Encoding") == false)
            {
                Add("Accept-Encoding", "gzip, deflate, sdch");
            }
            if (Contains("Accept-Language") == false)
            {
                Add("Accept-Language", "zh-CN,zh;q=0.8");
            }
            if (Contains("Cache-Control") == false)
            {
                Add("Cache-Control", "max-age=0");
            }
            if (Contains("User-Agent") == false)
            {
                Add("User-Agent", DefaultUserAgent);
            }
            if (Contains("Connection") == false)
            {
                Add("Connection", "Keep-Alive");
            }
        }

        /// <summary>
        /// 是否插入默认头
        /// </summary>
        public bool AutoAddDefaultHeaders { get; set; } = true;

        /// <summary>
        /// 请求或响应头 Accept 的值
        /// </summary>
        public string Accept
        {
            get { return base["Accept"]; }
            set { base["Accept"] = value; }
        }

        /// <summary>
        /// 请求或响应头 Accept-Encoding 的值
        /// </summary>
        public string AcceptEncoding
        {
            get { return base["Accept-Encoding"]; }
            set { base["Accept-Encoding"] = value; }
        }

        /// <summary>
        /// 请求或响应头 Accept-Language 的值
        /// </summary>
        public string AcceptLanguage
        {
            get { return base["Accept-Language"]; }
            set { base["Accept-Language"] = value; }
        }

        /// <summary>
        /// 请求或响应头 Cache-Control 的值
        /// </summary>
        public string CacheControl
        {
            get { return base["Cache-Control"]; }
            set { base["Cache-Control"] = value; }
        }

        /// <summary>
        /// 请求或响应头 User-Agent 的值
        /// </summary>
        public string UserAgent
        {
            get { return base["User-Agent"]; }
            set { base["User-Agent"] = value; }
        }

        /// <summary>
        /// 根据请求或响应头 Connection 判断当前请求或响应是否是长连接
        /// </summary>
        public bool KeepAlive
        {
            get { return "keep-alive".Equals(base["Connection"], StringComparison.OrdinalIgnoreCase); }
            set
            {
                if (value)
                {
                    base["Connection"] = "Keep-Alive";
                }
                else
                {
                    base["Connection"] = "Close";
                }
            }
        }
        
    }
}