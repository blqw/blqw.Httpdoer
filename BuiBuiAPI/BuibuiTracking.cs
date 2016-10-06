using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using blqw.Web;

namespace BuiBuiAPI
{
    class BuibuiTracking : IHttpTracking
    {
        public void OnBodyParamFound(IHttpRequest request, ref string name, ref object value)
        {
            request.Debug($"Body -> {name}:{value}");
        }

        public void OnEnd(IHttpRequest request, IHttpResponse response)
        {
            request.Debug($"请求完成");
        }

        public void OnError(IHttpRequest request, IHttpResponse response)
        {
            request.Error($"{response?.Exception?.ToString()}");
        }

        public void OnHeaderFound(IHttpRequest request, ref string name,ref IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                request.Debug($"Header -> {name}:{value}");
            }
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
            request.Debug($"PathParam -> {name}:{value}");
        }

        public void OnQueryParamFound(IHttpRequest request, ref string name, ref object value)
        {
            request.Debug($"Query -> {name}:{value}");
        }

        public void OnSending(IHttpRequest request)
        {
        }
    }
}
