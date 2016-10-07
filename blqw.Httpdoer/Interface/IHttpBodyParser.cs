using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace blqw.Web
{
    /// <summary>
    /// 表示一个正文解析器
    /// </summary>
    [InheritedExport(typeof(IHttpBodyParser))]
    public interface IHttpBodyParser : ICustomFormatter
    {
        /// <summary>
        /// 匹配解析器,返回 true 表示匹配成功
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="format"> 格式 </param>
        /// <returns></returns>
        bool IsMatch(string type, string format);

        /// <summary>
        /// 将正文格式化为字节流
        /// </summary>
        /// <param name="format"> 包含格式规范的格式字符串 </param>
        /// <param name="body"> 请求或响应正文 </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider);

        /// <summary>
        /// 将字节流转换为键值对枚举
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider);

        /// <summary>
        /// 将字节流转换为指定对象
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        T Deserialize<T>(byte[] bytes, IFormatProvider formatProvider);
    }
}