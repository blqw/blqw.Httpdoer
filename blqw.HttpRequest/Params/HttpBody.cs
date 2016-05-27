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

        public HttpBody(byte[] responseBody, IEnumerable<KeyValuePair<string, object>> body)
            : base(new HttpParameterCollection())
        {
            foreach (var item in body)
            {
                base[item.Key] = item.Value;
            }
            ResponseBody = responseBody;
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
                var charset = ContentType.GetFormat(typeof(Encoding)) as Encoding;
                if (charset != null)
                {
                    return charset.GetString(ResponseBody);
                }
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

    }
}
