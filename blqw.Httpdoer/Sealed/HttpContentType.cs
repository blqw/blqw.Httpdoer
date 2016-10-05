using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 表示请求或响应的正文内容类型
    /// </summary>
    public struct HttpContentType : IFormatProvider,IEquatable<HttpContentType>
    {
        static readonly Regex _ParseRegex = new Regex(@"^\s*(?<type>[^\\\s]+)\s*/\s*(?<format>[^;\s]+)\s*(;\s*charset\s*=\s*(?<charset>[^\s]*)\s*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        
        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        public static readonly HttpContentType Form = "application/x-www-form-urlencoded";
        /// <summary>
        /// application/json;charset=utf-8
        /// </summary>
        public static readonly HttpContentType Json = "application/json;charset=utf-8";
        /// <summary>
        /// application/octet-stream
        /// </summary>
        public static readonly HttpContentType OctetStream = "application/octet-stream";
        /// <summary>
        /// text/xml;charset=utf-8
        /// </summary>
        public static readonly HttpContentType XML = "text/xml;charset=utf-8";
        /// <summary>
        /// application/x-protobuf
        /// </summary>
        public static readonly HttpContentType Protobuf = "application/x-protobuf";
        /// <summary>
        /// text/plain;charset=utf-8
        /// </summary>
        public static readonly HttpContentType UTF8Text = "text/plain;charset=utf-8";
        /// <summary>
        /// text/plain
        /// </summary>
        public static readonly HttpContentType Text = "text/plain";
        /// <summary>
        /// null
        /// </summary>
        public static readonly HttpContentType Undefined = null;


        public static bool TryParse(string contentType, out HttpContentType result)
        {
            if (contentType == null)
            {
                result = default(HttpContentType);
                return false;
            }
            var m = _ParseRegex.Match(contentType);
            if (m.Success)
            {
                var g = m.Groups;
                var charset = g["charset"]?.Value;
                Encoding encoding = null;
                if (string.IsNullOrEmpty(charset) == false)
                {
                    encoding = Encoding.GetEncoding(charset);
                }

                result = new HttpContentType(g["type"].Value, g["format"].Value, encoding);
                return true;
            }
            else if (string.IsNullOrWhiteSpace(contentType))
            {
                result = Text;
                return true;
            }
            else
            {
                result = default(HttpContentType);
                return false;
            }
        }

        public static HttpContentType Parse(string contentType)
        {
            HttpContentType result;
            if (TryParse(contentType, out result))
            {
                return result;
            }
            return new HttpContentType(contentType, null, null);
        }

        #region 转换
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator string(HttpContentType value)
        {
            return value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator HttpContentType(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Undefined;
            }
            return Parse(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator HttpContentTypes(HttpContentType value)
        {
            if (value == Form)
            {
                return HttpContentTypes.Form;
            }
            else if (value == Json)
            {
                return HttpContentTypes.Json;
            }
            else if (value == OctetStream)
            {
                return HttpContentTypes.OctetStream;
            }
            else if (value == XML)
            {
                return HttpContentTypes.XML;
            }
            else if (value == Protobuf)
            {
                return HttpContentTypes.Protobuf;
            }
            else if (value == UTF8Text)
            {
                return HttpContentTypes.UTF8Text;
            }
            else if (value == Text)
            {
                return HttpContentTypes.Text;
            }
            else if (value == Undefined)
            {
                return HttpContentTypes.Undefined;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator HttpContentType(HttpContentTypes value)
        {
            switch (value)
            {
                case HttpContentTypes.Form:
                    return Form;
                case HttpContentTypes.Json:
                    return Json;
                case HttpContentTypes.OctetStream:
                    return OctetStream;
                case HttpContentTypes.XML:
                    return XML;
                case HttpContentTypes.Protobuf:
                    return Protobuf;
                case HttpContentTypes.UTF8Text:
                    return UTF8Text;
                case HttpContentTypes.Text:
                    return Text;
                case HttpContentTypes.Undefined:
                default:
                    return Undefined;
            }
        }

        #endregion

        public HttpContentType(string type, string format, Encoding charset)
        {
            Type = type;
            Format = format;
            Charset = charset;
            _Parser = null;
        }

        public HttpContentType(string type, string format, Encoding charset, IHttpBodyParser parser)
        {
            Type = type;
            Format = format;
            Charset = charset;
            _Parser = parser;
        }

        private readonly IHttpBodyParser _Parser;

        public string Type { get; }

        public string Format { get; }

        public Encoding Charset { get; }

        public override string ToString()
        {
            if (Charset == null)
            {
                if (Type == null || Format == null)
                {
                    return Type ?? Format;
                }
                return $"{Type}/{Format}";
            }
            return $"{Type}/{Format};charset={Charset.WebName}";
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(IHttpBodyParser)
                || formatType == typeof(ICustomFormatter))
            {
                return _Parser ?? HttpBodyParserFactory.Get(Type, Format);
            }
            if (formatType == typeof(Encoding))
            {
                return Charset;
            }
            if (formatType == typeof(string))
            {
                return $"{Type}/{Format}";
            }
            return null;
        }

        /// <summary>
        /// 指示此实例与指定对象是否相等。</summary>
        /// <returns>如果 <paramref name="obj" /> 和该实例具有相同的类型并表示相同的值，则为 true；否则为 false。</returns>
        /// <param name="obj">要与当前实例进行比较的对象。</param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj) => obj is HttpContentType && Equals((HttpContentType)obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(HttpContentType a, HttpContentType b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(HttpContentType a, HttpContentType b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// 返回此实例的哈希代码。</summary>
        /// <returns>一个 32 位有符号整数，它是该实例的哈希代码。</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() => ToString().GetHashCode();

        /// <summary>
        /// 改变当前对象的编码,产生一个新对象
        /// </summary>
        /// <param name="charset"></param>
        /// <returns></returns>
        public HttpContentType ChangeCharset(Encoding charset) => new HttpContentType(Type, Form, charset);

        /// <summary>
        /// 指示当前对象是否等于同一类型的另一个对象。</summary>
        /// <returns>如果当前对象等于 <paramref name="other" /> 参数，则为 true；否则为 false。</returns>
        /// <param name="other">与此对象进行比较的对象。</param>
        public bool Equals(HttpContentType other) => Equals(Charset, other.Charset)
                                                     && string.Equals(Format, other.Format, StringComparison.OrdinalIgnoreCase)
                                                     && string.Equals(Type, other.Type, StringComparison.OrdinalIgnoreCase)
                                                     && _Parser == other._Parser;
    }
}
