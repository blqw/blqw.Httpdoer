using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace blqw.Web
{
    /// <summary>
    /// 默认解析器,当存在charset时解析为字符串,否则解析为字节流
    /// </summary>
    //[Export(typeof(IHttpBodyParser))]
    [ExportMetadata("Priority", int.MinValue)]
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

        /// <summary>
        /// 匹配解析器,返回 true 表示匹配成功
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="format"> 格式 </param>
        /// <returns></returns>
        public override bool IsMatch(string type, string format) => true;

        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}