using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public abstract class HttpBodyParserBase : IHttpBodyParser
    {
        public abstract string Format(string format, object arg, IFormatProvider formatProvider);
        public virtual byte[] Format(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            var str = ((ICustomFormatter)this).Format(format, body, formatProvider);
            var charset = formatProvider?.GetFormat(typeof(Encoding)) as Encoding
                        ?? Encoding.UTF8;
            return charset.GetBytes(str);
        }
    }
}
