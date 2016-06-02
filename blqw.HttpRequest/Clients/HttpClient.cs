using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Web
{
    public static class HttpClient
    {

        static HttpClient()
        {
            ServicePointManager.MaxServicePointIdleTime = 30000;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.SetTcpKeepAlive(true, 30000, 30000);
        }


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

        #region 异步
        static readonly IHttpClient Async = new HttpClientAsync();

        public static Task<IHttpResponse> SendAsync(this IHttpRequest request)
        {
            return SendAsync(request, CancellationToken.None);
        }
        public static Task<IHttpResponse> SendAsync(this IHttpRequest request, TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                return SendAsync(request, tokenSource.Token);
            }
        }
        public static Task<IHttpResponse> SendAsync(this IHttpRequest request, CancellationToken cancellationToken)
        {
            return Async.SendAsync(request, cancellationToken);
        }


        public static async Task<string> GetStringAsync(this IHttpRequest request)
        {
            var res = await Async.SendAsync(request, CancellationToken.None);
            var str = res.Body.ToString();
            Trace.WriteLine(str, "HttpRequest.Result");
            return str;
        }

        public static async Task<byte[]> GetBytesAsync(this IHttpRequest request)
        {
            var res = await Async.SendAsync(request, CancellationToken.None);
            return res.Body.ResponseBody;
        }

        public static async Task<string> GetStringAsync(this IHttpRequest request, TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                var res = await Async.SendAsync(request, tokenSource.Token);
                var str = res.Body.ToString();
                Trace.WriteLine(str, "HttpRequest.Result");
                return str;
            }
        }

        public static async Task<byte[]> GetBytesAsync(this IHttpRequest request, TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                var res = await Async.SendAsync(request, tokenSource.Token);
                return res.Body.ResponseBody;
            }
        }

        public static async Task<string> GetStringAsync(this IHttpRequest request, CancellationToken cancellationToken)
        {
            var res = await Async.SendAsync(request, cancellationToken);
            var str = res.Body.ToString();
            Trace.WriteLine(str, "HttpRequest.Result");
            return str;
        }

        public static async Task<byte[]> GetBytesAsync(this IHttpRequest request, CancellationToken cancellationToken)
        {
            var res = await Async.SendAsync(request, cancellationToken);
            return res.Body.ResponseBody;
        }
        #endregion

        #region Begin...End

        public static IAsyncResult BeginSend(this IHttpRequest request, AsyncCallback callback, object state)
        {
            return Sync.BeginSend(request, callback, state);
        }

        public static IHttpResponse EndSend(this IHttpRequest request, IAsyncResult asyncResult)
        {
            return Sync.EndSend(asyncResult);
        }

        #endregion
    }
}
