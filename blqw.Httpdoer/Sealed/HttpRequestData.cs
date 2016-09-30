using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace blqw.Web
{
    /// <summary>
    /// 用于存储Http请求的各种信息
    /// </summary>
    public struct HttpRequestData
    {
        [ThreadStatic]
        private static HttpQueryBuilder _QueryBuilder;

        private const string CRLF = "\r\n";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="request"> </param>
        public HttpRequestData(IHttpRequest request)
            : this()
        {
            Request = request;
            Method = request.HttpMethod;
            var url = URIEx.GetFullURL(request.BaseUrl, request.Path);
            if (url == null)
            {
                throw new UriFormatException("url不能为空");
            }
            if (_QueryBuilder == null)
            {
                _QueryBuilder = new HttpQueryBuilder();
            }
            else
            {
                _QueryBuilder.Clear();
            }
            Host = new Uri(url, "/");

            if (request.Version != null)
            {
                Version = request.Version;
            }
            else if (Host.Scheme == Uri.UriSchemeHttps)
            {
                Version = HttpVersion.Version10;
            }
            else
            {
                Version = HttpVersion.Version11;
            }

            SchemeVersion = $"{url.Scheme.ToUpperInvariant()}/{Version}";

            _provider = request.Body.ContentType;
            var parser = _provider.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            if (parser == null)
            {
                throw new FormatException($"无法获取{nameof(IHttpBodyParser)}");
            }
            Url = url.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.Unescaped);
            Headers = new List<KeyValuePair<string, string>>();
            request.OnParamsExtracting();
            Body = parser.Serialize(null, GetBodyParams(request), _provider);
            request.OnParamsExtracted();
            var query = _QueryBuilder.ToString();

            if ((query?.Length).GetValueOrDefault(0) == 0)
            {
                Url += url.Fragment;
            }
            else
            {
                if (url.Query.Length == 0)
                {
                    Url += "?" + query + url.Fragment;
                }
                else
                {
                    Url += url.Query + "&" + query + url.Fragment;
                }
            }
            //插入默认头
            if (HasHeader("Accept") == false)
            {
                AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            }
            if (HasHeader("Accept-Encoding") == false)
            {
                AddHeader("Accept-Encoding", "gzip, deflate, sdch");
            }
            if (HasHeader("Accept-Language") == false)
            {
                AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            }
            if (HasHeader("Cache-Control") == false)
            {
                AddHeader("Cache-Control", "max-age=0");
            }
            if (HasHeader("User-Agent") == false)
            {
                AddHeader("User-Agent", HttpHeaders.DefaultUserAgent);
            }
            if (HasHeader("Connection") == false)
            {
                AddHeader("Connection", "Keep-Alive");
            }
            if (HasHeader("Host") == false)
            {
                AddHeader("Host", url.Host);
            }
            if (Method == null)
            {
                Method = Body?.Length > 0 ? "POST" : "GET";
            }

            if (request.CookieMode == HttpCookieMode.None)
            {
                return;
            }
            if (request.CookieMode.HasFlag(HttpCookieMode.CustomOrCache))
            {
                Cookies = new CookieContainer();

                var cookies = request.Cookies?.GetCookies(Host);

                if (cookies == null)
                {
                    Cookies.Add(HttpRequest.LocalCookies.GetCookies(Host));
                }
                else
                {
                    Cookies.Add(cookies);
                    foreach (Cookie c in HttpRequest.LocalCookies.GetCookies(Host))
                    {
                        if (cookies[c.Name] == null)
                        {
                            Cookies.Add(c);
                        }
                    }
                }
            }
            else if (request.CookieMode.HasFlag(HttpCookieMode.ApplicationCache))
            {
                Cookies = HttpRequest.LocalCookies;
            }
            else if (request.CookieMode.HasFlag(HttpCookieMode.UserCustom))
            {
                Cookies = request.Cookies;
            }

            var cookie = Cookies.GetCookieHeader(Host);
            if (string.IsNullOrWhiteSpace(cookie) == false)
            {
                Headers.Add(new KeyValuePair<string, string>("Cookie", cookie));
            }
        }
        

        /// <summary>
        /// 添加头
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="values"></param>
        private void AddHeader(string name, string value = null, IEnumerable<string> values = null)
        {
            if (values != null)
            {
                foreach (var val in values)
                {
                    Request?.OnHeaderFound(ref name, ref value);
                    Headers.Add(new KeyValuePair<string, string>(name, val));
                }
            }
            else if (value != null)
            {
                Request?.OnHeaderFound(ref name, ref value);
                Headers.Add(new KeyValuePair<string, string>(name, value));
            }
        }

        /// <summary>
        /// 添加路由参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="values"></param>
        private void AddPathParam(string name, string value, IEnumerable<string> values)
        {
            if (values != null)
            {
                Request?.OnPathParamFound(ref name, ref value);
                Url = Url.Replace("{" + name + "}", string.Join(",", values));
            }
            else if (value != null)
            {
                Request?.OnPathParamFound(ref name, ref value);
                Url = Url.Replace("{" + name + "}", value);
            }
        }

        /// <summary>
        /// 判断是否存在指定的头
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasHeader(string name) => Headers.Any(it => string.Equals(it.Key, name, StringComparison.OrdinalIgnoreCase));

        private readonly IFormatProvider _provider;

        public CookieContainer Cookies { get; private set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 请求主机域名
        /// </summary>
        public Uri Host { get; }

        /// <summary>
        /// 请求体字节码
        /// </summary>
        public byte[] Body { get; }

        /// <summary>
        /// <seealso cref="IHttpRequest"/> 对象
        /// </summary>
        public IHttpRequest Request { get; }

        /// <summary>
        /// 从请求体中获取字符串
        /// </summary>
        /// <returns></returns>
        private string GetBodyString()
        {
            if ((Body == null) || (Body.Length == 0))
            {
                return null;
            }
            var charset = _provider?.GetFormat(typeof(Encoding)) as Encoding ?? Encoding.UTF8;
            return charset.GetString(Body);
        }
        /// <summary>
        /// 请求头
        /// </summary>
        public List<KeyValuePair<string, string>> Headers { get; }
        /// <summary>
        /// 请求方法
        /// </summary>
        public string Method { get; }
        /// <summary>
        /// 请求版本
        /// </summary>
        public Version Version { get; }
        /// <summary>
        /// 请求方案/版本 ({Scheme.ToUpperInvariant()}/{SchemeVersion})
        /// </summary>
        public string SchemeVersion { get; private set; }

        private Dictionary<string, object> GetBodyParams(IHttpRequest request)
        {
            var @params = new Dictionary<string, object>();
            foreach (var param in request)
            {
                var name = param.Name;
                var value = param.Values ?? param.Value;
                switch (param.Location)
                {
                    case HttpParamLocation.Auto:
                        if (Url.Contains("{" + name + "}"))
                        {
                            goto case HttpParamLocation.Path;
                        }
                        if (request.Method == HttpRequestMethod.Get)
                        {
                            goto case HttpParamLocation.Query;
                        }
                        goto case HttpParamLocation.Body;
                    case HttpParamLocation.Query:
                        Request?.OnQueryParamFound(ref name, ref value);
                        _QueryBuilder.AppendObject(name, value);
                        break;
                    case HttpParamLocation.Body:
                        request.OnBodyParamFound(ref name, ref value);
                        if (name != null)
                        {
                            @params.Add(name, value);
                        }
                        break;
                    case HttpParamLocation.Path:
                        AddPathParam(name, value as string, param.Values?.Cast<string>());
                        break;
                    case HttpParamLocation.Header:
                        AddHeader(name, value as string, param.Values?.Cast<string>());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(param.Location));
                }
            }
            return @params;
        }

        /// <summary>
        /// 返回当前对象的url
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Url ?? "http://";

        /// <summary>
        /// 请求的原始数据
        /// </summary>
        public string Raw => $"{Method} {Url} {SchemeVersion}{CRLF}{string.Join(CRLF, Headers.Select(it => $"{it.Key}: {it.Value}"))}{CRLF}{CRLF}{GetBodyString()}";
    }
}