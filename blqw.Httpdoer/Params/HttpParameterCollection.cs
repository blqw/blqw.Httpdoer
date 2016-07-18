using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    internal class HttpParameterCollection : NameObjectCollectionBase, IHttpParameterCollection
    {
        private string GetKey(string name, HttpParamLocation location)
        {
            if (name == null)
            {
                return $"{(int)location}";
            }
            return $"{(int)location}-{name.ToLowerInvariant()}";
        }

        public HttpParamValue Get(string name, HttpParamLocation location)
        {
            var key = GetKey(name, location);
            var value = BaseGet(key);
            if (value == null)
            {
                return new HttpParamValue(this, name, null, location);
            }
            return (HttpParamValue)value;
        }

        public object GetValue(string name, HttpParamLocation location)
        {
            return Get(name, location).Value;
        }

        public IList GetValues(string name, HttpParamLocation location)
        {
            return Get(name, location).Values.AsReadOnly();
        }

        public void SetValue(string name, object value, HttpParamLocation location)
        {
            var key = GetKey(name, location);
            if (value == null)
            {
                BaseRemove(key);
            }
            else
            {
                base.BaseSet(key, new HttpParamValue(this, name, value, location));
            }
        }

        public bool Contains(string name, HttpParamLocation location)
        {
            var key = GetKey(name, location);
            return BaseGet(key) != null;
        }

        public IEnumerator<HttpParamValue> GetEnumerator()
        {
            return BaseGetAllValues().Cast<HttpParamValue>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return BaseGetAllValues().GetEnumerator();
        }

        public void Remove(string name, HttpParamLocation location)
        {
            var key = GetKey(name, location);
            BaseRemove(key);
        }

        public void AddValue(string name, object value, HttpParamLocation location)
        {
            if (value == null) return;

            var key = GetKey(name, location);
            var result = BaseGet(key);
            if (result == null)
            {
                BaseSet(key, new HttpParamValue(this, name, value, location));
                return;
            }
            var param = (HttpParamValue)result;

            var ee = (value as IEnumerable)?.GetEnumerator();
            if (ee != null && value is string == false)
            {
                var list = param.Values
                        ?? new List<object>();
                if (list.Count == 0 && param.Value != null)
                {
                    list.Add(param.Value);
                }
                while (ee.MoveNext())
                {
                    list.Add(ee.Current);
                }
                if (param.IsArray == false)
                {
                    BaseSet(key, new HttpParamValue(this, name, list, location));
                }
                return;
            }

            if (param.IsArray)
            {
                param.Add(value);
                return;
            }

            if (param.Value == null)
            {
                BaseSet(key, new HttpParamValue(this, name, value, location));
            }
            else
            {
                BaseSet(key, new HttpParamValue(this, name, new List<object>() { param.Value, value }, location));
            }
        }

    }
}
