using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Generator
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class BodyAttribute : HttpParamAttribute
    {
        public BodyAttribute()
        {
        }

        public BodyAttribute(string paramName) : base(paramName)
        {
        }

        public override HttpParamLocation Location
        {
            get
            {
                return HttpParamLocation.Body;
            }
        }
    }
}
