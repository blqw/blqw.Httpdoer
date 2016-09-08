using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 表示一个Http请求中的参数
    /// </summary>
    [DebuggerDisplay("{DebugInfo}")]
    public struct HttpParamValue
    {
        public HttpParamValue(IHttpParameterCollection owner, string name, object value, HttpParamLocation location)
        {
            Name = name;
            Location = location;
            Owner = owner;
            Values = (value as List<object>);
            _Value = value;
        }

        internal IHttpParameterCollection Owner { get; }

        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; }

        private object _Value;
        public List<object> Values { get; }
        /// <summary>
        /// 参数值
        /// </summary>
        public object Value
        {
            get
            {
                if (Values != null)
                {
                    return Values.FirstOrDefault();
                }
                return _Value;
            }
            private set
            {
                _Value = value;
            }
        }

        public bool IsArray { get { return Values != null; } }

        public void Add(object value)
        {
            if (Owner == null)
            {
                throw new ArgumentNullException(nameof(Owner));
            }
            if (value == null) return;
            if (Values != null)
            {
                Values.Add(value);
            }
            else if (_Value != null)
            {
                lock (array)
                {
                    array[0] = _Value;
                    array[1] = value;
                    Owner.AddValue(Name, array, Location);
                }
            }
            else
            {
                Owner.AddValue(Name, value, Location);
            }
        }

        static object[] array = new object[2];

        /// <summary>
        /// 参数位置
        /// </summary>
        public HttpParamLocation Location { get; }

        private string DebugInfo
        {
            get
            {
                if (IsArray)
                {
                    var name = Name;
                    return $"{Location,7}|" + string.Join(", ", Values.Select(it => $"[{name}] {it}"));
                }
                return $"{Location,7}|[{Name}] {Value}";
            }
        }

        public override string ToString()
        {
            if (IsArray)
            {
                return string.Join(", ", Values);
            }
            return Value + "";
        }
    }
}
