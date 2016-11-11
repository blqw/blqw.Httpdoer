using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace blqw.Web
{
    /// <summary>
    /// 表示请求或响应的正文
    /// </summary>
    public sealed class HttpBody : HttpParamsBase<object>, IFormattable
    {
        /// <summary>
        /// 为属性<see cref="ContentType" />提供数据
        /// </summary>
        private HttpContentType _contentType;

        /// <summary>
        /// 初始化请求正文
        /// </summary>
        /// <param name="params"> 参数容器 </param>
        internal HttpBody(IHttpParameterContainer @params)
            : base(@params, HttpParamLocation.Body)
        {
            ContentType = HttpContentType.Undefined;
            IsResponseBody = false;
        }

        /// <summary>
        /// 初始化响应正文
        /// </summary>
        /// <param name="contentType"> 正文内容类型 </param>
        /// <param name="responseBody"> 响应正文数据 </param>
        public HttpBody(HttpContentType contentType, byte[] responseBody)
            : base(new LazyDeserializeBodyParameters(contentType, responseBody), HttpParamLocation.Body)
        {
            ResponseBody = responseBody;
            ContentType = contentType;
            IsResponseBody = true;
        }

        /// <summary>
        /// 正文内容类型
        /// </summary>
        public HttpContentType ContentType
        {
            get
            {
                if (_contentType.Format == null && _contentType.Charset == null && _contentType.Type == null)
                {
                    if (Params.Any(it => it.Location == HttpParamLocation.Body))
                    {
                        _contentType = HttpContentType.Form;
                    }
                }
                return _contentType;
            }
            set
            {
                Params.SetValue(HttpParamLocation.Header, "Content-Type", value.ToString());
                _contentType = value;
            }
        }

        /// <summary>
        /// 是否是响应正文
        /// </summary>
        public bool IsResponseBody { get; }

        /// <summary>
        /// 响应正文字节
        /// </summary>
        public byte[] ResponseBody { get; }

        /// <summary>
        /// 使用指定的格式格式化当前实例的值。
        /// </summary>
        /// <returns> 使用指定格式的当前实例的值。 </returns>
        /// <param name="format">
        /// 要使用的格式。- 或 -null 引用（Visual Basic 中为 Nothing）将使用为 <see cref="T:System.IFormattable" />
        /// 实现的类型所定义的默认格式。
        /// </param>
        /// <param name="formatProvider"> 要用于设置值格式的提供程序。- 或 -null 引用（Visual Basic 中为 Nothing）将从操作系统的当前区域设置中获取数字格式信息。 </param>
        /// <filterpriority> 2 </filterpriority>
        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                formatProvider = ContentType;
            }
            var parser = formatProvider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
            Debug.Assert(parser != null, "parser != null");
            return parser.Format(format, this, formatProvider);
        }

        /// <summary>
        /// 根据<paramref name="format" />参数返回使用指定格式的当前正文的字符串表现形式
        /// </summary>
        /// <param name="format"> </param>
        /// <returns> </returns>
        public string ToString(string format) => ((IFormattable)this).ToString(format, ContentType);

        /// <summary>
        /// 返回当前正文的字符串表现形式
        /// </summary>
        /// <returns> </returns>
        public override string ToString()
        {
            if (ResponseBody != null)
            {
                var charset = ContentType.GetFormat(typeof(Encoding)) as Encoding ?? Encoding.Default;
                return charset.GetString(ResponseBody);
            }
            return ((IFormattable)this).ToString(null, ContentType);
        }

        /// <summary>
        /// 将当前正文转为指定的类型实体
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        public T ToObject<T>()
        {
            var parser = ContentType.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            Debug.Assert(parser != null, "parser != null");
            var body = ResponseBody ?? parser.Serialize(null, this, ContentType);
            return parser.Deserialize<T>(body, ContentType);
        }

        /// <summary>
        /// 获取当前正文的字节信息
        /// </summary>
        /// <returns> </returns>
        public byte[] GetBytes()
        {
            if (ResponseBody != null)
            {
                return ResponseBody;
            }
            var parser = ContentType.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            Debug.Assert(parser != null, "parser != null");
            return parser.Serialize(null, this, ContentType);
        }
    }
}