using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    public interface IHttpRequestBase
    {
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
        /// HTTP 参数,根据 Method 和 Path 来确定参数位置
        /// </summary>
        HttpParams Params { get; }
        /// <summary> 
        /// 请求编码
        /// </summary>
        Encoding Encoding { get; set; }
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
        bool UseCookies { get; set; }

        /// <summary>
        /// 设置或获取日志记录器
        /// </summary>
        IHttpLogger Logger { get; set; }

        /// <summary>
        /// 获取或设置用于触发一系列事件的跟踪对象
        /// </summary>
        IHttpTracking Tracking { get; set; }
    }
}
