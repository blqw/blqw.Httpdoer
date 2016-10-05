using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 表示请求或响应的正文
    /// </summary>
    public sealed class HttpBody : HttpParamsBase<object>, IFormattable
    {
        internal HttpBody(IHttpParameterCollection @params)
            : base(@params)
        {
            ContentType = HttpContentType.Undefined;
        }

        public HttpBody(HttpContentType contentType, byte[] responseBody)
            : base(new LazyHttpParameterCollection(contentType, responseBody, HttpParamLocation.Body))
        {
            ResponseBody = responseBody;
            ContentType = contentType;
        }

        public override HttpParamLocation Location
        {
            get
            {
                return HttpParamLocation.Body;
            }
        }

        private HttpContentType _ContentType;
        public HttpContentType ContentType
        {
            get
            {
                return _ContentType;
            }
            set
            {
                Params.SetValue("Content-Type", value.ToString(), HttpParamLocation.Header);
                _ContentType = value;
            }
        }

        public byte[] ResponseBody { get; }

        public string ToString(string format)
        {
            return ToString(format, ContentType);
        }

        public override string ToString()
        {
            if (ResponseBody != null)
            {
                var charset = ContentType.GetFormat(typeof(Encoding)) as Encoding ?? Encoding.Default;
                return charset.GetString(ResponseBody);
            }
            return ToString(null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                formatProvider = ContentType;
            }
            var parser = formatProvider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
            return parser?.Format(format, this, formatProvider);
        }

        public T ToObject<T>()
        {
            var parser = ContentType.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            Debug.Assert(parser != null, "parser != null");
            var body = ResponseBody ?? parser.Serialize(null, this, ContentType);
            return parser.Deserialize<T>(body, ContentType);
        }

        public byte[] GetBytes()
        {
            var parser = ContentType.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            Debug.Assert(parser != null, "parser != null");
            return ResponseBody ?? parser.Serialize(null, this, ContentType);
        }
    }
}
