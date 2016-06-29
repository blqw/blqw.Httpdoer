using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public abstract class HttpVerbAttribute : Attribute
    {
        public string Template { get; private set; }
        public abstract HttpRequestMethod Method { get; }
        public HttpVerbAttribute(string template)
        {
            Template = template;
        }
    }
}
