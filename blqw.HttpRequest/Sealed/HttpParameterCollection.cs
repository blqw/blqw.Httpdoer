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
        public object GetValue(string name, HttpParamLocation location)
        {
            HttpParamValue result;
            var key = $"{(int)location}\n{name}";
            if (TryGetValue(key, out result))
            {
                return result;
            }
            return null;
        }

        public void SetValue(HttpParamValue value)
        {
            var key = $"{(int)value.Location}\n{value.Name}";
            base[key] = value;
        }

        public void SetValue(string name, object value, HttpParamLocation location)
        {
            var key = $"{(int)location}\n{name}";
            base[key] = new HttpParamValue(name, value, location);
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
