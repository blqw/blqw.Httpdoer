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

        public override string this[string name]
        {
            get
            {
                var r = Params.Get(name, Location);
                if (r.IsArray)
                {
                    return string.Join(",", r.Values);
                }
                return (string)r.Value;
            }
            set
            {
                base[name] = value;
            }
        }
    }
}
