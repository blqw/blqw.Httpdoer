using System;
using System.Collections;
using System.Collections.Generic;
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

        public HttpContentType ContentType
        {
            get
            {
                return (string)Params.GetValue("Content-Type", HttpParamLocation.Header);
            }
            set
            {
                Params.SetValue("Content-Type", value.ToString(), HttpParamLocation.Header);
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
            if (parser == null)
            {
                return null;
            }
            return parser.Format(format, this, formatProvider);
        }

        public T ToObject<T>()
        {
            var parser = ContentType.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            var body = ResponseBody ?? parser.Serialize(null, this, ContentType);
            return parser.Deserialize<T>(body, ContentType);
        }
    }
}
