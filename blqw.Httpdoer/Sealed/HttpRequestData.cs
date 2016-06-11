using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public struct HttpRequestData
    {
        [ThreadStatic]
        static HttpQueryBuilder _QueryBuilder;

        public HttpRequestData(IHttpRequest request)
            : this()
        {
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

            var provider = request.Body.ContentType;
            var parser = provider.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            if (parser == null)
            {
                throw new FormatException($"无法获取{nameof(IHttpBodyParser)}");
            }
            _Path = url.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.Unescaped);

            Body = parser.Serialize(null, GetBodyParams(request), provider);
            string query = _QueryBuilder.ToString();

            if ((query?.Length).GetValueOrDefault(0) == 0)
            {
                Url = url;
            }
            else
            {
                if (url.Query.Length == 0)
                {
                    query = "?" + query + url.Fragment;
                }
                else
                {
                    query = url.Query + "&" + query + url.Fragment;
                }
                if (Uri.TryCreate(url, query, out url) == false)
                {
                    throw new UriFormatException();
                }
                Url = url;
            }

        }

        private string _Path;

        public Uri Url { get; private set; }

        public byte[] Body { get; private set; }

        private IEnumerable<KeyValuePair<string, object>> GetBodyParams(IHttpRequest request)
        {
            foreach (var param in request)
            {
                switch (param.Location)
                {
                    case HttpParamLocation.Auto:
                        if (_Path.Contains("{" + param.Name + "}"))
                            goto case HttpParamLocation.Path;
                        else if (request.Method == HttpRequestMethod.Get)
                            goto case HttpParamLocation.Query;
                        else
                            goto case HttpParamLocation.Body;
                    case HttpParamLocation.Query:
                        _QueryBuilder.AppendObject(param.Name, param.Value);
                        break;
                    case HttpParamLocation.Body:
                        yield return new KeyValuePair<string, object>(param.Name, param.Value);
                        break;
                    case HttpParamLocation.Path:
                        _Path.Replace("{" + param.Name + "}", param.Value?.ToString());
                        break;
                    case HttpParamLocation.Header:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(param.Location));
                }
            }
        }

        public override string ToString()
        {
            return Url?.ToString() ?? "http://";
        }
    }
}
