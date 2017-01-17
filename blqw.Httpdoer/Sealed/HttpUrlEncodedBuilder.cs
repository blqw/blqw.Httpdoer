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
        /// 数组编码模式
        /// </summary>
        public ArrayEncodeMode ArrayEncodeMode { get; set; }

        /// <summary>
        /// 对象编码模式
        /// </summary>
        public ObjectEncodeMode ObjectEncodeMode { get; set; }

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

        private string TryToString(object value)
        {
            switch (value)
            {
                case string s:
                    return s;
                case bool b:
                    return b ? "true" : "false";
                case IFormattable f:
                    return f.ToString(null, null);
                case IConvertible c:
                    return c.ToString(null);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 将有一个参数写入<paramref name="buffer" />中
        /// </summary>
        /// <param name="buffer"> 字符缓存 </param>
        /// <param name="preName"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        private void AppendObject(StringBuilder buffer, string preName, object value)
        {
            var str = TryToString(value);
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

            var array = (value as IEnumerable)?.GetEnumerator() ?? value as IEnumerator;
            if (array != null)
            {
                switch (ArrayEncodeMode)
                {
                    case ArrayEncodeMode.Merge:
                        AppendEscape(buffer, preName);
                        buffer.Append('=');
                        if (array.MoveNext())
                        {
                            var v = array.Current;
                            AppendEscape(buffer, TryToString(v) ?? IOC.ComponentServices.ToJsonString(v));
                            while (array.MoveNext())
                            {
                                v = array.Current;
                                buffer.Append(',');
                                AppendEscape(buffer, TryToString(v) ?? IOC.ComponentServices.ToJsonString(v));
                            }
                        }
                        break;
                    case ArrayEncodeMode.JQuery:
                        if (array.MoveNext())
                        {
                            AppendObject(buffer, preName + "[]", array.Current);
                            while (array.MoveNext())
                            {
                                buffer.Append('&');
                                AppendObject(buffer, preName + "[]", array.Current);
                            }
                        }
                        break;
                    case ArrayEncodeMode.Json:
                        AppendObject(buffer, preName, IOC.ComponentServices.ToJsonString(array));
                        break;
                    case ArrayEncodeMode.Asp:
                        if (array.MoveNext())
                        {
                            var i = 0;
                            AppendObject(buffer, $"{preName}[{i}]", array.Current);
                            while (array.MoveNext())
                            {
                                i++;
                                buffer.Append('&');
                                AppendObject(buffer, $"{preName}[{i}]", array.Current);
                            }
                        }
                        break;
                    case ArrayEncodeMode.Default:
                    default:
                        if (array.MoveNext())
                        {
                            AppendObject(buffer, preName, array.Current);
                            while (array.MoveNext())
                            {
                                buffer.Append('&');
                                AppendObject(buffer, preName, array.Current);
                            }
                        }
                        break;
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

            switch (ObjectEncodeMode)
            {
                case ObjectEncodeMode.NameOnly:
                    {
                        AppendObject(buffer, props[0].Name, props[0].GetValue(value));
                        for (var i = 1; i < pCount; i++)
                        {
                            buffer.Append('&');
                            AppendObject(buffer, props[i].Name, props[i].GetValue(value));
                        }
                    }
                    break;
                case ObjectEncodeMode.JQuery:
                    {
                        AppendObject(buffer, ConcatName(preName, props[0].Name, 1), props[0].GetValue(value));
                        for (var i = 1; i < pCount; i++)
                        {
                            buffer.Append('&');
                            AppendObject(buffer, ConcatName(preName, props[i].Name, 1), props[i].GetValue(value));
                        }
                    }
                    break;
                case ObjectEncodeMode.Json:
                    AppendObject(buffer, preName, IOC.ComponentServices.ToJsonString(value));
                    break;
                case ObjectEncodeMode.Default:
                default:
                    {
                        AppendObject(buffer, ConcatName(preName, props[0].Name, 0), props[0].GetValue(value));
                        for (var i = 1; i < pCount; i++)
                        {
                            buffer.Append('&');
                            AppendObject(buffer, ConcatName(preName, props[i].Name, 0), props[i].GetValue(value));
                        }
                    }
                    break;
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
        /// 连接参数名
        /// </summary>
        /// <param name="pre"> 参数名前缀 </param>
        /// <param name="name"> 参数名 </param>
        /// <returns> </returns>
        /// <remarks> 周子鉴 2015.08.01 </remarks>
        private static string ConcatName(string pre, string name, int style) => pre == null ? name : (style == 1 ? $"{pre}[{name}]" : $"{pre}.{name}");


    }
}