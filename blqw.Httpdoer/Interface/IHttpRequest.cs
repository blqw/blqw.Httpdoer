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
    public interface IHttpRequest : IHttpRequestBase, IEnumerable<HttpParamValue>
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
        /// 完整路径
        /// </summary>
        Uri FullUrl { get; }
    }
}
