using blqw.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    internal sealed class HttpQueryBuilder
    {
        StringBuilder _Buffer = new StringBuilder();

        public void Clear()
        {
            _Buffer?.Clear();
        }

        public override string ToString()
        {
            var query = _Buffer?.ToString();
            _Buffer?.Clear();
            return query;
        }

        public void AppendObject(string name, object obj)
        {
            if (_Buffer.Length > 0)
            {
                _Buffer.Append('&');
            }
            AppendObject(_Buffer, name, obj);
        }

        private static void AppendObject(StringBuilder buffer, string preName, object obj)
        {
            var str = obj as string
                ?? (obj as IFormattable)?.ToString(null, null)
                ?? (obj as IConvertible)?.ToString(null);
            if (str != null || obj == null)
            {
                if (preName != null)
                {
                    AppendEscape(buffer, preName);
                    buffer.Append('=');
                }
                AppendEscape(buffer, str);
                return;
            }

            var array = (obj as IEnumerable)?.GetEnumerator()
                        ?? obj as IEnumerator;
            if (array != null)
            {
                if (array.MoveNext())
                {
                    AppendObject(buffer, preName + "[]", array.Current);
                    while (array.MoveNext())
                    {
                        buffer.Append('&');
                        AppendObject(buffer, preName + "[]", array.Current);
                    }
                }
                return;
            }

            var props = PropertyGetter.Get(obj.GetType());
            var pCount = props.Count;
            if (pCount == 0)
            {
                if (preName != null)
                {
                    AppendEscape(buffer, preName);
                    buffer.Append('=');
                }
                AppendEscape(buffer, obj.ToString());
                return;
            }

            AppendObject(buffer, ConcatName(preName, props[0].Name), props[0].GetValue(obj));
            for (int i = 1; i < pCount; i++)
            {
                buffer.Append('&');
                AppendObject(buffer, ConcatName(preName, props[i].Name), props[i].GetValue(obj));
            }
        }

        private static void AppendEscape(StringBuilder buffer, string str)
        {
            const int max = 32766;
            if (str == null)
            {
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

        /// <summary> 
        /// 连接参数名,如果存在前缀的话 组成 `前缀.参数名` 的格式
        /// </summary>
        /// <param name="pre">参数名前缀</param>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        /// <remarks>周子鉴 2015.08.01</remarks>
        private static string ConcatName(string pre, string name)
        {
            return pre == null ? name : $"{pre}[{name}]";
        }
    }
}
