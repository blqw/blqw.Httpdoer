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
            base[key] = value;
        }

        public void SetValue(string name, object value, HttpParamLocation location)
        {
            var key = GetKey(name, location);
            base[key] = new HttpParamValue(name, value, location);
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
        




    }
}
