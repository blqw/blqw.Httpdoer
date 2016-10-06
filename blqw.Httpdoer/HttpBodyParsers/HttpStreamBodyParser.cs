using System;
using System.Collections.Generic;
using System.Linq;

namespace blqw.Web
{
    /// <summary>
    /// 流解析器,用于解析字节流形式的正文
    /// </summary>
    public class HttpStreamBodyParser : HttpBodyParserBase
    {
        /// <summary>
        /// 将字节流转换为键值对枚举
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes,
            IFormatProvider formatProvider) => new[] { new KeyValuePair<string, object>(null, bytes) };

        /// <summary>
        /// 将正文格式化为字节流
        /// </summary>
        /// <param name="format"> 包含格式规范的格式字符串 </param>
        /// <param name="body"> 请求或响应正文 </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body,
            IFormatProvider formatProvider) => body.FirstOrDefault(it => it.Key == null).Value as byte[];
    }
}