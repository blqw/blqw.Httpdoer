using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 表示一个Http请求中的参数
    /// </summary>
    public struct HttpParamValue
    {
        public HttpParamValue(string name, object value)
            : this(name, value, HttpParamLocation.Auto)
        {
        }

        public HttpParamValue(string name, object value, HttpParamLocation location)
        {
            Name = name;
            Value = value;
            Location = location;
        }
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// 参数位置
        /// </summary>
        public HttpParamLocation Location { get; }

        public override string ToString()
        {
            return $"{Location} : {Name} = {Value}";
        }
    }
}
