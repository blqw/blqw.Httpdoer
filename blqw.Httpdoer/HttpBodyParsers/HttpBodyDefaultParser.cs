using System;
using System.Collections.Generic;
using System.Text;

namespace blqw.Web
{
    /// <summary>
    /// 默认解析器,当存在charset时解析为字符串,否则解析为字节流
    /// </summary>
    internal sealed class HttpBodyDefaultParser : HttpBodyParserBase
    {
        /// <summary>
        /// 将字节流转换为键值对枚举
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            if (bytes?.Length > 0)
            {
                var charset = formatProvider?.GetFormat(typeof(Encoding)) as Encoding;
                if (charset == null)
                {
                    yield return new KeyValuePair<string, object>(null, bytes);
                }
                else
                {
                    var text = charset.GetString(bytes);
                    yield return new KeyValuePair<string, object>(null, text);
                }
            }
        }

        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}