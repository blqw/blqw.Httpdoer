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
    public sealed class HttpRequest : IHttpRequest
    {
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
        CookieContainer _Cookie;
        public CookieContainer Cookie
        {
            get
            {
                return _Cookie ?? (_Cookie = new CookieContainer());
            }
            set
            {
                _Cookie = value;
            }
        }
        public Encoding Encoding { get; set; }
        public HttpMethod Method { get; set; }
        public string Path { get; set; }
        public TimeSpan Timeout { get; set; }
        public Version Version { get; set; }

        public Uri GetURL()
        {
            Uri baseUrl = BaseUrl;
            var path = Path;

            if (path == null && baseUrl == null)
            {
                return null;
            }

            Uri url;
            if (baseUrl == null)
            {
                if (Uri.TryCreate(path, UriKind.Absolute, out url) == false)
                {
                    throw new UriFormatException("UrlError");
                }
                return url;
            }

            if(Uri.TryCreate(baseUrl, path, out url))
            {
                return url;
            }

            throw new UriFormatException("UrlError");
        }

        static readonly char[] _TrimChars = new[] { '&' };

        public override string ToString()
        {
            return GetURL()?.ToString() ?? "http://";
        }

        public IEnumerator<HttpParamValue> GetEnumerator()
        {
            return _AllParams.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
