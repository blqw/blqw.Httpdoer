using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Extensions
{
    public sealed class HttpDeleteAttribute : HttpVerbAttribute
    {
        public HttpDeleteAttribute(string template) : base(template)
        {
        }

        public override HttpRequestMethod Method
        {
            get
            {
                return HttpRequestMethod.Delete;
            }
        }
    }
}
