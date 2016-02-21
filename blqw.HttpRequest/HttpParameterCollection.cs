using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using blqw.HttpRequestComponent;

namespace blqw.Web
{
    /// <summary> 用于表示http请求中的参数集合
    /// </summary>
    /// <remarks>周子鉴 2015.08.01</remarks>
    public class HttpParameterCollection : NameValueCollection
    {

        /// <summary> 初始化参数集
        /// </summary>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public HttpParameterCollection()
            : base(0, StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary> 连接参数名,如果存在前缀的话 组成 `前缀.参数名` 的格式
        /// </summary>
        /// <param name="pre">参数名前缀</param>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        /// <remarks>周子鉴 2015.08.01</remarks>
        private static string ConcatName(string pre, string name)
        {
            return pre == null ? name : pre + "." + name;
        }

        /// <summary> 追加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public new void Add(string name, string value)
        {
            base.Add(name ?? "", value);
        }

        /// <summary> 添加参数
        /// </summary>
        /// <param name="preName">参数名前缀</param>
        /// <param name="value">实体参数</param>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public void Add(string preName, object value)
        {
            if (preName == null && value == null)
            {
                return;
            }
            var str = value as string;
            if (str != null)
            {
                Add(preName, str);
                return;
            }

            var nv = value as NameValueCollection;
            if (nv != null)
            {
                base.Add(nv);
                return;
            }

            var format = value as IFormattable;
            if (format != null)
            {
                Add(preName, format.ToString(null, null));
                return;
            }

            var convert = value as IConvertible;
            if (convert != null)
            {
                Add(preName, convert.ToString(null));
                return;
            }

            var dict = value as IDictionary;
            if (dict != null)
            {
                var item = dict.GetEnumerator();
                while (item.MoveNext())
                {
                    var key = item.Key as string;
                    if (key == null && item.Key != null)
                    {
                        throw new NotImplementedException("不支持的key类型");
                    }
                    Add(ConcatName(preName, key), item.Value);
                }
                return;
            }

            var list = value as IEnumerable;
            IEnumerator enumer;
            if (list != null)
            {
                enumer = list.GetEnumerator();
            }
            else
            {
                enumer = value as IEnumerator;
            }

            if (enumer != null)
            {
                while (enumer.MoveNext())
                {
                    Add(preName, enumer.Current);
                }
                return;
            }

            var ser = value as ISerializable;
            if (ser != null)
            {
                var info = new SerializationInfo(typeof(string), new FormatterConverter());
                ser.GetObjectData(info, default(StreamingContext));
                foreach (var item in info)
                {
                    Add(ConcatName(preName, item.Name), item.Value);
                }
                return;
            }

            if (value == null)
            {
                base.Add(preName, "");
                return;
            }

            var props = value.GetType().GetProperties();
            if (props.Length == 0)
            {
                Add(preName, Component.Converter.ToString(value));
                return;
            }

            foreach (var p in props)
            {
                Add(ConcatName(preName, p.Name), p.GetValue(value));
            }
        }

        /// <summary> 添加 参数, value将被序列化为json字符串作为参数
        /// </summary>
        /// <param name="value">实体参数</param>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public void Add(object value)
        {
            var str = value as string;
            if (str != null)
            {
                value = HttpUtility.ParseQueryString(str);
            }
            Add(null, value);
        }

        [ThreadStatic]
        private static StringBuilder _Buffer;

        /// <summary> 返回参数集的字符串标签形式
        /// </summary>
        /// <returns></returns>
        /// <remarks>周子鉴 2015.09.09</remarks>
        public override string ToString()
        {
            var length = Count;
            if (length == 0)
            {
                return string.Empty;
            }
            var buffer = _Buffer;
            if (buffer == null)
            {
                buffer = _Buffer = new StringBuilder(127);
            }
            else
            {
                buffer.Clear();
            }
            for (int i = 0; i < length; i++)
            {
                var key = GetKey(i);
                if (string.IsNullOrWhiteSpace(key) == false)
                {
                    buffer.Append(key);
                    buffer.Append('=');
                }
                var values = GetValues(i);
                if (values == null)
                {
                    buffer.Append('&');
                    continue;
                }
                AppendEscape(buffer, values[0]);
                for (int j = 1; j < values.Length; j++)
                {
                    buffer.Append(',');
                    AppendEscape(buffer, values[j]);
                }
                buffer.Append('&');
            }
            buffer.Length -= 1;
            return buffer.ToString();
        }


        static void AppendEscape(StringBuilder buffer, string str)
        {
            const int max = 32766;
            if (str == null)
            {
                buffer.Append(str);
                return;
            }
            var length = str.Length;
            if (length < max)
            {
                buffer.Append(Uri.EscapeDataString(str));
                return;
            }
            int i = 0;
            length -= max;
            for (; i < length; i += max)
            {
                var s = str.Substring(i, max);
                buffer.Append(Uri.EscapeDataString(s));
            }
            buffer.Append(Uri.EscapeDataString(str.Substring(i, length - i + max)));
        }

    }
}
