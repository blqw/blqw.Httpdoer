using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public sealed class HttpParams : HttpParamsBase<object>
    {
        internal HttpParams(IHttpParameterCollection @params)
            :base(@params)
        {

        }
        public override HttpParamLocation Location
        {
            get
            {
                return HttpParamLocation.Auto;
            }
        }
    }
}
