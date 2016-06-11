using blqw.Serialization;
using blqw.Web.Generator;
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
        static ConcurrentDictionary<Type, IConvertible> _Cache = new ConcurrentDictionary<Type, IConvertible>();

        public static T Create<T>(string domain)
        {
            throw new NotImplementedException();
        }
    }
}
