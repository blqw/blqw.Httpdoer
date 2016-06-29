using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Extensions
{
    public abstract class HttpParamAttribute : Attribute
    {
        public string Name { get; private set; }

        public HttpParamAttribute(string name)
        {
            Name = name;
        }

        public HttpParamAttribute()
        {

        }

        public abstract HttpParamLocation Location { get; }
    }
}
