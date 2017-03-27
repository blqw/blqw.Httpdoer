using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Extensions
{
    public abstract class HttpParamAttribute : Attribute
    {
        public string Name { get; }
        public bool NameIsNull { get; }
        public string Format { get; set; }
        public HttpParamAttribute(string name)
        {
            Name = name;
            if (name == null)
            {
                NameIsNull = true;
            }
        }

        public HttpParamAttribute()
        {

        }

        public abstract HttpParamLocation Location { get; }
    }
}
