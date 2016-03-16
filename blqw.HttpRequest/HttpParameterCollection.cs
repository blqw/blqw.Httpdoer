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
    public class HttpParameterCollection : IDictionary, IDictionary<string, object>
    {
        static readonly string NULL = Guid.NewGuid().ToString() + new object().GetHashCode();
        IDictionary _Dictionary;
        /// <summary> 初始化参数集
        /// </summary>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public HttpParameterCollection()
        {
            _Dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
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

        /// <summary> 添加参数
        /// </summary>
        /// <param name="name">参数名前缀</param>
        /// <param name="value">实体参数</param>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public void Add(string name, object value)
        {
            if (value == null)
            {
                _Dictionary.Remove(name ?? NULL);
                return;
            }
            AddImpl(name ?? NULL, value);
        }

        /// <summary> 添加 参数, value将被序列化为json字符串作为参数
        /// </summary>
        /// <param name="value">实体参数</param>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public void Add(object value)
        {
            if (value == null)
            {
                return;
            }
            var str = value as string;
            if (str != null)
            {
                value = HttpUtility.ParseQueryString(str);
            }

            var nv = value as NameValueCollection;
            if (nv != null)
            {
                for (int i = 0, length = nv.Count; i < length; i++)
                {
                    Add(nv.GetKey(i), nv[i]);
                }
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
                    Add(key, item.Value);
                }
                return;
            }

            var props = value.GetType().GetProperties();
            if (props.Length == 0)
            {
                AddImpl(NULL, value);
                return;
            }

            foreach (var p in props)
            {
                Add(p.Name, p.GetValue(value));
            }
        }

        private void AddImpl(string name, object value)
        {
            var v = _Dictionary[name];
            if (v == null)
            {
                _Dictionary.Add(name, value);
            }
            else
            {
                _Dictionary[name] = new LinkedValues
                {
                    Value = value,
                    Next = v,
                    Index = (v is LinkedValues) ? ((LinkedValues)v).Length() : 1
                };
            }
        }


        public void Set(object value)
        {
            _Dictionary.Remove(NULL);
            Add(value);
        }

        public void Clear()
        {
            _Dictionary.Clear();
        }

        public string this[string name]
        {
            get
            {
                var value = GetValue(_Dictionary[name ?? NULL]);

                var str = value as string
                    ?? (value as IFormattable)?.ToString(null, null)
                    ?? (value as IConvertible)?.ToString(null);
                if (str != null)
                {
                    return str;
                }

                var ee = (value as IEnumerable)?.GetEnumerator() ?? value as IEnumerator;
                if (ee != null && ee.MoveNext())
                {
                    var buffer = _Buffer;
                    if (buffer == null)
                    {
                        buffer = _Buffer = new StringBuilder(127);
                    }
                    else
                    {
                        buffer.Clear();
                    }
                    var cur = ee.Current;
                    buffer.Append(cur as string
                                ?? (cur as IFormattable)?.ToString(null, null)
                                ?? (cur as IConvertible)?.ToString(null)
                                ?? cur?.ToString()
                                );
                    while (ee.MoveNext())
                    {
                        buffer.Append(',');
                        cur = ee.Current;
                        buffer.Append(cur as string
                                    ?? (cur as IFormattable)?.ToString(null, null)
                                    ?? (cur as IConvertible)?.ToString(null)
                                    ?? cur?.ToString()
                                    );
                    }
                    return buffer.ToString();
                }
                return value?.ToString();
            }
            set
            {
                _Dictionary.Remove(name ?? NULL);
                Add(name, value);
            }
        }

        private static object GetValue(object obj)
        {
            if (obj is LinkedValues == false)
            {
                return obj;
            }
            var value = (LinkedValues)obj;
            var array = new object[value.Length()];

            while (true)
            {
                if (value.Value is string)
                {
                    array[value.Index] = value.Value;
                }
                else
                {
                    var ee = (value.Value as IEnumerable)?.GetEnumerator() ?? value.Value as IEnumerator;
                    if (ee == null)
                    {
                        array[value.Index] = value.Value;
                    }
                    else
                    {
                        var i = value.Index;
                        while (ee.MoveNext())
                        {
                            array[i] = ee.Current;
                            i++;
                        }
                    }
                }
                if (value.Next is LinkedValues == false)
                {
                    array[0] = value.Next;
                    break;
                }
                value = (LinkedValues)value.Next;
            }

            return array.ToArray();
        }

        public object Get(string name)
        {
            return GetValue(_Dictionary[name]);
        }

        struct LinkedValues
        {
            public object Value;
            public object Next;
            public int Index;
            public int Length()
            {
                if (Value is string)
                {
                    return 1 + Index;
                }
                var a = Value as ICollection;
                if (a != null)
                {
                    return a.Count + Index;
                }
                var count = 0;
                var b = Value as IEnumerable;
                if (b != null)
                {
                    var ee = b.GetEnumerator();
                    try
                    {
                        while (ee.MoveNext()) count++;
                    }
                    finally
                    {
                        (ee as IDisposable)?.Dispose();
                    }
                    return count + Index;
                }
                var c = Value as IEnumerator;
                if (c != null)
                {
                    try
                    {
                        while (c.MoveNext()) count++;
                    }
                    finally
                    {
                        (c as IDisposable)?.Dispose();
                    }
                    return count + Index;
                }
                return 1 + Index;
            }
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

            var v = Get(NULL);
            if (v != null)
            {
                AppendObject(buffer, null, v);
                buffer.Append('&');
            }

            var ee = _Dictionary.GetEnumerator();
            while (ee.MoveNext())
            {
                var key = (string)ee.Key;
                if (key == NULL)
                {
                    continue;
                }
                var value = GetValue(ee.Value);
                var array = value as Array;
                if (array == null)
                {
                    AppendObject(buffer, key, value);
                }
                else if (array.Length == 0)
                {
                    AppendObject(buffer, key, null);
                }
                else
                {
                    AppendObject(buffer, key, array.GetValue(0));
                    for (int j = 1; j < array.Length; j++)
                    {
                        AppendObject(buffer, key, array.GetValue(j));
                    }
                }
            }
            buffer.Length -= 1;
            return buffer.ToString();
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
                buffer.Append('&');
                return;
            }

            var props = obj.GetType().GetProperties();
            if (props.Length == 0)
            {
                if (preName != null)
                {
                    AppendEscape(buffer, preName);
                    buffer.Append('=');
                }
                AppendEscape(buffer, obj.ToString());
                buffer.Append('&');
                return;
            }

            foreach (var p in props)
            {
                AppendObject(buffer, ConcatName(preName, p.Name), p.GetValue(obj));
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

        //public IEnumerator GetEnumerator()
        //{
        //    return _Dictionary.GetEnumerator();
        //}


        public bool IsReadOnly { get; internal set; }
        public int Count
        {
            get
            {
                return _Dictionary.Count;
            }
        }


        #region 实现接口

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public ICollection<string> Keys
        {
            get
            {
                return ((IDictionary<string, object>)_Dictionary).Keys;
            }
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get
            {
                return ((IDictionary<string, object>)_Dictionary).Values;
            }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get
            {
                return IsReadOnly;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return _Dictionary.Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return _Dictionary.Values;
            }
        }




        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                var name = key as string;
                if (name == null && key != null)
                {
                    return null;
                }
                return Get(name ?? NULL);
            }
            set
            {
                var name = key as string;
                if (name == null && key != null)
                {
                    throw new FormatException($"参数 {nameof(key)} 只能是string类型");
                }
                _Dictionary.Remove(name ?? NULL);
                Add(name, value);
            }
        }

        object IDictionary<string, object>.this[string key]
        {
            get
            {
                var name = key as string;
                if (name == null && key != null)
                {
                    return null;
                }
                return Get(name);
            }
            set
            {
                var name = key as string;
                if (name == null && key != null)
                {
                    throw new FormatException($"参数 {nameof(key)} 只能是string类型");
                }
                _Dictionary.Remove(name ?? NULL);
                Add(name, value);
            }
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return _Dictionary.Contains(key);
        }


        public void Remove(string key)
        {
            _Dictionary.Remove(key);
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return ((IDictionary<string, object>)_Dictionary).TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return _Dictionary.Contains(item.Key);
        }


        bool IDictionary.Contains(object key)
        {
            return _Dictionary.Contains(key as string);
        }

        public void Add(object key, object value)
        {
            _Dictionary.Add(key, value);
        }

        public void Remove(object key)
        {
            _Dictionary.Remove(key);
        }

        public void CopyTo(Array array, int index)
        {
            _Dictionary.CopyTo(array, index);
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            return ((IDictionary<string, object>)_Dictionary).Remove(key);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return ((IDictionary<string, object>)_Dictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var item in (IDictionary<string, object>)_Dictionary)
            {
                yield return new KeyValuePair<string, object>(
                    item.Key,
                    GetValue(item.Value)
                    );
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator(((IDictionary<string, object>)_Dictionary).GetEnumerator());
        }

        struct DictionaryEnumerator : IDictionaryEnumerator
        {
            private IEnumerator<KeyValuePair<string, object>> enumerator;

            public DictionaryEnumerator(IEnumerator<KeyValuePair<string, object>> enumerator) : this()
            {
                this.enumerator = enumerator;
            }

            public object Current
            {
                get
                {
                    return new DictionaryEntry(enumerator.Current.Key, GetValue(enumerator.Current.Value));
                }
            }

            public DictionaryEntry Entry
            {
                get
                {
                    return new DictionaryEntry(enumerator.Current.Key, GetValue(enumerator.Current.Value));
                }
            }

            public object Key { get { return enumerator.Current.Key; } }

            public object Value { get { return GetValue(enumerator.Current.Value); } }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                enumerator.Reset();
            }
        }


        #endregion
    }
}
