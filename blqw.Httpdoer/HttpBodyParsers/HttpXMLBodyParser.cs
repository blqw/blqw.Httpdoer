using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace blqw.Web
{
    internal class HttpXMLBodyParser : HttpBodyParserBase
    {
        public static HttpXMLBodyParser Instance { get; } = new HttpXMLBodyParser();
        
        static readonly XmlSerializer XmlDeserializer = new XmlSerializer(typeof(List<KeyValuePair<string, object>>));

        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            var charset = GetEncoding(formatProvider) ?? Encoding.UTF8;
            var xml = charset.GetString(bytes);
            using (var stream = new StreamReader(new MemoryStream(bytes), charset))
            {
                return (Dictionary<string, object>)XmlDeserializer.Deserialize(stream);
            }
        }

        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
        

    }
}
