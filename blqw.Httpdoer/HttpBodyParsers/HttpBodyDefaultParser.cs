using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    internal class HttpBodyDefaultParser : HttpBodyParserBase
    {
        public static HttpBodyDefaultParser Instance { get; } = new HttpBodyDefaultParser();

        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            if (bytes?.Length > 0)
            {
                var charset = formatProvider?.GetFormat(typeof(Encoding)) as Encoding;
                if (charset == null)
                {
                    yield return new KeyValuePair<string, object>(null, bytes);
                }
                else
                {
                    var text = charset.GetString(bytes);
                    yield return new KeyValuePair<string, object>(null, text);
                }
            }
        }

        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}
