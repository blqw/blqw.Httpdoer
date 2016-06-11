using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Generator
{
    struct GeneratorParam
    {
        public GeneratorParam(ParameterInfo p)
        {
            VarName = p.Name;
            ParamType = p.ParameterType;

            var attr = p.GetCustomAttribute<HttpParamAttribute>();
            Location = attr?.Location ?? HttpParamLocation.Auto;
            ParamName = attr?.Name ?? VarName;
        }

        public string ParamName { get; private set; }
        public string VarName { get; private set; }
        public Type ParamType { get; private set; }
        public HttpParamLocation Location { get; private set; }
        
    }
}
