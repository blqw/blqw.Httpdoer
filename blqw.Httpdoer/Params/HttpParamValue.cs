using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace blqw.Web
{
    /// <summary>
    /// 表示一个 HTTP 请求中的参数
    /// </summary>
    [DebuggerDisplay("{DebugInfo}")]
    public class HttpParamValue
    {
        /// <summary>
        /// 实际值
        /// </summary>
        private object _value;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="name"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        /// <param name="location"> 参数位置 </param>
        public HttpParamValue(string name, object value, HttpParamLocation location)
        {
            Name = name;
            Location = location;
            SetValue(value);
        }

        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 参数值集合
        /// </summary>
        public List<object> Values
        {
            get
            {
                var list = _value as ValueCollection;
                if (list == null)
                {
                    if (_value == null)
                    {
                        _value = list = new ValueCollection();
                    }
                    else
                    {
                        _value = list = new ValueCollection { _value };
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// 如果是集合返回第一个参数值
        /// </summary>
        public object FirstValue
        {
            get
            {
                var list = _value as ValueCollection;
                if (list == null)
                {
                    return _value;
                }
                if (list.Count == 0)
                {
                    return null;
                }
                return list[0];
            }
        }

        /// <summary>
        /// 参数值,<see cref="IsMultiValue" /> = true 返回 <see cref="Values" /> 否则 返回<see cref="FirstValue" />
        /// </summary>
        public object Value
        {
            get
            {
                var list = _value as ValueCollection;
                return list?.Count == 1 ? list[0] : _value;
            }
        }

        /// <summary>
        /// 是否有多个值
        /// </summary>
        public bool IsMultiValue => (_value as ValueCollection)?.Count > 1;

        /// <summary>
        /// 参数位置
        /// </summary>
        public HttpParamLocation Location { get; }

        /// <summary>
        /// 调试信息
        /// </summary>
        private string DebugInfo
        {
            get
            {
                if (IsMultiValue)
                {
                    var name = Name;
                    return $"{Location,7}|" + string.Join(", ", Values.Select(it => $"[{name}] {it}"));
                }
                return $"{Location,7}|[{Name}] {FirstValue}";
            }
        }

        /// <summary>
        /// 向当前参数追加值
        /// </summary>
        /// <param name="value"> 带追加的参数值 </param>
        public void AddValue(object value)
        {
            if (_value == null)
            {
                _value = value;
                return;
            }

            var list = _value as ValueCollection;
            if (list == null)
            {
                _value = list = new ValueCollection { _value };
            }
            list.Add(value);
        }

        /// <summary>
        /// 用一个新的值覆盖旧的值
        /// </summary>
        /// <param name="value"> 新的参数值 </param>
        public void SetValue(object value)
        {
            _value = null;
            AddValue(value);
        }

        /// <summary>
        /// 返回参数值的字符串形式
        /// </summary>
        /// <returns> </returns>
        public override string ToString() => IsMultiValue ? string.Join(", ", Values) : FirstValue.ToString();

        /// <summary>
        /// 内部类,避免与系统类型冲突
        /// </summary>
        private sealed class ValueCollection : List<object>
        {
        }
    }
}