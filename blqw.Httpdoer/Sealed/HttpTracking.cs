using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public sealed class HttpTracking : IHttpTracking
    {
        public HttpTrackingHandler OnInitialize;
        public HttpParamFoundTrackingHandler OnBodyParamFound;
        public HttpResponseTrackingHandler OnEnd;
        public HttpResponseTrackingHandler OnError;
        public HttpParamFoundTrackingHandler OnHeaderFound;
        public HttpTrackingHandler OnParamsExtracted;
        public HttpTrackingHandler OnParamsExtracting;
        public HttpParamFoundTrackingHandler OnPathParamFound;
        public HttpParamFoundTrackingHandler OnQueryParamFound;
        public HttpTrackingHandler OnSending;

        void IHttpTracking.OnInitialize(IHttpRequest request)
        {
            OnInitialize?.Invoke(request);
        }

        void IHttpTracking.OnBodyParamFound(IHttpRequest request, ref string name, ref object value)
        {
            OnBodyParamFound?.Invoke(request, ref name, ref value);
        }

        void IHttpTracking.OnEnd(IHttpRequest request, IHttpResponse response)
        {
            OnEnd?.Invoke(request, response);
        }

        void IHttpTracking.OnError(IHttpRequest request, IHttpResponse response)
        {
            OnError?.Invoke(request, response);
        }

        void IHttpTracking.OnHeaderFound(IHttpRequest request, ref string name, ref object value)
        {
            OnHeaderFound?.Invoke(request, ref name, ref value);
        }

        void IHttpTracking.OnParamsExtracted(IHttpRequest request)
        {
            OnParamsExtracted?.Invoke(request);
        }

        void IHttpTracking.OnParamsExtracting(IHttpRequest request)
        {
            OnParamsExtracting?.Invoke(request);
        }

        void IHttpTracking.OnPathParamFound(IHttpRequest request, ref string name, ref object value)
        {
            OnPathParamFound?.Invoke(request, ref name, ref value);
        }

        void IHttpTracking.OnQueryParamFound(IHttpRequest request, ref string name, ref object value)
        {
            OnQueryParamFound?.Invoke(request, ref name, ref value);
        }

        void IHttpTracking.OnSending(IHttpRequest request)
        {
            OnSending?.Invoke(request);
        }
    }
}
