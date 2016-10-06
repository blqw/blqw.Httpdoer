using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 空解析器,没有任何功能
    /// </summary>
    internal sealed class HttpNullBodyParser : HttpBodyParserBase
    {
        /// <summary>
        /// 固定返回 null
        /// </summary>
        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider) => null;

        /// <summary>
        /// 固定返回 null
        /// </summary>
        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider) => null;
    }
}
