using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class QueryAttribute: HttpParamAttribute
    {
        public QueryAttribute()
        {

        }

        public QueryAttribute(string paramName) : base(paramName)
        {
        }

        public override HttpParamLocation Location
        {
            get
            {
                return HttpParamLocation.Query;
            }
        }
    }
}
