using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace blqw.Web
{
    /// <summary>
    /// 表单解析器,用于解析 x-www-form-urlencoded 类型的正文
    /// </summary>
    internal sealed class HttpFormBodyParser : HttpBodyParserBase
    {
        /// <summary>
        /// 静态缓存对象
        /// </summary>
        [ThreadStatic]
        private static HttpUrlEncodedBuilder _UrlEncodedBuilder;

        /// <summary>
        /// 将字节流转换为键值对枚举
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            var encoding = GetEncoding(formatProvider) ?? Encoding.Default;
            var str = encoding.GetString(bytes);
            var nv = HttpUtility.ParseQueryString(str);
            foreach (string name in nv)
            {
                var values = nv.GetValues(name);
                yield return new KeyValuePair<string, object>(name, values?.Length == 1 ? (object)values[0] : values);
            }
        }

        /// <summary>
        /// 匹配解析器,返回 true 表示匹配成功
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="format"> 格式 </param>
        /// <returns></returns>
        public override bool IsMatch(string type, string format)
            => format?.Length == "x-www-form-urlencoded".Length && format?.EndsWith("x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) == true;

        /// <summary>
        /// 将正文格式化为字节流
        /// </summary>
        /// <param name="format"> 包含格式规范的格式字符串 </param>
        /// <param name="body"> 请求或响应正文 </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            if (_UrlEncodedBuilder == null)
            {
                _UrlEncodedBuilder = new HttpUrlEncodedBuilder();
            }
            else
            {
                _UrlEncodedBuilder.Clear();
            }

            foreach (var item in body)
            {
                _UrlEncodedBuilder.AppendObject(item.Key, item.Value);
            }
            var encoding = GetEncoding(formatProvider) ?? Encoding.ASCII;
            return encoding.GetBytes(_UrlEncodedBuilder.ToString());
        }
    }
}