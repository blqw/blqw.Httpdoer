using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Generator.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class PathAttribute : HttpParamAttribute
    {
        public PathAttribute()
        {
        }

        public PathAttribute(string paramName) : base(paramName)
        {
        }

        public override HttpParamLocation Location
        {
            get
            {
                return HttpParamLocation.Path;
            }
        }
    }
}
