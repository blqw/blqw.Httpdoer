using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    internal class HttpParameterCollection : Dictionary<string, HttpParamValue>, IHttpParameterCollection
    {
        private string GetKey(string name, HttpParamLocation location)
        {
            if (name == null)
            {
                return $"{(int)location}";
            }
            return $"{(int)location}-{name}";
        }

        public object GetValue(string name, HttpParamLocation location)
        {
            HttpParamValue result;
            var key = GetKey(name, location);
            if (TryGetValue(key, out result))
            {
                return result.Value;
            }
            return null;
        }

        public void SetValue(HttpParamValue value)
        {
            var key = GetKey(value.Name, value.Location);
            if (value.Value == null)
            {
                Remove(key);
            }
            else
            {
                base[key] = value;
            }
        }

        public void SetValue(string name, object value, HttpParamLocation location)
        {
            var key = GetKey(name, location);
            if (value == null)
            {
                Remove(key);
            }
            else
            {
                base[key] = new HttpParamValue(name, value, location);
            }            
        }

        public bool Contains(string name, HttpParamLocation location)
        {
            var key = GetKey(name, location);
            return ContainsKey(key);
        }

        IEnumerator<HttpParamValue> IEnumerable<HttpParamValue>.GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        public void Remove(string name, HttpParamLocation location)
        {
            var key = GetKey(name, location);
            Remove(key);
        }

        public void AddValue(string name, object value, HttpParamLocation location)
        {
            var key = GetKey(name, location);
            if (value != null)
            {
                var list = base[key].Value as List<object>;
                if (list == null)
                {
                    base[key] = new HttpParamValue(name, value, location);
                }
                else
                {
                    list.Add(value);
                }
            }
        }

        public void AddValue(HttpParamValue value)
        {
            var key = GetKey(value.Name, value.Location);
            if (value.Value != null)
            {
                var list = base[key].Value as List<object>;
                if (list == null)
                {
                    base[key] = value;
                }
                else
                {
                    list.Add(value.Value);
                }
            }
        }
    }
}
