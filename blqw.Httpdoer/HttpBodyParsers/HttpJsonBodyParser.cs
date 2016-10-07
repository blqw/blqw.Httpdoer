using System;
using System.Collections.Generic;
using System.Text;
using blqw.IOC;

namespace blqw.Web
{
    /// <summary>
    /// Json解析器,用于解析 Json 格式的正文
    /// </summary>
    internal sealed class HttpJsonBodyParser : HttpBodyParserBase
    {
        /// <summary>
        /// 将字节流转换为键值对枚举
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            var charset = GetEncoding(formatProvider) ?? Encoding.UTF8;
            var json = charset.GetString(bytes);
            return (Dictionary<string, object>) ComponentServices.ToJsonObject(typeof(Dictionary<string, object>), json);
        }

        /// <summary>
        /// 匹配解析器,返回 true 表示匹配成功
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="format"> 格式 </param>
        /// <returns></returns>
        public override bool IsMatch(string type, string format)
            => format?.Length == 4 && format.EndsWith("json", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 将正文格式化为字节流
        /// </summary>
        /// <param name="format"> 包含格式规范的格式字符串 </param>
        /// <param name="body"> 请求或响应正文 </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            var json = ComponentServices.ToJsonString(body);
            var charset = GetEncoding(formatProvider) ?? Encoding.UTF8;
            return charset.GetBytes(json);
        }

        /// <summary>
        /// 将字节流转换为指定对象
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override T Deserialize<T>(byte[] bytes, IFormatProvider formatProvider)
        {
            var charset = GetEncoding(formatProvider) ?? Encoding.UTF8;
            var json = charset.GetString(bytes);
            return (T) ComponentServices.ToJsonObject(typeof(T), json);
        }
    }
}