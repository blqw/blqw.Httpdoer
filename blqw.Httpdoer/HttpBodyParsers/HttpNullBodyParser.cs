using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    internal class HttpNullBodyParser : HttpBodyParserBase
    {
        public static HttpNullBodyParser Instance { get; } = new HttpNullBodyParser();

        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            return null;
        }

        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            foreach (var item in body)
            {

            }
            return null;
        }
    }
}
