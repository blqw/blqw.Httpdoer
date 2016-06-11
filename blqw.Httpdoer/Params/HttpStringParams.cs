using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public sealed class HttpStringParams : HttpParamsBase<string>
    {
        internal HttpStringParams(IHttpParameterCollection @params, HttpParamLocation location)
            :base(@params)
        {
            _Location = location;
        }

        HttpParamLocation _Location;

        public override HttpParamLocation Location
        {
            get
            {
                return _Location;
            }
        }
        
    }
}
