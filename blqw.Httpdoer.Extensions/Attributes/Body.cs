using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class BodyAttribute : HttpParamAttribute
    {
        public BodyAttribute() : base(null)
        {
        }

        public BodyAttribute(string paramName) : base(paramName)
        {
        }

        public override HttpParamLocation Location => HttpParamLocation.Body;
    }
}
