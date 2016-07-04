using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    internal class HttpStreamBodyParser : HttpBodyParserBase
    {
        public static HttpStreamBodyParser Instance { get; } = new HttpStreamBodyParser();
        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            yield return new KeyValuePair<string, object>(null, bytes);
        }

        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            return body.FirstOrDefault().Value as Byte[];
        }
    }
}
