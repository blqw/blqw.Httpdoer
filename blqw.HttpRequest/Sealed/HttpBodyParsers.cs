using System;

namespace blqw.Web
{
    /// <summary>
    /// HTTP 正文解析器
    /// </summary>
    public static class HttpBodyParsers
    {
        /// <summary>
        /// 获取 HTTP 正文解析器
        /// </summary>
        /// <param name="type"> 正文类型 </param>
        /// <param name="format"> 正文格式 </param>
        /// <returns></returns>
        public static IHttpBodyParser Get(string type, string format)
        {
            switch (format?.ToLowerInvariant())
            {
                case "x-www-form-urlencoded":
                    return HttpFormBodyParser.Instance;
                case "html":
                case "plain":
                    return HttpTextBodyParser.Instance;
                default:
                    throw new NotImplementedException("暂不支持");
            }
        }
    }
}