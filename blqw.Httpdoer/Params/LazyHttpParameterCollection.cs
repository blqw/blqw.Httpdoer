using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    class LazyHttpParameterCollection : HttpParameterCollection, IHttpParameterCollection
    {
        public LazyHttpParameterCollection(HttpContentType contentType, byte[] body, HttpParamLocation location)
        {
            _Initialization = () =>
            {
                var parser = contentType.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
                if (parser == null)
                {
                    return;
                }
                var res = new HttpResponse();
                var @params = parser.Deserialize(body, contentType);
                foreach (var item in parser.Deserialize(body, contentType))
                {
                    SetValue(item.Key, item.Value, location);
                }
                _Initialization = null;
            };
        }

        Action _Initialization;

        void IHttpParameterCollection.AddValue(HttpParamValue value)
        {
            _Initialization?.Invoke();
            AddValue(value);
        }

        void IHttpParameterCollection.AddValue(string name, object value, HttpParamLocation location)
        {
            _Initialization?.Invoke();
            AddValue(name, value, location);
        }

        bool IHttpParameterCollection.Contains(string name, HttpParamLocation location)
        {
            _Initialization?.Invoke();
            return Contains(name, location);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            _Initialization?.Invoke();
            return GetEnumerator();
        }

        IEnumerator<HttpParamValue> IEnumerable<HttpParamValue>.GetEnumerator()
        {
            _Initialization?.Invoke();
            return Values.GetEnumerator();
        }

        object IHttpParameterCollection.GetValue(string name, HttpParamLocation location)
        {
            _Initialization?.Invoke();
            return GetValue(name, location);
        }

        void IHttpParameterCollection.Remove(string name, HttpParamLocation location)
        {
            _Initialization?.Invoke();
            Remove(name, location);
        }

        void IHttpParameterCollection.SetValue(HttpParamValue value)
        {
            _Initialization?.Invoke();
            SetValue(value);
        }

        void IHttpParameterCollection.SetValue(string name, object value, HttpParamLocation location)
        {
            _Initialization?.Invoke();
            SetValue(name, value, location);
        }
    }
}
