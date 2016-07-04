using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// Body解析器
    /// </summary>
    public static class HttpBodyParsers
    {
        public static HttpBodyParserBase Default { get; } = HttpBodyDefaultParser.Instance;
        public static HttpBodyParserBase Form { get; } = HttpFormBodyParser.Instance;
        public static HttpBodyParserBase Json { get; } = HttpJsonBodyParser.Instance;
        public static HttpBodyParserBase Null { get; } = HttpNullBodyParser.Instance;
        public static HttpBodyParserBase Protobuf { get; } = HttpProtobufBodyParser.Instance;
        public static HttpBodyParserBase Stream { get; } = HttpStreamBodyParser.Instance;
        public static HttpBodyParserBase Text { get; } = HttpTextBodyParser.Instance;
        public static HttpBodyParserBase XML { get; } = HttpXMLBodyParser.Instance;

    }
}
