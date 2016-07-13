using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 提供对请求动作的跟踪和切面
    /// </summary>
    public interface IHttpTracking
    {
        /// <summary>
        /// 准备抽取参数
        /// </summary>
        /// <param name="request"></param>
        void OnParamsExtracting(IHttpRequest request);
        /// <summary>
        /// 抽取参数结束
        /// </summary>
        /// <param name="request"></param>
        void OnParamsExtracted(IHttpRequest request);
        /// <summary>
        /// 发现Query参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void OnQueryParamFound(IHttpRequest request,ref string name, ref object value);
        /// <summary>
        /// 发现Body参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void OnBodyParamFound(IHttpRequest request, ref string name, ref object value);
        /// <summary>
        /// 发现Header参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void OnHeaderFound(IHttpRequest request, ref string name, ref string value);
        /// <summary>
        /// 发现Path参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void OnPathParamFound(IHttpRequest request, ref string name, ref string value);
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="request"></param>
        void OnInitialize(IHttpRequest request);
        /// <summary>
        /// 发生错误
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void OnError(IHttpRequest request, IHttpResponse response);
        /// <summary>
        /// 正在发送请求
        /// </summary>
        void OnSending(IHttpRequest request);
        /// <summary>
        /// 请求结束
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void OnEnd(IHttpRequest request, IHttpResponse response);
    }
}
