using System.Collections.Generic;

namespace blqw.Web
{

    /// <summary>
    /// 提供对请求动作的跟踪
    /// </summary>
    public sealed class HttpTracking : IHttpTracking
    {
        /// <summary>
        /// 发现请求中的 Body 参数时触发
        /// </summary>
        public event HttpParamFoundTrackingHandler<object> BodyParamFound;
        /// <summary>
        /// 请求完成时触发
        /// </summary>
        public event HttpResponseTrackingHandler End;
        /// <summary>
        /// 出现错误时触发
        /// </summary>
        public event HttpResponseTrackingHandler Error;
        /// <summary>
        /// 发现请求中的 Header 参数时触发
        /// </summary>
        public event HttpParamFoundTrackingHandler<IEnumerable<string>> HeaderFound;
        /// <summary>
        /// 请求初始化完成时触发
        /// </summary>
        public event HttpTrackingHandler Initialize;
        /// <summary>
        /// 处理参数完成时触发
        /// </summary>
        public event HttpTrackingHandler ParamsExtracted;
        /// <summary>
        /// 准备处理参数时触发
        /// </summary>
        public event HttpTrackingHandler ParamsExtracting;
        /// <summary>
        /// 发现请求中的 Path 参数时触发
        /// </summary>
        public event HttpParamFoundTrackingHandler<string> PathParamFound;
        /// <summary>
        /// 发现请求中的 Query 参数时触发
        /// </summary>
        public event HttpParamFoundTrackingHandler<object> QueryParamFound;
        /// <summary>
        /// 开始发送请求时触发
        /// </summary>
        public event HttpTrackingHandler Sending;

        void IHttpTracking.OnInitialize(IHttpRequest request)
        {
            Initialize?.Invoke(request);
        }

        void IHttpTracking.OnBodyParamFound(IHttpRequest request, ref string name, ref object value)
        {
            BodyParamFound?.Invoke(request, ref name, ref value);
        }

        void IHttpTracking.OnEnd(IHttpRequest request, IHttpResponse response)
        {
            End?.Invoke(request, response);
        }

        void IHttpTracking.OnError(IHttpRequest request, IHttpResponse response)
        {
            Error?.Invoke(request, response);
        }

        void IHttpTracking.OnHeaderFound(IHttpRequest request, ref string name, ref IEnumerable<string> values)
        {
            HeaderFound?.Invoke(request, ref name, ref values);
        }

        void IHttpTracking.OnParamsExtracted(IHttpRequest request)
        {
            ParamsExtracted?.Invoke(request);
        }

        void IHttpTracking.OnParamsExtracting(IHttpRequest request)
        {
            ParamsExtracting?.Invoke(request);
        }

        void IHttpTracking.OnPathParamFound(IHttpRequest request, ref string name, ref string value)
        {
            PathParamFound?.Invoke(request, ref name, ref value);
        }

        void IHttpTracking.OnQueryParamFound(IHttpRequest request, ref string name, ref object value)
        {
            QueryParamFound?.Invoke(request, ref name, ref value);
        }

        void IHttpTracking.OnSending(IHttpRequest request)
        {
            Sending?.Invoke(request);
        }
    }
}