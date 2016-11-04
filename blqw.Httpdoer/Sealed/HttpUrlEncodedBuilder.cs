using System;
using System.Collections;
using System.Text;
using blqw.Reflection;

namespace blqw.Web
{
    /// <summary>
    /// 用于拼接 HTTP 请求中的 UrlEncoded 格式的参数
    /// </summary>
    internal sealed class HttpUrlEncodedBuilder
    {
        /// <summary>
        /// 字符串缓存
        /// </summary>
        private readonly StringBuilder _buffer = new StringBuilder();

        /// <summary>
        /// 清空缓存中的字符
        /// </summary>
        public void Clear(string pre) => _buffer?.Clear().Append(pre);

        /// <summary>
        /// 获取已拼接完成的所有字符,并清空缓存
        /// </summary>
        /// <returns> </returns>
        public override string ToString()
        {
            var query = _buffer?.ToString();
            _buffer?.Clear();
            return query ?? "";
        }

        /// <summary>
        /// 追加一个参数
        /// </summary>
        /// <param name="name"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        public void AppendObject(string name, object value)
        {
            if (_buffer.Length > 0)
            {
                _buffer.Append('&');
            }
            AppendObject(_buffer, name, value);
        }

        /// <summary>
        /// 将有一个参数写入<paramref name="buffer" />中
        /// </summary>
        /// <param name="buffer"> 字符缓存 </param>
        /// <param name="preName"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        private static void AppendObject(StringBuilder buffer, string preName, object value)
        {
            var str = value as string
                      ?? (value as IFormattable)?.ToString(null, null)
                      ?? (value as IConvertible)?.ToString(null);
            if ((str != null) || (value == null))
            {
                if (preName != null)
                {
                    AppendEscape(buffer, preName);
                    buffer.Append('=');
                }
                AppendEscape(buffer, str);
                return;
            }

            var array = (value as IEnumerable)?.GetEnumerator()
                        ?? value as IEnumerator;
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

            var props = PropertyHandlerCollection.Get(value.GetType());
            var pCount = props.Count;
            if (pCount == 0)
            {
                if (preName != null)
                {
                    AppendEscape(buffer, preName);
                    buffer.Append('=');
                }
                AppendEscape(buffer, value.ToString());
                return;
            }

            AppendObject(buffer, ConcatName(preName, props[0].Name), props[0].GetValue(value));
            for (var i = 1; i < pCount; i++)
            {
                buffer.Append('&');
                AppendObject(buffer, ConcatName(preName, props[i].Name), props[i].GetValue(value));
            }
        }

        /// <summary>
        /// 将字符串转义并写入<paramref name="buffer" />中
        /// </summary>
        /// <param name="buffer"> 字符缓存 </param>
        /// <param name="str"> 待写入的字符串 </param>
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
            var i = 0;
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
        /// <param name="pre"> 参数名前缀 </param>
        /// <param name="name"> 参数名 </param>
        /// <returns> </returns>
        /// <remarks> 周子鉴 2015.08.01 </remarks>
        private static string ConcatName(string pre, string name) => pre == null ? name : $"{pre}[{name}]"; //jquery采用该命名方式 $.param(myObject)
        //=> pre == null ? name : $"{pre}.{name}";
    }
}