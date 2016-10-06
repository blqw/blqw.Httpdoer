using System.Collections;
using System.Collections.Generic;

namespace blqw.Web
{
    /// <summary>
    /// HTTP 参数容器,包含所有类型的参数
    /// </summary>
    public interface IHttpParameterContainer : IEnumerable<HttpParamValue>
    {
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="location">参数位置</param>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        void SetValue(HttpParamLocation location, string name, object value);
        /// <summary>
        /// 判断指定名称的参数是否存在
        /// </summary>
        /// <param name="location">待判断参数位置</param>
        /// <param name="name">待判断的参数名称</param>
        /// <returns></returns>
        bool Contains(HttpParamLocation location, string name);
        /// <summary>
        /// 移除指定名称的参数
        /// </summary>
        /// <param name="location">待移除的参数位置</param>
        /// <param name="name">待移除的参数名称</param>
        void Remove(HttpParamLocation location, string name);
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="location">参数位置</param>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        void AddValue(HttpParamLocation location, string name, object value);
        /// <summary>
        /// 获取参数值,如果指定名称的参数有多个值,只返回一个匹配的参数值
        /// </summary>
        /// <param name="location">参数位置</param>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        object GetValue(HttpParamLocation location, string name);
        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="location">参数位置</param>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        IEnumerable GetValues(HttpParamLocation location, string name);
        /// <summary>
        /// 获取<seealso cref="HttpParamValue"/>
        /// </summary>
        /// <param name="location">参数位置</param>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        HttpParamValue Get(HttpParamLocation location, string name);
    }
}