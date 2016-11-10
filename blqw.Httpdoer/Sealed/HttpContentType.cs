using System;
using System.Text;
using System.Text.RegularExpressions;

namespace blqw.Web
{
    /// <summary>
    /// 表示请求或响应的正文内容类型
    /// </summary>
    public struct HttpContentType : IFormatProvider, IEquatable<HttpContentType>
    {
        /// <summary>
        /// 用于解析字符串的正则表达式
        /// </summary>
        private static readonly Regex _ParseRegex = new Regex(@"^\s*(?<type>[^\\\s]+)\s*/\s*(?<format>[^;\s]+)\s*(;\s*charset\s*=\s*(?<charset>[^\s]*)\s*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

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

        /// <summary>
        /// 将<seealso cref="HttpContentType" />对象的等效字符串转换为具体对象
        /// </summary>
        /// <param name="contentType"> 待解析的字符串 </param>
        /// <returns> </returns>
        public static HttpContentType Parse(string contentType)
        {
            if (contentType == null)
            {
                return default(HttpContentType);
            }
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return Text;
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

                return new HttpContentType(g["type"].Value, g["format"].Value, encoding);
            }
            return new HttpContentType(contentType, null, null);
        }

        #region 转换

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"> </param>
        /// <returns> </returns>
        public static implicit operator string(HttpContentType value) => value.ToString();

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"> </param>
        public static implicit operator HttpContentType(string value) => Parse(value);

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"> </param>
        public static implicit operator HttpContentTypes(HttpContentType value)
        {
            if (value == Form)
            {
                return HttpContentTypes.Form;
            }
            if (value == Json)
            {
                return HttpContentTypes.Json;
            }
            if (value == OctetStream)
            {
                return HttpContentTypes.OctetStream;
            }
            if (value == XML)
            {
                return HttpContentTypes.XML;
            }
            if (value == Protobuf)
            {
                return HttpContentTypes.Protobuf;
            }
            if (value == UTF8Text)
            {
                return HttpContentTypes.UTF8Text;
            }
            if (value == Text)
            {
                return HttpContentTypes.Text;
            }
            if (value == Undefined)
            {
                return HttpContentTypes.Undefined;
            }
            throw new InvalidCastException();
        }

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"> </param>
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

        /// <summary>
        /// 初始化 <see cref="HttpContentType" />
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="format"> 格式 </param>
        /// <param name="charset"> 编码 </param>
        /// <param name="parser"> 自定义解析器 </param>
        public HttpContentType(string type, string format, Encoding charset, IHttpBodyParser parser = null)
        {
            Type = type;
            Format = format;
            Charset = charset;
            _Parser = parser;
        }

        /// <summary>
        /// 自定义解析器
        /// </summary>
        private readonly IHttpBodyParser _Parser;

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// 格式
        /// </summary>
        public string Format { get; }

        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Charset { get; }

        /// <summary>
        /// 返回当前对象的等效字符串
        /// </summary>
        /// <returns> </returns>
        public override string ToString()
        {
            if (Charset == null)
            {
                if ((Type == null) || (Format == null))
                {
                    return Type ?? Format;
                }
                return $"{Type}/{Format}";
            }
            return $"{Type}/{Format};charset={Charset.WebName}";
        }

        /// <summary>
        /// 返回一个对象，该对象为指定类型提供格式设置服务。
        /// </summary>
        /// <returns> 如果 <see cref="T:System.IFormatProvider" /> 实现能够提供该类型的对象，则为 <paramref name="formatType" /> 所指定对象的实例；否则为 null。 </returns>
        /// <param name="formatType"> 一个对象，该对象指定要返回的格式对象的类型。 </param>
        /// <filterpriority> 1 </filterpriority>
        public object GetFormat(Type formatType)
        {
            if ((formatType == typeof(IHttpBodyParser))
                || (formatType == typeof(ICustomFormatter)))
            {
                return _Parser ?? HttpBodyParsers.Get(Type, Format);
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
        /// 指示此实例与指定对象是否相等。
        /// </summary>
        /// <returns> 如果 <paramref name="obj" /> 和该实例具有相同的类型并表示相同的值，则为 true；否则为 false。 </returns>
        /// <param name="obj"> 要与当前实例进行比较的对象。 </param>
        /// <filterpriority> 2 </filterpriority>
        public override bool Equals(object obj) => obj is HttpContentType && Equals((HttpContentType) obj);

        /// <summary>
        /// 重写 == 运算符
        /// </summary>
        public static bool operator ==(HttpContentType a, HttpContentType b) => a.Equals(b);

        /// <summary>
        /// 重写 != 运算符
        /// </summary>
        public static bool operator !=(HttpContentType a, HttpContentType b) => !a.Equals(b);

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns> 一个 32 位有符号整数，它是该实例的哈希代码。 </returns>
        /// <filterpriority> 2 </filterpriority>
        public override int GetHashCode() => ToString().GetHashCode();

        /// <summary>
        /// 改变当前对象的编码,产生一个新对象
        /// </summary>
        /// <param name="charset"> </param>
        /// <returns> </returns>
        public HttpContentType ChangeCharset(Encoding charset) => new HttpContentType(Type, Format, charset);

        /// <summary>
        /// 指示当前对象是否等于同一类型的另一个对象。
        /// </summary>
        /// <returns> 如果当前对象等于 <paramref name="other" /> 参数，则为 true；否则为 false。 </returns>
        /// <param name="other"> 与此对象进行比较的对象。 </param>
        public bool Equals(HttpContentType other) => Equals(Charset, other.Charset)
                                                     && string.Equals(Format, other.Format, StringComparison.OrdinalIgnoreCase)
                                                     && string.Equals(Type, other.Type, StringComparison.OrdinalIgnoreCase)
                                                     && (_Parser == other._Parser);
    }
}