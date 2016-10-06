using System;
using System.Collections;
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
        /// <summary>
        /// 静态缓存
        /// </summary>
        [ThreadStatic]
        private static HttpUrlEncodedBuilder _UrlEncodedBuilder;

        /// <summary>
        /// 回车换行
        /// </summary>
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
            var url = request.BaseUrl.Combine(request.Path);
            if (url == null)
            {
                throw new UriFormatException("url不能为空");
            }
            if (_UrlEncodedBuilder == null)
            {
                _UrlEncodedBuilder = new HttpUrlEncodedBuilder();
            }
            else
            {
                _UrlEncodedBuilder.Clear();
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
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            var parser = _provider.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            if (parser == null)
            {
                throw new FormatException($"无法获取{nameof(IHttpBodyParser)}");
            }
            Url = url.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.Unescaped);
            Headers = new List<KeyValuePair<string, string>>();
            request.OnParamsExtracting();
            List<KeyValuePair<string, object>> bodyparams;
            FindParams(request, out bodyparams);
            Body = parser.Serialize(null, bodyparams, _provider);
            request.OnParamsExtracted();
            var query = _UrlEncodedBuilder.ToString();

            if (query.Length == 0)
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
            if (request.Headers.AutoAddDefaultHeaders)
            {
                request.Headers.AddDefaultHeaders();

                if (request.Headers.Contains("Host") == false)
                {
                    AddHeader("Host", new[] { url.Host });
                }
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
        /// <param name="name"> </param>
        /// <param name="values"> </param>
        private void AddHeader(string name, IEnumerable<string> values)
        {
            Request?.OnHeaderFound(ref name, ref values);
            if (values == null)
            {
                return;
            }
            foreach (var value in values)
            {
                Headers.Add(new KeyValuePair<string, string>(name, value));
            }
        }

        /// <summary>
        /// 添加路由参数
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="value"> </param>
        private void AddPathParam(string name, object value)
        {
            var str = value as string ?? string.Join(",", value as IEnumerable ?? Type.EmptyTypes /*随便一个空数组都可以*/);
            Request?.OnPathParamFound(ref name, ref str);
            if (name != null)
            {
                Url = Url.Replace("{" + name + "}", str);
            }
        }

        /// <summary>
        /// 判断是否存在指定的头
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public bool HasHeader(string name) => Headers.Any(it => string.Equals(it.Key, name, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// 请求正文內容类型
        /// </summary>
        private readonly HttpContentType _provider;

        /// <summary>
        /// Cookie
        /// </summary>
        public CookieContainer Cookies { get; }

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
        /// <seealso cref="IHttpRequest" /> 对象
        /// </summary>
        public IHttpRequest Request { get; }

        /// <summary>
        /// 从请求体中获取字符串
        /// </summary>
        private string GetBodyString()
        {
            if ((Body == null) || (Body.Length == 0))
            {
                return null;
            }
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            var charset = _provider.GetFormat(typeof(Encoding)) as Encoding ?? Encoding.UTF8;
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
        /// 请求方案/版本 (Scheme.ToUpperInvariant()/Version;)
        /// </summary>
        public string SchemeVersion { get; }

        /// <summary>
        /// 遍历所有参数
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <param name="bodyParams"> body参数 </param>
        private void FindParams(IHttpRequest request, out List<KeyValuePair<string, object>> bodyParams)
        {
            bodyParams = new List<KeyValuePair<string, object>>();
            foreach (var param in request)
            {
                var name = param.Name;
                var value = param.Value;
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
                        if ((name != null) || (value != null))
                        {
                            _UrlEncodedBuilder.AppendObject(name, value);
                        }
                        break;
                    case HttpParamLocation.Body:
                        request.OnBodyParamFound(ref name, ref value);
                        if ((name != null) || (value != null))
                        {
                            bodyParams.Add(new KeyValuePair<string, object>(name, value));
                        }
                        break;
                    case HttpParamLocation.Path:
                        AddPathParam(name, value);
                        break;
                    case HttpParamLocation.Header:
                        AddHeader(name, param.Values.Cast<string>());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(param.Location));
                }
            }
        }

        /// <summary>
        /// 返回当前对象的url
        /// </summary>
        public override string ToString() => Url ?? "http://";

        /// <summary>
        /// 请求的原始数据
        /// </summary>
        public string Raw => $"{Method} {Url} {SchemeVersion}{CRLF}{string.Join(CRLF, Headers.Select(it => $"{it.Key}: {it.Value}"))}{CRLF}{CRLF}{GetBodyString()}";
    }
}