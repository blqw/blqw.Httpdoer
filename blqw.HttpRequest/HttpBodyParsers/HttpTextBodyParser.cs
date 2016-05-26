using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace blqw.Web
{
    internal class HttpTextBodyParser : HttpBodyParserBase
    {
        public static HttpTextBodyParser Instance { get; } = new HttpTextBodyParser();

        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            if (bytes?.Length > 0)
            {
                var charset = formatProvider?.GetFormat(typeof(Encoding)) as Encoding
                               ?? Encoding.UTF8;
                var text = charset.GetString(bytes);
                yield return new KeyValuePair<string, object>(null, text);
            }
        }

        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            var text = body.FirstOrDefault(it => it.Key == null).Value as string;
            if (string.IsNullOrEmpty(text))
            {
                return EmptyBytes;
            }
            var charset = formatProvider?.GetFormat(typeof(Encoding)) as Encoding
                           ?? Encoding.UTF8;
            return charset.GetBytes(text);
        }

        static readonly byte[] EmptyBytes = new byte[0];
    }
}