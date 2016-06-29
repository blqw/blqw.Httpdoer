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
    public class Httpdoer : HttpRequest
    {
        static Httpdoer()
        {
            ServicePointManager.MaxServicePointIdleTime = 30000;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.SetTcpKeepAlive(true, 30000, 30000);
        }

        public Httpdoer(string baseUrl) : base(baseUrl)
        {
        }

        public Httpdoer()
        {
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
            Logger?.Debug(str);
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

        public Task<IHttpResponse> SendAsync()
        {
            return SendAsync(CancellationToken.None);
        }

        public Task<IHttpResponse> SendAsync(TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                return SendAsync(tokenSource.Token);
            }
        }
        public Task<IHttpResponse> SendAsync(CancellationToken cancellationToken)
        {
            return Async.SendAsync(this, cancellationToken);
        }


        public async Task<string> GetStringAsync()
        {
            var res = await Async.SendAsync(this, CancellationToken.None);
            var str = res.Body?.ToString();
            Logger?.Debug(str);
            return str;
        }

        public async Task<byte[]> GetBytesAsync()
        {
            var res = await Async.SendAsync(this, CancellationToken.None);
            return res.Body.ResponseBody;
        }

        public async Task<T> GetObjectAsync<T>()
        {
            var res = await Async.SendAsync(this, CancellationToken.None);
            if (res.Body == null)
            {
                return default(T);
            }
            return res.Body.ToObject<T>();
        }

        public async Task<string> GetStringAsync(TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                var res = await Async.SendAsync(this, tokenSource.Token);
                var str = res.Body?.ToString();
                Logger?.Debug(str);
                return str;
            }
        }

        public async Task<byte[]> GetBytesAsync(TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                var res = await Async.SendAsync(this, tokenSource.Token);
                return res.Body?.ResponseBody;
            }
        }

        public async Task<T> GetObjectAsync<T>(TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                var res = await Async.SendAsync(this, tokenSource.Token);
                if (res.Body == null)
                {
                    return default(T);
                }
                return res.Body.ToObject<T>();
            }
        }

        public async Task<string> GetStringAsync(CancellationToken cancellationToken)
        {
            var res = await Async.SendAsync(this, cancellationToken);
            var str = res.Body?.ToString();
            Logger?.Debug(str);
            return str;
        }

        public async Task<byte[]> GetBytesAsync(CancellationToken cancellationToken)
        {
            var res = await Async.SendAsync(this, cancellationToken);
            return res.Body?.ResponseBody;
        }

        public async Task<T> GetObjectAsync<T>(CancellationToken cancellationToken)
        {
            var res = await Async.SendAsync(this, cancellationToken);
            if (res.Body == null)
            {
                return default(T);
            }
            return res.Body.ToObject<T>();
        }
        #endregion

        #region Begin...End

        public IAsyncResult BeginSend(AsyncCallback callback, object state)
        {
            return Sync.BeginSend(this, callback, state);
        }

        public IHttpResponse EndSend(IAsyncResult asyncResult)
        {
            return Sync.EndSend(asyncResult);
        }

        #endregion
    }
}
