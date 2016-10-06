using System;
using System.Collections.Generic;

namespace blqw.Web
{
    /// <summary>
    /// 用于描述 HTTP 请求时的参数
    /// </summary>
    public interface IHttpRequest : IHttpRequestBase
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
        /// 枚举所有请求参数
        /// </summary>
        IEnumerator<HttpParamValue> GetEnumerator();
        
    }
}