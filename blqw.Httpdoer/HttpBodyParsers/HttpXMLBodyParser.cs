using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace blqw.Web
{
    /// <summary>
    /// XML解析器,用于解析 XML 格式的正文
    /// </summary>
    internal class HttpXMLBodyParser : HttpBodyParserBase
    {
        /// <summary>
        /// XML序列化组件
        /// </summary>
        private static readonly XmlSerializer _XmlDeserializer = new XmlSerializer(typeof(List<KeyValuePair<string, object>>));

        /// <summary>
        /// 将字节流转换为键值对枚举
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            var charset = GetEncoding(formatProvider) ?? Encoding.Default;
            using (var stream = new StreamReader(new MemoryStream(bytes), charset))
            {
                return (Dictionary<string, object>)_XmlDeserializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// 匹配解析器,返回 true 表示匹配成功
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="format"> 格式 </param>
        /// <returns></returns>
        public override bool IsMatch(string type, string format)
            => format?.Length == 3 && format.EndsWith("xml", StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 将正文格式化为字节流(暂未实现)
        /// </summary>
        /// <param name="format"> 包含格式规范的格式字符串 </param>
        /// <param name="body"> 请求或响应正文 </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }
    }
}