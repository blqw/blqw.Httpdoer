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
    public sealed class Httpdoer : HttpRequest
    {
        static Httpdoer()
        {
            ServicePointManager.MaxServicePointIdleTime = 30000;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.SetTcpKeepAlive(true, 30000, 30000);
        }

        #region 同步
        static readonly IHttpClient Sync = new HttpClientSync();

        public IHttpResponse Send()
        {
            return Sync.Send(this);
        }

        public string GetString()
        {
            var res = Sync.Send(this);
            var str = res.Body?.ToString();
            this.Logger?.Debug(str);
            return str;
        }

        public byte[] GetBytes()
        {
            var res = Sync.Send(this);
            return res.Body?.ResponseBody;
        }

        public T GetObject<T>()
        {
            var res = Sync.Send(this);
            if (res.Body == null)
            {
                return default(T);
            }
            return res.Body.ToObject<T>();
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
            var str = res.Body?.ToString();
            request.Logger?.Debug(str);
            return str;
        }

        public static async Task<byte[]> GetBytesAsync(this IHttpRequest request)
        {
            var res = await Async.SendAsync(request, CancellationToken.None);
            return res.Body?.ResponseBody;
        }

        public static async Task<T> GetObjectAsync<T>(this IHttpRequest request)
        {
            var res = await Async.SendAsync(request, CancellationToken.None);
            if (res.Body == null)
            {
                return default(T);
            }
            return res.Body.ToObject<T>();
        }

        public static async Task<string> GetStringAsync(this IHttpRequest request, TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                var res = await Async.SendAsync(request, tokenSource.Token);
                var str = res.Body?.ToString();
                request.Logger?.Debug(str);
                return str;
            }
        }

        public static async Task<byte[]> GetBytesAsync(this IHttpRequest request, TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                var res = await Async.SendAsync(request, tokenSource.Token);
                return res.Body?.ResponseBody;
            }
        }

        public static async Task<T> GetObjectAsync<T>(this IHttpRequest request, TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                var res = await Async.SendAsync(request, tokenSource.Token);
                if (res.Body == null)
                {
                    return default(T);
                }
                return res.Body.ToObject<T>();
            }
        }

        public static async Task<string> GetStringAsync(this IHttpRequest request, CancellationToken cancellationToken)
        {
            var res = await Async.SendAsync(request, cancellationToken);
            var str = res.Body?.ToString();
            request.Logger?.Debug(str);
            return str;
        }

        public static async Task<byte[]> GetBytesAsync(this IHttpRequest request, CancellationToken cancellationToken)
        {
            var res = await Async.SendAsync(request, cancellationToken);
            return res.Body?.ResponseBody;
        }

        public static async Task<T> GetObjectAsync<T>(this IHttpRequest request, CancellationToken cancellationToken)
        {
            var res = await Async.SendAsync(request, cancellationToken);
            if (res.Body == null)
            {
                return default(T);
            }
            return res.Body.ToObject<T>();
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
