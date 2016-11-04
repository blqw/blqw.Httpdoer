using System.Diagnostics;
using System.Net;

namespace blqw.Web
{
    /// <summary>
    /// HTTP 请求入口
    /// </summary>
    public class Httpdoer : HttpRequest
    {
        #region 实例方法
        /// <summary>
        /// 默认日志记录器
        /// </summary>
        public static readonly TraceSource DefaultLogger = new IOC.LoggerSource("blqw.Httpdoer", SourceLevels.Information);

        static Httpdoer()
        {
            ServicePointManager.MaxServicePointIdleTime = 30000;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.SetTcpKeepAlive(true, 30000, 30000);
        }

        /// <summary>
        /// 初始化 HTTP 请求,并设定基路径
        /// </summary>
        /// <param name="baseUrl"> 基路径 </param>
        public Httpdoer(string baseUrl) : base(baseUrl) { }

        /// <summary>
        /// 初始化一个新的 HTTP 请求
        /// </summary>
        public Httpdoer() { }
        #endregion

        /// <summary>
        /// 快速获得一个请求对象
        /// </summary>
        /// <param name="method">请求方法</param>
        /// <param name="url">请求地址</param>
        /// <param name="contentType">请求类型</param>
        /// <param name="get">query参数</param>
        /// <param name="post">formbody参数</param>
        /// <param name="header">header头参数</param>
        /// <returns></returns>
        public static Httpdoer Request(string method, string url, HttpContentType contentType = default(HttpContentType), object get = null, object post = null, object header = null)
        {
            var www = new Httpdoer(url);
            www.HttpMethod = method;
            www.Body.ContentType = contentType;
            if (header != null)
            {
                www.Headers.AddModel(header);
            }
            if (post != null)
            {
                www.Body.AddModel(post);
            }
            if (get != null)
            {
                var q = get as string;
                if (q != null)
                {
                    www.Query.AddQuery(q);
                }
                else
                {
                    www.Query.AddModel(get);
                }
            }
            return www;
        }

        /// <summary>
        /// 获取get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="args">请求参数</param>
        /// <param name="header">请求头</param>
        /// <returns></returns>
        public static Httpdoer Get(string url, object args = null, object header = null)
            => Request("GET", url, default(HttpContentType), args, null, header);

        /// <summary>
        /// 获取post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="args">请求参数</param>
        /// <param name="header">请求头</param>
        public static Httpdoer Post(string url, object args, object header = null)
            => Request("POST", url, HttpContentType.Form, null, args, header);

        /// <summary>
        /// 获取使用json方式提交参数的post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="args">请求参数</param>
        /// <param name="header">请求头</param>
        public static Httpdoer PostJson(string url, object args, object header = null)
            => Request("POST", url, HttpContentType.Json, null, args, header);

        /// <summary>
        /// 获取put请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="args">请求参数</param>
        /// <param name="header">请求头</param>
        public static Httpdoer Put(string url, object args, object header = null)
            => Request("PUT", url, HttpContentType.Form, null, args, header);

        /// <summary>
        /// 获取使用json方式提交参数的put请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="args">请求参数</param>
        /// <param name="header">请求头</param>
        public static Httpdoer PutJson(string url, object args, object header = null)
            => Request("PUT", url, HttpContentType.Json, null, args, header);

        /// <summary>
        /// 获取patch请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="args">请求参数</param>
        /// <param name="header">请求头</param>
        public static Httpdoer Patch(string url, object args, object header = null)
            => Request("PATCH", url, HttpContentType.Form, null, args, header);

        /// <summary>
        /// 获取使用json参数提交参数的patch请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="args">请求参数</param>
        /// <param name="header">请求头</param>
        public static Httpdoer PatchJson(string url, object args, object header = null)
            => Request("PATCH", url, HttpContentType.Json, null, args, header);

        /// <summary>
        /// 获取一个get请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="args">请求参数</param>
        /// <param name="header">请求头</param>
        public static Httpdoer Delete(string url, object args = null, object header = null)
            => Request("DELETE", url, default(HttpContentType), args, null, header);

    }
}