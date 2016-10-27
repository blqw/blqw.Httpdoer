using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using blqw.IOC;

namespace blqw.Web
{
    /// <summary>
    /// Json解析器,用于解析 Json 格式的正文
    /// </summary>
    internal sealed class HttpJsonBodyParser : HttpBodyParserBase
    {
        /// <summary>
        /// 将字节流转换为键值对枚举
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes, IFormatProvider formatProvider)
        {
            var charset = GetEncoding(formatProvider) ?? Encoding.UTF8;
            var json = charset.GetString(bytes);
            return (Dictionary<string, object>)ComponentServices.ToJsonObject(typeof(Dictionary<string, object>), json);
        }

        /// <summary>
        /// 匹配解析器,返回 true 表示匹配成功
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="format"> 格式 </param>
        /// <returns></returns>
        public override bool IsMatch(string type, string format)
            => format?.Length == 4 && format.EndsWith("json", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 将正文格式化为字节流
        /// </summary>
        /// <param name="format"> 包含格式规范的格式字符串 </param>
        /// <param name="body"> 请求或响应正文 </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body, IFormatProvider formatProvider)
        {
            string json;
            if (body == null)
            {
                json = "null";
            }
            else if (body.FirstOrDefault().Key == null && body.Count() == 1)
            {
                json = ComponentServices.ToJsonString(body.FirstOrDefault().Value);
            }
            else if (body.GetType().GetInterfaces().Any(t => t == typeof(IDictionary) || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDictionary<,>))))
            {
                json = ComponentServices.ToJsonString(body);
            }
            else
            {
                json = ComponentServices.ToJsonString(new DictionaryWrapper(body));
            }

            var charset = GetEncoding(formatProvider) ?? Encoding.UTF8;
            return charset.GetBytes(json);
        }

        class DictionaryWrapper : IDictionary<string,object>
        {
            private IEnumerable<KeyValuePair<string, object>> body;

            public DictionaryWrapper(IEnumerable<KeyValuePair<string, object>> body)
            {
                this.body = body;
            }

            public object this[string key]
            {
                get { return body.FirstOrDefault(it => Equals(it.Key, key)).Value; }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public int Count => body.Count();
            
            public bool IsReadOnly => true;
            
            public ICollection<string> Keys => body.Select(it => it.Key).ToList();
            
            public ICollection<object> Values => body.Select(it => it.Value).ToList();

            public void Add(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public void Add(string key, object value)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(KeyValuePair<string, object> item) => ContainsKey(item.Key);

            public bool ContainsKey(string key) => body.Any(it => Equals(it.Key, key));

            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => body.GetEnumerator();

            public bool Remove(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public bool Remove(string key)
            {
                throw new NotImplementedException();
            }

            public bool TryGetValue(string key, out object value) => null != (value = body.FirstOrDefault(it => Equals(it.Key, key)).Value);

            IEnumerator IEnumerable.GetEnumerator() => body.GetEnumerator();
        }

        /// <summary>
        /// 将字节流转换为指定对象
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public override T Deserialize<T>(byte[] bytes, IFormatProvider formatProvider)
        {
            var charset = GetEncoding(formatProvider) ?? Encoding.UTF8;
            var json = charset.GetString(bytes);
            return (T)ComponentServices.ToJsonObject(typeof(T), json);
        }
    }
}