using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    class LazyHttpParameterCollection : HttpParameterCollection, IHttpParameterCollection
    {
        private readonly HttpContentType _contentType;
        private readonly byte[] _body;
        private readonly HttpParamLocation _location;
        private bool _isInitialized;
        public LazyHttpParameterCollection(HttpContentType contentType, byte[] body, HttpParamLocation location)
        {
            _contentType = contentType;
            _body = body;
            _location = location;
            _isInitialized = false;
        }

        private void Initialize()
        {
            var parser = _contentType.GetFormat(typeof(IHttpBodyParser)) as IHttpBodyParser;
            var @params = parser?.Deserialize(_body, _contentType);
            if (@params != null)
            {
                foreach (var item in @params)
                {
                    SetValue(item.Key, item.Value, _location);
                }
            }
            _isInitialized = true;
        }


        void IHttpParameterCollection.AddValue(string name, object value, HttpParamLocation location)
        {
            if (location == _location)
            {
                throw new NotSupportedException("集合为只读");
            }
            if (!_isInitialized) Initialize();
            AddValue(name, value, location);
        }

        bool IHttpParameterCollection.Contains(string name, HttpParamLocation location)
        {
            if (!_isInitialized) Initialize();
            return Contains(name, location);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (!_isInitialized) Initialize();
            return GetEnumerator();
        }

        IEnumerator<HttpParamValue> IEnumerable<HttpParamValue>.GetEnumerator()
        {
            if (!_isInitialized) Initialize();
            return GetEnumerator();
        }

        object IHttpParameterCollection.GetValue(string name, HttpParamLocation location)
        {
            if (!_isInitialized) Initialize();
            return GetValue(name, location);
        }

        IList IHttpParameterCollection.GetValues(string name, HttpParamLocation location)
        {
            if (!_isInitialized) Initialize();
            return Get(name, location).Values.AsReadOnly();
        }

        void IHttpParameterCollection.Remove(string name, HttpParamLocation location)
        {
            if (location == _location)
            {
                throw new NotSupportedException("集合为只读");
            }
            if (!_isInitialized) Initialize();
            Remove(name, location);
        }

        void IHttpParameterCollection.SetValue(string name, object value, HttpParamLocation location)
        {
            if (location == _location)
            {
                throw new NotSupportedException("集合为只读");
            }
            if (!_isInitialized) Initialize();
            SetValue(name, value, location);
        }
    }
}
