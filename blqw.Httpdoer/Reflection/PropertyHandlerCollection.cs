using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;

namespace blqw.Reflection
{
    /// <summary>
    /// 表示 <seealso cref="PropertyHandler" /> 的集合
    /// </summary>
    internal class PropertyHandlerCollection : NameObjectCollectionBase
    {
        /// <summary>
        /// 根据<seealso cref="Type" />缓存类型中的所有属性
        /// </summary>
        private static readonly ConcurrentDictionary<Type, PropertyHandlerCollection> TypesCache = new ConcurrentDictionary<Type, PropertyHandlerCollection>();

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private PropertyHandlerCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// 根据索引获取属性
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public PropertyHandler this[int index] => (PropertyHandler)BaseGet(index);

        /// <summary>
        /// 根据属性名称获取属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns></returns>
        public PropertyHandler this[string name] => (PropertyHandler)BaseGet(name);

        /// <summary>
        /// 从缓存中获取指定类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyHandlerCollection Get(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return TypesCache.GetOrAdd(type, t =>
            {
                var dict = new PropertyHandlerCollection();
                foreach (var p in t.GetProperties())
                {
                    dict.BaseSet(p.Name, new PropertyHandler(p));
                }
                return dict;
            });
        }
    }
}