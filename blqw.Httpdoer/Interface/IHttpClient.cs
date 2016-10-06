using System;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 用于 HTTP 请求的发送和接收
    /// </summary>
    public interface IHttpClient
    {
        /// <summary>
        /// 同步发送 HTTP 请求
        /// </summary>
        /// <param name="request"> 请求对象 </param>
        /// <returns> </returns>
        IHttpResponse Send(IHttpRequest request);

        /// <summary>
        /// 发送 HTTP 请求,并返回可等待的任务,用于异步获取响应
        /// </summary>
        /// <param name="request"> 请求对象 </param>
        /// <param name="cancellationToken"> 用于取消请求的通知对象 </param>
        /// <returns> </returns>
        Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// 发送 HTTP 请求,并设置回调方法
        /// </summary>
        /// <param name="request"> 请求对象 </param>
        /// <param name="callback"> 异步回调 </param>
        /// <param name="state"> 回调参数 </param>
        /// <returns> </returns>
        IAsyncResult BeginSend(IHttpRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// 完成HTTP请求,并获取响应
        /// </summary>
        /// <param name="asyncResult"> 异步操作对象 </param>
        /// <returns> </returns>
        IHttpResponse EndSend(IAsyncResult asyncResult);
    }
}