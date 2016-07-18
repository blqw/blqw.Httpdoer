using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public abstract class HttpParamsBase<T> : IEnumerable<KeyValuePair<string, object>>
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

        public virtual T this[string name]
        {
            get
            {
                return (T)Params.GetValue(name, Location);
            }
            set
            {
                Params.SetValue(name, value, Location);
            }
        }

        public void Set(string name, T value)
        {
            Params.SetValue(name, value, Location);
        }

        public void Add(string name, T value)
        {
            Params.AddValue(name, value, Location);
        }

        public T Get(string name)
        {
            return (T)Params.GetValue(name, Location);
        }

        public IReadOnlyList<T> GetValues(string name)
        {
            var values = Params.GetValues(name, Location);
            if (values == null) return null;
            return values as IReadOnlyList<T>
                ?? new CastList(values);
        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var item in Params)
            {
                if (item.Location == Location)
                {
                    yield return new KeyValuePair<string, object>(item.Name, item.Values ?? item.Value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            var keys = Params.Where(it => it.Location == Location).Select(it => it.Name).ToArray();
            for (int i = 0, length = keys.Length; i < length; i++)
            {
                Params.Remove(keys[i], Location);
            }
        }
        

        class CastList : IReadOnlyList<T>
        {
            public CastList(IList list)
            {
                _list = list;
            }
            IList _list;
            public T this[int index]
            {
                get
                {
                    if (index < 0 || index > _list.Count)
                    {
                        return default(T);
                    }
                    return (T)_list[index];
                }
            }

            public int Count
            {
                get
                {
                    return _list.Count;
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                for (int i = 0, length = _list.Count; i < length; i++)
                {
                    yield return (T)_list[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _list.GetEnumerator();
            }
        }
    }
}
