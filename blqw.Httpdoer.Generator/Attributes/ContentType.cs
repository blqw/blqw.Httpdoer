using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Generator
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public sealed class ContentTypeAttribute : Attribute
    {
        public HttpContentType ContentType { get; private set; }
        public ContentTypeAttribute(string value)
        {
            ContentType = value;
        }
        public ContentTypeAttribute(HttpContentTypes value)
        {
            ContentType = value;
        }
    }
}
