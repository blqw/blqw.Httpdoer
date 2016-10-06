using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// Protobuf解析器,用于解析 Protobuf 格式的正文
    /// </summary>
    internal sealed class HttpProtobufBodyParser : HttpBodyParserBase
    {
        /// <summary>
        /// 暂未实现
        /// </summary>
        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 暂未实现
        /// </summary>
        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}
