using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace blqw.Web
{
    /// <summary>
    /// 用于表示一种类型 HTTP 参数集合的抽象基类
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public abstract class HttpParamsBase<T> : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// 初始化 Http参数集合
        /// </summary>
        /// <param name="params"> 参数容器 </param>
        /// <param name="location"> </param>
        /// <exception cref="ArgumentNullException"> <paramref name="params" /> is null </exception>
        protected HttpParamsBase(IHttpParameterContainer @params, HttpParamLocation location)
        {
            if (@params == null)
            {
                throw new ArgumentNullException(nameof(@params));
            }
            Params = @params;
            Location = location;
        }

        /// <summary>
        /// 参数位置
        /// </summary>
        public HttpParamLocation Location { get; }

        /// <summary>
        /// 参数容器
        /// </summary>
        protected IHttpParameterContainer Params { get; }

        /// <summary>
        /// 获取或设置指定名称的参数
        /// </summary>
        /// <param name="name"> 参数名 </param>
        /// <returns> </returns>
        public virtual T this[string name]
        {
            get { return (T)Params.GetValue(Location, name); }
            set { Params.SetValue(Location, name, value); }
        }


        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns> 用于循环访问集合的枚举数。 </returns>
        /// <filterpriority> 1 </filterpriority>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            => (from item in Params
                where item.Location == Location
                select new KeyValuePair<string, object>(item.Name, item.Value)).GetEnumerator();

        /// <summary> 返回循环访问集合的枚举数。 </summary>
        /// <returns> 可用于循环访问集合的 <see cref="T:System.Collections.IEnumerator" /> 对象。 </returns>
        /// <filterpriority> 2 </filterpriority>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="name"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        public void Set(string name, T value) => Params.SetValue(Location, name, value);

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        public void Add(string name, T value) => Params.AddValue(Location, name, value);

        /// <summary>
        /// 获取指定名称的参数值
        /// </summary>
        /// <param name="name"> 参数名 </param>
        /// <returns> </returns>
        public T Get(string name) => (T)Params.GetValue(Location, name);

        /// <summary>
        /// 获取指定名称的参数值
        /// </summary>
        /// <param name="name"> 参数名 </param>
        /// <returns> </returns>
        /// <exception cref="InvalidCastException"> 序列中的元素不能强制转换为 <typeparamref name="T" /> 类型。 </exception>
        public IEnumerable<T> GetValues(string name) => Params.GetValues(Location, name)?.Cast<T>();

        /// <summary>
        /// 清空集合中的所有参数
        /// </summary>
        public void Clear()
        {
            var keys = (from item in Params where item.Location == Location select item.Name).ToArray();
            for (int i = 0, length = keys.Length; i < length; i++)
            {
                Params.Remove(Location, keys[i]);
            }
        }

        /// <summary>
        /// 判断指定名称的参数是否存在
        /// </summary>
        /// <param name="name">待判断的参数名称</param>
        /// <returns></returns>
        public bool Contains(string name) => Params.Contains(Location, name);
    }
}