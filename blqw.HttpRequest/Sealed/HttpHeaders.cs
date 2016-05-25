using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public class HttpHeaders : HttpParamsBase<string>
    {
        internal HttpHeaders(IHttpParameterCollection @params)
            :base(@params)
        {

        }

        public override HttpParamLocation Location
        {
            get
            {
                return HttpParamLocation.Header;
            }
        }
    }
}
