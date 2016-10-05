using System;
using System.Collections.Generic;
using System.Linq;

namespace blqw.Web
{
    internal class HttpStreamBodyParser : HttpBodyParserBase
    {
        public static HttpStreamBodyParser Instance { get; } = new HttpStreamBodyParser();

        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes,
            IFormatProvider formatProvider) => new[] { new KeyValuePair<string, object>(null, bytes) };

        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body,
            IFormatProvider formatProvider) => body.FirstOrDefault().Value as byte[];
    }
}