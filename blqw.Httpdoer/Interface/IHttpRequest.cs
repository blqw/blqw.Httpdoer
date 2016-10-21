using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace blqw.Web
{
    /// <summary>
    /// 用于描述 HTTP 请求时的参数
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// 基路径
        /// </summary>
        Uri BaseUrl { get; set; }

        /// <summary>
        /// 基路径的相对路径
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        HttpRequestMethod Method { get; set; }

        /// <summary>
        /// 请求方式的字符串形式
        /// </summary>
        string HttpMethod { get; set; }

        /// <summary>
        /// 完整路径
        /// </summary>
        Uri FullUrl { get; }

        /// <summary>
        /// HTTP 头信息
        /// </summary>
        HttpHeaders Headers { get; }

        /// <summary>
        /// HTTP 请求查询参数
        /// </summary>
        HttpStringParams Query { get; }

        /// <summary>
        /// HTTP 请求路径参数
        /// </summary>
        HttpStringParams PathParams { get; }

        /// <summary>
        /// HTTP 请求正文
        /// </summary>
        HttpBody Body { get; }

        /// <summary>
        /// Cookie
        /// </summary>
        CookieContainer Cookies { get; set; }

        /// <summary>
        /// 代理设置
        /// </summary>
        IWebProxy Proxy { get; set; }

        /// <summary>
        /// HTTP 参数,根据 Method 和 Path 来确定参数位置
        /// </summary>
        HttpParams Params { get; }

        /// <summary>
        /// 超时时间
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 消息版本。默认值为 1.1。
        /// </summary>
        Version Version { get; set; }

        /// <summary>
        /// 最后一次响应
        /// </summary>
        IHttpResponse Response { get; set; }

        /// <summary>
        /// 是否使用 Cookie
        /// </summary>
        [Obsolete("使用新属性CookieMode来设置,默认为 HttpCookieMode.CustomOrCache ")]
        bool UseCookies { get; set; }

        /// <summary>
        /// 缓存模式
        /// </summary>
        HttpCookieMode CookieMode { get; set; }

        /// <summary>
        /// 自动302跳转
        /// </summary>
        bool AutoRedirect { get; set; }

        /// <summary>
        /// 获取日志记录器
        /// </summary>
        TraceSource Logger { get; }

        /// <summary>
        /// 获取用于触发一系列事件的跟踪对象
        /// </summary>
        List<IHttpTracking> Trackings { get; }

        /// <summary>
        /// 枚举所有请求参数
        /// </summary>
        IEnumerator<HttpParamValue> GetEnumerator();

        /// <summary>
        /// 获取一个用于异步请求的
        /// </summary>
        HttpMessageInvoker GetAsyncInvoker();
    }
}