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
        static StringBuilder _Buffer;

        public HttpRequestData(IHttpRequest request)
            : this()
        {
            if (_Buffer == null)
            {
                _Buffer = new StringBuilder();
            }
            else
            {
                _Buffer.Clear();
            }

            var provider = request.Body.ContentType;
            var parser = provider.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            if (parser == null)
            {
                throw new FormatException($"无法获取{nameof(IHttpBodyParser)}");
            }

            _Path = request.ToString();
            Body = parser.Format(null, GetBodyParams(request), provider);
            var query = _Buffer.ToString();

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
                    case HttpParamLocation.Query:
                        AppendObject(_Buffer, param.Name, param.Value);
                        break;
                    case HttpParamLocation.Body:
                        yield return new KeyValuePair<string, object>(param.Name, param.Value);
                        break;
                    case HttpParamLocation.Path:
                        _Path.Replace("{" + param.Name + "}", param.Value?.ToString());
                        break;
                    case HttpParamLocation.Auto:
                        if (request.Method == HttpMethod.Get)
                        {
                            goto case HttpParamLocation.Query;
                        }
                        else if (_Path.Contains("{" + param.Name + "}"))
                        {
                            goto case HttpParamLocation.Path;
                        }
                        else
                        {
                            goto case HttpParamLocation.Body;
                        }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(param.Location));
                }

            }
            throw new NotImplementedException();
        }

        private static void AppendObject(StringBuilder buffer, string preName, object obj)
        {
            var str = obj as string
                ?? (obj as IFormattable)?.ToString(null, null)
                ?? (obj as IConvertible)?.ToString(null);
            if (str != null || obj == null)
            {
                if (preName != null)
                {
                    AppendEscape(buffer, preName);
                    buffer.Append('=');
                }
                AppendEscape(buffer, str);
                buffer.Append('&');
                return;
            }

            var props = obj.GetType().GetProperties();
            if (props.Length == 0)
            {
                if (preName != null)
                {
                    AppendEscape(buffer, preName);
                    buffer.Append('=');
                }
                AppendEscape(buffer, obj.ToString());
                buffer.Append('&');
                return;
            }

            foreach (var p in props)
            {
                AppendObject(buffer, ConcatName(preName, p.Name), p.GetValue(obj));
            }
        }



        private static void AppendEscape(StringBuilder buffer, string str)
        {
            const int max = 32766;
            if (str == null)
            {
                return;
            }
            var length = str.Length;
            if (length < max)
            {
                buffer.Append(Uri.EscapeDataString(str));
                return;
            }
            int i = 0;
            length -= max;
            for (; i < length; i += max)
            {
                var s = str.Substring(i, max);
                buffer.Append(Uri.EscapeDataString(s));
            }
            buffer.Append(Uri.EscapeDataString(str.Substring(i, length - i + max)));
        }


        /// <summary> 
        /// 连接参数名,如果存在前缀的话 组成 `前缀.参数名` 的格式
        /// </summary>
        /// <param name="pre">参数名前缀</param>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        /// <remarks>周子鉴 2015.08.01</remarks>
        private static string ConcatName(string pre, string name)
        {
            return pre == null ? name : pre + "." + name;
        }


    }
}
