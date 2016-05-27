using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Web
{
    public static class HttpClient
    {
        #region 同步
        static readonly IHttpClient Sync = new HttpClientSync();

        public static IHttpResponse Send(this IHttpRequest request)
        {
            return Sync.Send(request);
        }

        public static string GetString(this IHttpRequest request)
        {
            var res = Sync.Send(request);
            var str = res.Body.ToString();
            Trace.WriteLine(str, "HttpRequest.Result");
            return str;
        }

        public static byte[] GetBytes(this IHttpRequest request)
        {
            var res = Sync.Send(request);
            return res.Body.ResponseBody;
        }

        #endregion



        public static IAsyncResult BeginSend(this IHttpRequest request, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }
        public static IHttpResponse EndSend(this IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }
        public static Task<IHttpResponse> SendAsync(this IHttpRequest request)
        {
            throw new NotImplementedException();
        }
        public static Task<IHttpResponse> SendAsync(this IHttpRequest request, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }
        public static Task<IHttpResponse> SendAsync(this IHttpRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
