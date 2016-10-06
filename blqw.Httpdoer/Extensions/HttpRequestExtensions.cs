using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using blqw.IOC;

namespace blqw.Web
{
    /// <summary>
    /// HttpRequest 扩展方法
    /// </summary>
    public static class HttpRequestExtensions
    {
        #region 同步

        /// <summary>
        /// 同步客户端
        /// </summary>
        private static readonly IHttpClient SyncClient = new HttpClientSync();

        /// <summary>
        /// 同步发送请求,返回响应体
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <returns> </returns>
        public static IHttpResponse Send(this IHttpRequest request) => SyncClient.Send(request);

        /// <summary>
        /// 同步发送请求,返回字符串
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <returns> </returns>
        public static string GetString(this IHttpRequest request)
        {
            var res = SyncClient.Send(request);
            var str = res.Body?.ToString();
            request.Logger?.Write(TraceEventType.Information, str);
            return str;
        }

        /// <summary>
        /// 同步发送请求,返回字节数组
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <returns> </returns>
        public static byte[] GetBytes(this IHttpRequest request) => SyncClient.Send(request).Body?.ResponseBody;

        /// <summary>
        /// 同步发送请求,返回实体对象
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="request"> 请求 </param>
        /// <returns> </returns>
        public static T GetObject<T>(this IHttpRequest request)
        {
            var res = SyncClient.Send(request);
            return res.Body == null ? default(T) : res.Body.ToObject<T>();
        }

        #endregion

        #region 异步

        /// <summary>
        /// 异步客户端
        /// </summary>
        private static readonly IHttpClient AsyncClient = new HttpClientAsync();

        /// <summary>
        /// 异步发送请求,返回响应体
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <returns> </returns>
        public static Task<IHttpResponse> SendAsync(this IHttpRequest request) => SendAsync(request, CancellationToken.None);

        /// <summary>
        /// 异步发送请求,设置超时时间,返回响应体
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <param name="timeout"> 超时时间 </param>
        /// <returns> </returns>
        public static Task<IHttpResponse> SendAsync(this IHttpRequest request, TimeSpan timeout)
        {
            using (var source = new CancellationTokenSource(timeout))
            {
                return SendAsync(request, source.Token);
            }
        }

        /// <summary>
        /// 异步发送请求,设置取消标识,返回响应体
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <param name="cancellationToken"> 取消标识 </param>
        /// <returns> </returns>
        public static Task<IHttpResponse> SendAsync(this IHttpRequest request, CancellationToken cancellationToken) => AsyncClient.SendAsync(request, cancellationToken);

        /// <summary>
        /// 异步发送请求,返回字符串
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <returns> </returns>
        public static Task<string> GetStringAsync(this IHttpRequest request) => GetStringAsync(request, CancellationToken.None);

        /// <summary>
        /// 异步发送请求,返回字节数组
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <returns> </returns>
        public static Task<byte[]> GetBytesAsync(this IHttpRequest request) => GetBytesAsync(request, CancellationToken.None);

        /// <summary>
        /// 异步发送请求,返回实体对象
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="request"> 请求 </param>
        /// <returns> </returns>
        public static Task<T> GetObjectAsync<T>(this IHttpRequest request) => GetObjectAsync<T>(request, CancellationToken.None);

        /// <summary>
        /// 异步发送请求,设置超时时间,返回字符串
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <param name="timeout"> 超时时间 </param>
        /// <returns> </returns>
        public static async Task<string> GetStringAsync(this IHttpRequest request, TimeSpan timeout)
        {
            using (var tokenSource = new CancellationTokenSource(timeout))
            {
                return await GetStringAsync(request, tokenSource.Token);
            }
        }

        /// <summary>
        /// 异步发送请求,设置超时时间,返回字节数组
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <param name="timeout"> 超时时间 </param>
        /// <returns> </returns>
        public static async Task<byte[]> GetBytesAsync(this IHttpRequest request, TimeSpan timeout)
        {
            using (var source = new CancellationTokenSource(timeout))
            {
                return await GetBytesAsync(request, source.Token);
            }
        }

        /// <summary>
        /// 异步发送请求,设置超时时间,返回实体对象
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="request"> 请求 </param>
        /// <param name="timeout"> 超时时间 </param>
        /// <returns> </returns>
        public static async Task<T> GetObjectAsync<T>(this IHttpRequest request, TimeSpan timeout)
        {
            using (var source = new CancellationTokenSource(timeout))
            {
                return await GetObjectAsync<T>(request, source.Token);
            }
        }

        /// <summary>
        /// 异步发送请求,设置取消标识,返回字符串
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <param name="cancellationToken"> 取消标识 </param>
        /// <returns> </returns>
        public static async Task<string> GetStringAsync(this IHttpRequest request, CancellationToken cancellationToken)
        {
            var res = await AsyncClient.SendAsync(request, cancellationToken);
            var str = res.Body?.ToString();
            request.Logger?.Write(TraceEventType.Information, str);
            return str;
        }

        /// <summary>
        /// 异步发送请求,设置取消标识,返回字节数组
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <param name="cancellationToken"> 取消标识 </param>
        /// <returns> </returns>
        public static async Task<byte[]> GetBytesAsync(this IHttpRequest request, CancellationToken cancellationToken)
        {
            var res = await AsyncClient.SendAsync(request, cancellationToken);
            return res.Body?.ResponseBody;
        }

        /// <summary>
        /// 异步发送请求,设置取消标识,返回实体对象
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <param name="cancellationToken"> 取消标识 </param>
        /// <returns> </returns>
        public static async Task<T> GetObjectAsync<T>(this IHttpRequest request, CancellationToken cancellationToken)
        {
            var res = await AsyncClient.SendAsync(request, cancellationToken);
            return res.Body == null ? default(T) : res.Body.ToObject<T>();
        }

        #endregion

        #region Begin...End

        /// <summary>
        /// 异步回调客户端
        /// </summary>
        private static readonly IHttpClient CallbackClient = new HttpClientSync();

        /// <summary>
        /// 异步发送请求,并使用回调函数处理回调逻辑,也可以使用EndSend方法来接收返回值
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <param name="callback"> 回调方法 </param>
        /// <param name="state"> 需要状态回调方法的参数 </param>
        /// <returns> </returns>
        public static IAsyncResult BeginSend(this IHttpRequest request, AsyncCallback callback, object state) => CallbackClient.BeginSend(request, callback, state);

        /// <summary>
        /// 阻塞当前线程,直到异步操作接收到返回值
        /// </summary>
        /// <param name="request"> 请求 </param>
        /// <param name="asyncResult"> 表示一个异步操作 </param>
        /// <returns> </returns>
        // ReSharper disable once UnusedParameter.Global
        public static IHttpResponse EndSend(this IHttpRequest request, IAsyncResult asyncResult) => CallbackClient.EndSend(asyncResult);

        #endregion

        #region Trackings

        /// <summary>
        /// 当请求正在提取参数时触发
        /// </summary>
        internal static void OnParamsExtracting(this IHttpRequest request)
        {
            var trackings = request?.Trackings;
            if ((trackings == null) || (trackings.Count == 0))
            {
                return;
            }
            for (int i = 0, length = trackings.Count; i < length; i++)
            {
                trackings[i]?.OnParamsExtracting(request);
            }
        }

        /// <summary>
        /// 当请求提取参数完成时触发
        /// </summary>
        internal static void OnParamsExtracted(this IHttpRequest request)
        {
            var trackings = request?.Trackings;
            if ((trackings == null) || (trackings.Count == 0))
            {
                return;
            }
            for (int i = 0, length = trackings.Count; i < length; i++)
            {
                trackings[i]?.OnParamsExtracted(request);
            }
        }

        /// <summary>
        /// 当请求提取参数,并找到一个Query参数时触发
        /// </summary>
        internal static void OnQueryParamFound(this IHttpRequest request, ref string name, ref object value)
        {
            var trackings = request?.Trackings;
            if ((trackings == null) || (trackings.Count == 0))
            {
                return;
            }
            for (int i = 0, length = trackings.Count; i < length; i++)
            {
                trackings[i]?.OnQueryParamFound(request, ref name, ref value);
            }
        }

        /// <summary>
        /// 当请求提取参数,并找到一个Body参数时触发
        /// </summary>
        internal static void OnBodyParamFound(this IHttpRequest request, ref string name, ref object value)
        {
            var trackings = request?.Trackings;
            if ((trackings == null) || (trackings.Count == 0))
            {
                return;
            }
            for (int i = 0, length = trackings.Count; i < length; i++)
            {
                trackings[i]?.OnBodyParamFound(request, ref name, ref value);
            }
        }

        /// <summary>
        /// 当请求提取参数,并找到一个Header参数时触发
        /// </summary>
        internal static void OnHeaderFound(this IHttpRequest request, ref string name, ref IEnumerable<string> values)
        {
            var trackings = request?.Trackings;
            if ((trackings == null) || (trackings.Count == 0))
            {
                return;
            }
            for (int i = 0, length = trackings.Count; i < length; i++)
            {
                trackings[i]?.OnHeaderFound(request, ref name, ref values);
            }
        }

        /// <summary>
        /// 当请求提取参数,并找到一个Path参数时触发
        /// </summary>
        internal static void OnPathParamFound(this IHttpRequest request, ref string name, ref string value)
        {
            var trackings = request?.Trackings;
            if ((trackings == null) || (trackings.Count == 0))
            {
                return;
            }
            for (int i = 0, length = trackings.Count; i < length; i++)
            {
                trackings[i]?.OnPathParamFound(request, ref name, ref value);
            }
        }

        /// <summary>
        /// 当请求正在初始化时触发
        /// </summary>
        internal static void OnInitialize(this IHttpRequest request)
        {
            var trackings = request?.Trackings;
            if ((trackings == null) || (trackings.Count == 0))
            {
                return;
            }
            for (int i = 0, length = trackings.Count; i < length; i++)
            {
                trackings[i]?.OnInitialize(request);
            }
        }

        /// <summary>
        /// 当请求出现错误时触发
        /// </summary>
        internal static void OnError(this IHttpRequest request, IHttpResponse response)
        {
            var trackings = request?.Trackings;
            if ((trackings == null) || (trackings.Count == 0))
            {
                return;
            }
            for (int i = 0, length = trackings.Count; i < length; i++)
            {
                trackings[i]?.OnError(request, response);
            }
        }

        /// <summary>
        /// 当请求正被发送到服务器时触发
        /// </summary>
        internal static void OnSending(this IHttpRequest request)
        {
            var trackings = request?.Trackings;
            if ((trackings == null) || (trackings.Count == 0))
            {
                return;
            }
            for (int i = 0, length = trackings.Count; i < length; i++)
            {
                trackings[i]?.OnSending(request);
            }
        }
        /// <summary>
        /// 当请求发送完成,并已成功接受返回响应时触发
        /// </summary>
        internal static void OnEnd(this IHttpRequest request, IHttpResponse response)
        {
            var trackings = request?.Trackings;
            if ((trackings == null) || (trackings.Count == 0))
            {
                return;
            }
            for (int i = 0, length = trackings.Count; i < length; i++)
            {
                trackings[i]?.OnEnd(request, response);
            }
        }

        #endregion

        #region Logs
        // ReSharper disable ExplicitCallerInfoArgument
        /// <summary>
        /// 写入调试日志
        /// </summary>
        public static void Debug(this IHttpRequest request, string message,
                [CallerLineNumber] int line = 0, [CallerMemberName] string member = "",
                [CallerFilePath] string file = "")
            => request?.Logger?.Write(TraceEventType.Verbose, message, null, line, member, file);

        /// <summary>
        /// 写入提示日志
        /// </summary>
        public static void Information(this IHttpRequest request, string message,
                [CallerLineNumber] int line = 0, [CallerMemberName] string member = "",
                [CallerFilePath] string file = "")
            => request?.Logger?.Write(TraceEventType.Information, message, null, line, member, file);

        /// <summary>
        /// 写入警告日志
        /// </summary>
        public static void Warning(this IHttpRequest request, string message,
                [CallerLineNumber] int line = 0, [CallerMemberName] string member = "",
                [CallerFilePath] string file = "")
            => request?.Logger?.Write(TraceEventType.Warning, message, null, line, member, file);

        /// <summary>
        /// 写入异常日志
        /// </summary>
        public static void Error(this IHttpRequest request, string message,
                [CallerLineNumber] int line = 0, [CallerMemberName] string member = "",
                [CallerFilePath] string file = "")
            => request?.Logger?.Write(TraceEventType.Error, message, null, line, member, file);

        /// <summary>
        /// 写入异常日志
        /// </summary>
        public static void Error(this IHttpRequest request, Exception ex,
                [CallerLineNumber] int line = 0, [CallerMemberName] string member = "",
                [CallerFilePath] string file = "")
            => request?.Logger?.Write(TraceEventType.Error, ex.Message, ex, line, member, file);

        // ReSharper restore ExplicitCallerInfoArgument
        #endregion
    }
}