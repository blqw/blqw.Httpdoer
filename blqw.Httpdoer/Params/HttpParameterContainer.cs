using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace blqw.Web
{
    /// <summary>
    /// HTTP 参数容器的标准实现
    /// </summary>
    internal class HttpParameterContainer : NameObjectCollectionBase, IHttpParameterContainer
    {
        /// <summary>
        /// 获取<seealso cref="HttpParamValue" />
        /// </summary>
        /// <param name="location"> 参数位置 </param>
        /// <param name="name"> 参数名 </param>
        /// <returns> </returns>
        public HttpParamValue Get(HttpParamLocation location, string name)
        {
            var key = CreateKey(name, location);
            var value = BaseGet(key);
            if (value == null)
            {
                return new HttpParamValue(name, null, location);
            }
            return (HttpParamValue) value;
        }

        /// <summary>
        /// 获取参数值,如果指定名称的参数有多个值,只返回一个匹配的参数值
        /// </summary>
        /// <param name="location"> 参数位置 </param>
        /// <param name="name"> 参数名 </param>
        /// <returns> </returns>
        public object GetValue(HttpParamLocation location, string name) => Get(location, name).FirstValue;

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="location"> 参数位置 </param>
        /// <param name="name"> 参数名 </param>
        /// <returns> </returns>
        public IEnumerable GetValues(HttpParamLocation location, string name) => Get(location, name).Values;

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="location"> 参数位置 </param>
        /// <param name="name"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        public void SetValue(HttpParamLocation location, string name, object value)
        {
            var key = CreateKey(name, location);
            if (value == null)
            {
                BaseRemove(key);
            }
            else
            {
                var old = (HttpParamValue) BaseGet(key);
                if (old == null)
                {
                    BaseSet(key, new HttpParamValue(name, value, location));
                }
                else
                {
                    old.SetValue(value);
                }
            }
        }

        /// <summary>
        /// 判断指定名称的参数是否存在
        /// </summary>
        /// <param name="location"> 参数位置 </param>
        /// <param name="name"> 参数名 </param>
        /// <returns> </returns>
        public bool Contains(HttpParamLocation location, string name) => BaseGet(CreateKey(name, location)) != null;

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns> 用于循环访问集合的枚举数。 </returns>
        /// <filterpriority> 1 </filterpriority>
        public new IEnumerator<HttpParamValue> GetEnumerator() => BaseGetAllValues().Cast<HttpParamValue>().GetEnumerator();


        /// <summary>
        /// 移除指定名称的参数
        /// </summary>
        /// <param name="location"> 待移除的参数位置 </param>
        /// <param name="name"> 待移除的参数名称 </param>
        public void Remove(HttpParamLocation location, string name) => BaseRemove(CreateKey(name, location));

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="location"> 参数位置 </param>
        /// <param name="name"> 参数名称 </param>
        /// <param name="value"> 参数值 </param>
        public void AddValue(HttpParamLocation location, string name, object value)
        {
            if (value == null)
            {
                return;
            }

            var key = CreateKey(name, location);
            var param = (HttpParamValue) BaseGet(key);
            if (param == null) //如果指定名称的参数不存在,直接添加
            {
                BaseSet(key, new HttpParamValue(name, value, location));
            }
            else
            {
                param.AddValue(value);
            }
        }

        /// <summary>
        /// 根据参数新建一个key
        /// </summary>
        /// <param name="name"> 参数名 </param>
        /// <param name="location"> 参数位置 </param>
        /// <returns> </returns>
        private string CreateKey(string name, HttpParamLocation location)
        {
            if (name == null)
            {
                return $"{location}(null)";
            }
            return $"{location} | {name.ToLowerInvariant()}";
        }
    }
}