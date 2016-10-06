using System.Collections.Generic;

namespace blqw.Web
{
    /// <summary>
    /// 提供对请求动作的跟踪
    /// </summary>
    public interface IHttpTracking
    {
        /// <summary>
        /// 准备抽取参数
        /// </summary>
        /// <param name="request"> 触发事件的请求 </param>
        void OnParamsExtracting(IHttpRequest request);

        /// <summary>
        /// 抽取参数结束
        /// </summary>
        /// <param name="request"> 触发事件的请求 </param>
        void OnParamsExtracted(IHttpRequest request);

        /// <summary>
        /// 发现Query参数
        /// </summary>
        /// <param name="request"> 触发事件的请求 </param>
        /// <param name="name"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        void OnQueryParamFound(IHttpRequest request, ref string name, ref object value);

        /// <summary>
        /// 发现Body参数
        /// </summary>
        /// <param name="request"> 触发事件的请求 </param>
        /// <param name="name"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        void OnBodyParamFound(IHttpRequest request, ref string name, ref object value);

        /// <summary>
        /// 发现Header参数
        /// </summary>
        /// <param name="request"> 触发事件的请求 </param>
        /// <param name="name"> 参数名 </param>
        /// <param name="values"> 参数值 </param>
        void OnHeaderFound(IHttpRequest request, ref string name, ref IEnumerable<string> values);

        /// <summary>
        /// 发现Path参数
        /// </summary>
        /// <param name="request"> 触发事件的请求 </param>
        /// <param name="name"> 参数名 </param>
        /// <param name="value"> 参数值 </param>
        void OnPathParamFound(IHttpRequest request, ref string name, ref string value);

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="request"> 触发事件的请求 </param>
        void OnInitialize(IHttpRequest request);

        /// <summary>
        /// 发生错误
        /// </summary>
        /// <param name="request"> 触发事件的请求 </param>
        /// <param name="response"> 发生错误的响应 </param>
        void OnError(IHttpRequest request, IHttpResponse response);

        /// <summary>
        /// 正在发送请求
        /// </summary>
        /// <param name="request"> 触发事件的请求 </param>
        void OnSending(IHttpRequest request);

        /// <summary>
        /// 请求结束
        /// </summary>
        /// <param name="request"> 触发事件的请求 </param>
        /// <param name="response"> 请求对应的响应 </param>
        void OnEnd(IHttpRequest request, IHttpResponse response);
    }
}