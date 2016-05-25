using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public abstract class HttpParamsBase<T> : IEnumerable<KeyValuePair<string, T>>
    {
        protected HttpParamsBase(IHttpParameterCollection @params)
        {
            if (@params == null)
            {
                throw new ArgumentNullException(nameof(@params));
            }
            Params = @params;
        }

        public abstract HttpParamLocation Location { get; }

        protected readonly IHttpParameterCollection Params;

        public T this[string name]
        {
            get
            {
                return (T)Params.GetValue(name, HttpParamLocation.Body);
            }
            set
            {
                Params.SetValue(name, value, HttpParamLocation.Body);
            }
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            foreach (var item in Params)
            {
                if (item.Location == Location)
                {
                    yield return new KeyValuePair<string, T>(item.Name, (T)item.Value);
                }
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
