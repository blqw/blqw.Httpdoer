using blqw.Serialization;
using blqw.Web.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// Httpdoer 客户端发生器
    /// </summary>
    public static class HttpGenerator
    {
        static ConcurrentDictionary<Type, ICloneable> _Cache = new ConcurrentDictionary<Type, ICloneable>();

        public static T Create<T>(string domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException(nameof(domain));
            }
            if (domain.Length <= 8 || domain[0] == ':')
            {
                domain = "http://" + domain;
            }
            else if (domain[3] != ':' && domain[4] != ':' && domain[5] != ':')
            {
                domain = "http://" + domain;
            }

            Uri uri;
            if (Uri.TryCreate(domain, UriKind.Absolute, out uri) == false)
            {
                throw new UriFormatException(nameof(domain) + " 不是一个有效的Url字符串");
            }

            var server = _Cache.GetOrAdd(typeof(T), GeneratorClass.GetObject);
            var request = (IHttpRequest)server.Clone();
            request.BaseUrl = uri;
            return (T)request;
        }
    }
}
