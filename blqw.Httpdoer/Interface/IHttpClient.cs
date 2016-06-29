using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 用于 HTTP 请求的发送和接收
    /// </summary>
    public interface IHttpClient
    {
        IHttpResponse Send(IHttpRequest request);

        Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken);

        IAsyncResult BeginSend(IHttpRequest request, AsyncCallback callback, object state);

        IHttpResponse EndSend(IAsyncResult asyncResult);
    }
}
