using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public class HttpHeaders : HttpParamsBase<string>
    {
        static readonly string DefaultUserAgent = GetDefaultUserAgent();

        private static string GetDefaultUserAgent()
        {
            var windows = Environment.OSVersion.ToString();
            if (Environment.Is64BitOperatingSystem)
            {
                windows += "; WOW64";
            }
            var nf = ".NET-Framework/" + Environment.Version.ToString();
            var name = typeof(HttpRequest).Assembly.GetName();
            return $"{nf} ({windows}) {name.Name}/{name.Version.ToString()} ({Environment.MachineName}; {Environment.UserName})";
        }

        internal HttpHeaders(IHttpParameterCollection @params)
            : base(@params)
        {
            Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            AcceptEncoding = "gzip, deflate, sdch";
            AcceptLanguage = "zh-CN,zh;q=0.8";
            CacheControl = "max-age=0";
            UserAgent = DefaultUserAgent;
            KeepAlive = true;
        }

        public override HttpParamLocation Location
        {
            get
            {
                return HttpParamLocation.Header;
            }
        }


        public string Accept
        {
            get { return base["Accept"]; }
            set { base["Accept"] = value; }
        }

        public string AcceptEncoding
        {
            get { return base["Accept-Encoding"]; }
            set { base["Accept-Encoding"] = value; }
        }

        public string AcceptLanguage
        {
            get { return base["Accept-Language"]; }
            set { base["Accept-Language"] = value; }
        }

        public string CacheControl
        {
            get { return base["Cache-Control"]; }
            set { base["Cache-Control"] = value; }
        }

        public string UserAgent
        {
            get { return base["User-Agent"]; }
            set { base["User-Agent"] = value; }
        }

        public bool KeepAlive
        {
            get { return "keep-alive".Equals(base["Connection"], StringComparison.OrdinalIgnoreCase); }
            set
            {
                if (value)
                {
                    base["Connection"] = "keep-alive";
                }
                else
                {
                    base["Connection"] = "close";
                }
            }
        }

    }
}
