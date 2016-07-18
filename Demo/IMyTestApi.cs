using blqw.Web;
using blqw.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public interface IMyTestApi
    {
        [HttpPost("Test/GetUser")]
        [Tracking(typeof(MyTracking))]
        string GetUser(Guid ak, string id);

        [HttpGet("OAuth/GetUser")]
        string GetUser1(Guid ak, string id);

    }

    public class MyTracking : IHttpTracking
    {
        public void OnBodyParamFound(IHttpRequest request, ref string name, ref object value)
        {
            if (name == "ak")
            {
                Trace.WriteLine($"ak: {value}");
            }
        }

        public void OnEnd(IHttpRequest request, IHttpResponse response)
        {

        }

        public void OnError(IHttpRequest request, IHttpResponse response)
        {

        }

        public void OnHeaderFound(IHttpRequest request, ref string name, ref string value)
        {

        }

        public void OnInitialize(IHttpRequest request)
        {

        }

        public void OnParamsExtracted(IHttpRequest request)
        {

        }

        public void OnParamsExtracting(IHttpRequest request)
        {

        }

        public void OnPathParamFound(IHttpRequest request, ref string name, ref string value)
        {

        }

        public void OnQueryParamFound(IHttpRequest request, ref string name, ref object value)
        {

        }

        public void OnSending(IHttpRequest request)
        {

        }
    }
}
