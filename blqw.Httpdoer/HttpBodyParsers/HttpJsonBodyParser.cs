using blqw.HttpRequestComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    internal class HttpJsonBodyParser : HttpBodyParserBase
    {
        public static HttpJsonBodyParser Instance { get; } = new HttpJsonBodyParser();

        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            var charset = GetEncoding(formatProvider) ?? Encoding.UTF8;
            var json = charset.GetString(bytes);
            return (Dictionary<string, object>)Component.ToJsonObject(typeof(Dictionary<string, object>), json);
        }

        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            var json = Component.ToJsonString(body);
            var charset = GetEncoding(formatProvider) ?? Encoding.UTF8;
            return charset.GetBytes(json);
        }

        public override T Deserialize<T>(byte[] bytes, IFormatProvider formatProvider)
        {
            var charset = GetEncoding(formatProvider) ?? Encoding.UTF8;
            var json = charset.GetString(bytes);
            return (T)Component.ToJsonObject(typeof(T), json);
        }
    }
}
