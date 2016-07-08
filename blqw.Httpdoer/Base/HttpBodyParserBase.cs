using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public abstract class HttpBodyParserBase : IHttpBodyParser
    {
        public abstract IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider);
        public abstract byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider);
        public virtual T Deserialize<T>(byte[] bytes, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public virtual string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return null;
            }
            var body = arg as IEnumerable<KeyValuePair<string, object>>;
            if (body == null)
            {
                throw new FormatException(nameof(arg) + "必须是" + nameof(IEnumerable<KeyValuePair<string, object>>));
            }
            var bytes = Serialize(format, body, formatProvider);
            if (bytes?.Length > 0)
            {
                var charset = formatProvider?.GetFormat(typeof(Encoding)) as Encoding ?? Encoding.UTF8;
                return charset.GetString(bytes);
            }
            return null;
        }

        protected Encoding GetEncoding(IFormatProvider formatProvider)
        {
            return formatProvider?.GetFormat(typeof(Encoding)) as Encoding;
        }

    }
}
