using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 用于描述 HTTP 请求时的参数
    /// </summary>
    public interface IHttpRequest : IEnumerable<HttpParamValue>
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
        CookieContainer Cookie { get; set; }
        /// <summary>
        /// HTTP 参数,根据 Method 和 Path 来确定参数位置
        /// </summary>
        HttpParams Params { get; }

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
        HttpMethod Method { get; set; }
        /// <summary> 
        /// 请求编码
        /// </summary>
        Encoding Encoding { get; set; }
        /// <summary> 
        /// 超时时间
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <returns></returns>
        Uri GetURL();

        /// <summary>
        /// 获取或设置 HTTP 消息版本。默认值为 1.1。
        /// </summary>
        Version Version { get; set; }
    }
}
