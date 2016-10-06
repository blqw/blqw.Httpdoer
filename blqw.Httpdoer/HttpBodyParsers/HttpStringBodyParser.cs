using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace blqw.Web
{
    /// <summary>
    /// 字符串解析器,用于字符串类型的正文,包括text,html,plain等
    /// </summary>
    public class HttpStringBodyParser : HttpBodyParserBase
    {
        /// <summary>
        /// 表示一个空的<seealso cref="byte"/>数组
        /// </summary>
        private static readonly byte[] _EmptyBytes = new byte[0];

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
                var charset = formatProvider?.GetFormat(typeof(Encoding)) as Encoding ?? Encoding.Default;
                var text = charset.GetString(bytes);
                yield return new KeyValuePair<string, object>(null, text);
            }
        }

        /// <summary>
        /// 将正文格式化为字节流
        /// </summary>
        /// <param name="format"> 包含格式规范的格式字符串 </param>
        /// <param name="body"> 请求或响应正文 </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            var text = body.FirstOrDefault(it => it.Key == null).Value as string;
            if (string.IsNullOrEmpty(text))
            {
                return _EmptyBytes;
            }
            var charset = GetEncoding(formatProvider) ?? Encoding.Default;
            return charset.GetBytes(text);
        }
    }
}