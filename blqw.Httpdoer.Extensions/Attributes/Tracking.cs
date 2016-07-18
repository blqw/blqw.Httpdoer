using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Extensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class TrackingAttribute : Attribute
    {
        public TrackingAttribute(Type type)
        {
            if (typeof(IHttpTracking).IsAssignableFrom(type))
            {
                Type = type;
            }
        }

        public Type Type { get; private set; }
    }
}
