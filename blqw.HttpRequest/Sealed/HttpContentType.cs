using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 表示请求或响应的正文内容类型
    /// </summary>
    public struct HttpContentType : IFormatProvider
    {
        static readonly Regex _ParseRegex = new Regex(@"^\s*(?<type>[^\\\s]+)\s*/\s*(?<format>[^;\s]+)\s*(;\s*charset\s*=\s*(?<charset>[^\s]*)\s*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        public static bool TryParse(string contentType, Encoding defaultEncoding, out HttpContentType result)
        {
            if (defaultEncoding == null)
            {
                throw new ArgumentNullException(nameof(defaultEncoding));
            }

            var m = _ParseRegex.Match(contentType);
            if (m.Success)
            {
                var g = m.Groups;
                var charset = g["charset"]?.Value;
                if (charset != null)
                {
                    defaultEncoding = Encoding.GetEncoding(charset);
                }

                result = new HttpContentType(
                                g["type"].Value,
                                g["format"].Value,
                                defaultEncoding);
                return true;
            }
            else if (string.IsNullOrWhiteSpace(contentType))
            {
                result = new HttpContentType("unknown", "unknown", defaultEncoding);
                return true;
            }
            else
            {
                result = default(HttpContentType);
                return false;
                throw new FormatException($"{nameof(contentType)} 格式有误");
            }
        }

        public static HttpContentType Parse(string contentType, Encoding defaultEncoding)
        {
            HttpContentType result;
            if (TryParse(contentType, defaultEncoding, out result))
            {
                return result;
            }
            throw new FormatException($"{nameof(contentType)} 格式有误");
        }

        public static implicit operator string(HttpContentType value)
        {
            return value.ToString();
        }

        public static implicit operator HttpContentType(string value)
        {
            return Parse(value, Encoding.UTF8);
        }

        public HttpContentType(string type, string format, Encoding charset)
        {
            Type = type;
            Format = format;
            Charset = charset;
        }

        public string Type { get; }

        public string Format { get; }

        public Encoding Charset { get; }

        public override string ToString()
        {
            return $"{Type}/{Format};charset={Charset.WebName}";
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(IHttpBodyParser))
            {
                return HttpBodyParsers.Get(Type, Format);
            }
            else if (formatType == typeof(Encoding))
            {
                return Charset;
            }
            else if (formatType == typeof(string))
            {
                return $"{Type}/{Format}";
            }
            return null;
        }
    }
}
