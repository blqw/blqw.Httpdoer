using System;
using System.Diagnostics;
using System.Reflection;
using blqw.IOC;

namespace blqw.Reflection
{
    /// <summary>
    /// 表示属性的操作
    /// </summary>
    internal class PropertyHandler
    {
        /// <summary>
        /// 设置属性值的委托
        /// </summary>
        private readonly Func<object, object> _getter;

        /// <summary>
        /// 设置属性值的委托
        /// </summary>
        private readonly Action<object, object> _setter;

        /// <summary>
        /// 初始化属性操作器
        /// </summary>
        /// <param name="property"> 需要操作的属性 </param>
        /// <exception cref="Exception">
        /// IOC插件异常 <seealso cref="ComponentServices.GetGeter" /> 或
        /// <seealso cref="ComponentServices.GetSeter" /> 出现错误.
        /// </exception>
        public PropertyHandler(PropertyInfo property)
        {
            Name = property.Name;
            PropertyType = property.PropertyType;
            Debug.Assert(ComponentServices.GetGeter != null, "ComponentServices.GetGeter != null");
            Debug.Assert(ComponentServices.GetSeter != null, "ComponentServices.GetSeter != null");
            _getter = ComponentServices.GetGeter(property);
            _setter = ComponentServices.GetSeter(property);
        }

        /// <summary>
        /// 属性类型
        /// </summary>
        public Type PropertyType { get; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <exception cref="NotSupportedException"> 属性没有get方法或属性是只写的 </exception>
        /// <exception cref="Exception"> 其他错误. </exception>
        public object GetValue(object instance)
        {
            if (_getter == null)
            {
                throw new NotSupportedException("属性没有get方法或属性是只写的");
            }
            return _getter(instance);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="instance"> </param>
        /// <param name="value"> </param>
        /// <exception cref="NotSupportedException"> 属性没有set方法或属性是只读的 </exception>
        /// <exception cref="InvalidCastException"> <paramref name="value" />不是<see cref="PropertyType" />且类型转换失败 </exception>
        /// <exception cref="Exception"> 其他错误. </exception>
        public void SetValue(object instance, object value)
        {
            if (_setter == null)
            {
                throw new NotSupportedException("属性没有set方法或属性是只读的");
            }
            if (PropertyType.IsInstanceOfType(value) == false)
            {
                try
                {
                    value = ComponentServices.Converter.Convert(value, PropertyType);
                }
                catch (Exception ex)
                {
                    throw new InvalidCastException(ex.Message, ex);
                }
            }
            _setter(instance, value);
        }
    }
}