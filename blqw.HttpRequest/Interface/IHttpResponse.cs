using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 表示一个 HTTP 响应
    /// </summary>
    public interface IHttpResponse
    {
        /// <summary>
        /// HTTP 头信息
        /// </summary>
        HttpHeaders Headers { get; }
        /// <summary>
        /// HTTP 响应正文
        /// </summary>
        HttpBody Body { get; }
        /// <summary>
        /// Cookie
        /// </summary>
        CookieCollection Cookies { get; }

        /// <summary>
        /// HTTP 响应的状态代码
        /// </summary>
        HttpStatusCode StatusCode { get; }
        /// <summary>
        /// 获取一个值，该值指示 HTTP 响应是否成功。StatusCode 在 200-299 范围中，则为 true；否则为 false
        /// </summary>
        bool IsSuccessStatusCode { get; }

        /// <summary>
        /// 如果 HTTP 响应失败,则获取异常信息
        /// </summary>
        Exception Exception { get; }

    }
}
