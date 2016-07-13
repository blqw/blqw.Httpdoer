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
    public struct HttpRequestData
    {
        [ThreadStatic]
        static HttpQueryBuilder _QueryBuilder;
        const string CRLF = "\r\n";

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
            string query = _QueryBuilder.ToString();

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
                AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            if (HasHeader("Accept-Encoding") == false)
                AddHeader("Accept-Encoding", "gzip, deflate, sdch");
            if (HasHeader("Accept-Language") == false)
                AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            if (HasHeader("Cache-Control") == false)
                AddHeader("Cache-Control", "max-age=0");
            if (HasHeader("User-Agent") == false)
                AddHeader("User-Agent", HttpHeaders.DefaultUserAgent);
            if (HasHeader("Connection") == false)
                AddHeader("Connection", "Keep-Alive");
            if (HasHeader("Host") == false)
                AddHeader("Host", url.Host);
        }

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

        public bool HasHeader(string name)
        {
            return Headers.Any(it => string.Equals(it.Key, name, StringComparison.OrdinalIgnoreCase));
        }

        private IFormatProvider _provider;

        public string Url { get; private set; }

        public Uri Host { get; private set; }

        public byte[] Body { get; private set; }

        public IHttpRequest Request { get; private set; }

        private string GetBodyString()
        {
            if (Body == null || Body.Length == 0) return null;
            var charset = _provider?.GetFormat(typeof(Encoding)) as Encoding ?? Encoding.UTF8;
            return charset?.GetString(Body);
        }

        public List<KeyValuePair<string, string>> Headers { get; private set; }
        public string Method { get; private set; }
        public Version Version { get; private set; }
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
                            goto case HttpParamLocation.Path;
                        else if (request.Method == HttpRequestMethod.Get)
                            goto case HttpParamLocation.Query;
                        else
                            goto case HttpParamLocation.Body;
                    case HttpParamLocation.Query:
                        Request?.OnQueryParamFound(ref name, ref value);
                        _QueryBuilder.AppendObject(name, value);
                        break;
                    case HttpParamLocation.Body:
                        request.OnBodyParamFound(ref name, ref value);
                        if (name != null)
                            @params.Add(name, value);
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


        public override string ToString()
        {
            return Url ?? "http://";
        }

        /// <summary>
        /// 请求的原始数据
        /// </summary>
        public string Raw
        {
            get
            {
                return $"{Method} {Url} {Version}{CRLF}{string.Join(CRLF, Headers.Select(it => $"{it.Key}: {it.Value}"))}{CRLF}{CRLF}{ GetBodyString()}";
            }
        }
    }
}
