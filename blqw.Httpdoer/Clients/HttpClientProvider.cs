using System;
using System.Net;
using System.Net.Http;
using System.Threading;
#pragma warning disable 618
using static System.Net.GlobalProxySelection;
#pragma warning restore 618

namespace blqw.Web
{
    /// <summary>
    /// HttpClient的包装类
    /// </summary>
    public static class HttpClientProvider
    {
        /// <summary>
        /// 不使用任何代理的 <seealso cref="HttpClient" />对象
        /// </summary>
        private static readonly HttpClient _Client = Create(null);

        /// <summary>
        /// 使用空代理的 <seealso cref="HttpClient" />对象
        /// </summary>
        private static readonly HttpClient _EmptyProxyClient = Create(GetEmptyWebProxy());

        /// <summary>
        /// 全局默认代理
        /// </summary>
        private static IWebProxy _DefaultWebProxy;

        /// <summary>
        /// 用于请求的<seealso cref="HttpClient" />对象
        /// </summary>
        private static HttpClient _DefaultWebProxyClient = Create(_DefaultWebProxy = WebRequest.DefaultWebProxy);

        /// <summary>
        /// 空代理(EmptyWebProxy)的类型
        /// </summary>
        private static readonly Type _EmptyProxyType = GetEmptyWebProxy().GetType();

        /// <summary>
        /// 根据代理构造一个新的 <seealso cref="HttpClient" />对象
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        private static HttpClient Create(IWebProxy proxy) => new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = false, //不处理302
            UseCookies = false, //不使用cookie
            AutomaticDecompression = DecompressionMethods.GZip, //自动处理gzip
            ClientCertificateOptions = ClientCertificateOption.Automatic, //自动处理证书
            UseProxy = proxy != null,
            Proxy = proxy
        })
        {
            Timeout = Timeout.InfiniteTimeSpan, //默认无超时
            MaxResponseContentBufferSize = int.MaxValue //设置缓冲字节最大值
        };

        /// <summary>
        /// 获取请求客户端
        /// </summary>
        /// <param name="proxy"> web代理 </param>
        /// <returns> </returns>
        public static HttpClient GetClient(IWebProxy proxy)
        {
            if (proxy == null) //不使用代理的情况下
            {
                return _Client;
            }
            if (proxy.GetType() == _EmptyProxyType)
            {
                return _EmptyProxyClient;
            }
            if (proxy == _DefaultWebProxy)
            {
                return _DefaultWebProxyClient;
            }
            if (proxy == WebRequest.DefaultWebProxy)
            {
                return _DefaultWebProxyClient = Create(_DefaultWebProxy = proxy);
            }
            return Create(proxy);
        }

        /// <summary>
        /// 判断当前对象是否被缓存
        /// </summary>
        public static bool IsCached(object client) => ReferenceEquals(_Client, client) || ReferenceEquals(_EmptyProxyClient, client) || ReferenceEquals(_DefaultWebProxyClient, client);
    }
}