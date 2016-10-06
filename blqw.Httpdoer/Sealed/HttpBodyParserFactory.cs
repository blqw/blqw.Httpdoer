using System.Collections.Generic;

namespace blqw.Web
{
    /// <summary>
    /// 默认解析器工厂
    /// </summary>
    public sealed class HttpBodyParserFactory : IHttpBodyParserFactory
    {
        public static readonly IHttpBodyParserFactory Default = new HttpBodyParserFactory();

        public static readonly IList<IHttpBodyParserFactory> Factories = new List<IHttpBodyParserFactory> { Default };

        private HttpBodyParserFactory()
        {
        }

        public IHttpBodyParser Create(string type, string format)
        {
            switch (format?.ToLowerInvariant())
            {
                case "x-www-form-urlencoded":
                    return HttpBodyParsers.Form;
                case "html":
                case "plain":
                    return HttpBodyParsers.String;
                case "json":
                    return HttpBodyParsers.Json;
                case "xml":
                    return HttpBodyParsers.XML;
                case "octet-stream":
                    return HttpBodyParsers.Stream;
                case null:
                    return HttpBodyParsers.Null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取 HTTP 正文解析器
        /// </summary>
        /// <param name="type"> 正文类型 </param>
        /// <param name="format"> 正文格式 </param>
        /// <returns> </returns>
        public static IHttpBodyParser Get(string type, string format)
        {
            if (Factories?.Count > 0)
            {
                foreach (var factory in Factories)
                {
                    var parser = factory.Create(type, format);
                    if (parser != null)
                    {
                        return parser;
                    }
                }
            }
            return HttpBodyParsers.Default;
        }
    }
}