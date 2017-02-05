using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Extensions
{
    struct GeneratorParam
    {
        public GeneratorParam(ParameterInfo p)
        {
            VarName = p.Name;
            ParamType = p.ParameterType;
            
            var attr = p.GetCustomAttribute<HttpParamAttribute>();
            if (attr == null)
            {
                Location = HttpParamLocation.Auto;
                ParamName = VarName;
                Format = null;
            }
            else
            {
                Location = attr.Location;
                Format = attr.Format;
                ParamName = attr.Name ?? (attr.NameIsNull ? null : VarName);
            }
        }

        public string ParamName { get; }
        public string VarName { get;  }
        public Type ParamType { get;  }
        public HttpParamLocation Location { get; }
        public string Format { get;}
    }
}
