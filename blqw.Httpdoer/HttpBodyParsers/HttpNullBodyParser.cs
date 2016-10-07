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
        /// 匹配解析器,返回 true 表示匹配成功
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="format"> 格式 </param>
        /// <returns></returns>
        public override bool IsMatch(string type, string format)
            => type == null && format == null;

        /// <summary>
        /// 固定返回 null
        /// </summary>
        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider) => null;
    }
}
