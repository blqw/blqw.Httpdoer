
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary> 简单访问Web页面或接口
    /// </summary>
    public static partial class SimpleRequest
    {
        /// <summary> 请求url并获取返回文本
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="method">方法名</param>
        /// <param name="query">url参数</param>
        /// <param name="body">body参数</param>
        /// <returns></returns>
        [Export("Request")]
        [ExportMetadata("Priority", 100)]
        public static Task<string> Execute(string url, string method, object body, object query)
        {
            var request = new HttpRequest();
            request.Path = url;
            if (query != null)
                request.QueryString.Add(query);
            if (body != null)
                request.FormBody.Add(body);
            HttpRequestMethod m;
            if (Enum.TryParse<HttpRequestMethod>(method, true, out m) == false)
            {
                request.Method = HttpRequestMethod.GET;
            }
            return request.GetString();
        }
        /// <summary> 使用get方式请求url并获取返回文本
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="encoding">请求编码格式,默认utf8</param>
        /// <remarks>周子鉴 2015.08.19</remarks>
        public static Task<string> Get(string url, object data, Encoding encoding = null)
        {
            var request = new HttpRequest();
            request.Path = url;
            request.Encoding = encoding;
            request.QueryString.Add(data);
            request.Method = HttpRequestMethod.GET;
            return request.GetString();
        }

        /// <summary> 使用post方式请求url并获取返回文本
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数</param>
        /// <param name="encoding">请求编码格式,默认utf8</param>
        /// <remarks>周子鉴 2015.08.19</remarks>
        public static Task<string> Post(string url, object data, Encoding encoding = null)
        {
            var request = new HttpRequest();
            request.Path = url;
            request.Encoding = encoding;
            request.QueryString.Add(data);
            request.Method = HttpRequestMethod.POST;
            return request.GetString();
        }
    }

}
