using System;
using System.Collections.Generic;
using System.Linq;
using blqw.IOC;

namespace blqw.Web
{
    /// <summary>
    /// 列举所有系统Body解析器
    /// </summary>
    public static class HttpBodyParsers
    {
        private static readonly List<IHttpBodyParser> _Cache;

        static HttpBodyParsers()
        {
            _Cache = MEF.PlugIns.GetExports<IHttpBodyParser>().ToList();
        }

        /// <summary>
        /// 默认解析器,当存在charset时解析为字符串,否则解析为字节流
        /// </summary>
        public static HttpBodyParserBase Default { get; } = new HttpBodyDefaultParser();

        /// <summary>
        /// 表单解析器,用于解析 x-www-form-urlencoded 格式的正文
        /// </summary>
        public static HttpBodyParserBase Form { get; } = new HttpFormBodyParser();

        /// <summary>
        /// Json解析器,用于解析 Json 格式的正文
        /// </summary>
        public static HttpBodyParserBase Json { get; } = new HttpJsonBodyParser();

        /// <summary>
        /// 空解析器,没有任何功能
        /// </summary>
        public static HttpBodyParserBase Null { get; } = new HttpNullBodyParser();

        /// <summary>
        /// Protobuf解析器,用于解析 Protobuf 格式的正文
        /// </summary>
        public static HttpBodyParserBase Protobuf { get; } = new HttpProtobufBodyParser();

        /// <summary>
        /// 流解析器,用于解析字节流形式的正文
        /// </summary>
        public static HttpBodyParserBase Stream { get; } = new HttpStreamBodyParser();

        /// <summary>
        /// 字符串解析器,用于字符串类型的正文,包括text,html,plain等
        /// </summary>
        public static HttpBodyParserBase String { get; } = new HttpStringBodyParser();

        /// <summary>
        /// XML解析器,用于解析 XML 格式的正文
        /// </summary>
        public static HttpBodyParserBase XML { get; } = new HttpXMLBodyParser();

        /// <summary>
        /// 获取一个匹配的解析器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="format"></param>
        public static IHttpBodyParser Get(string type, string format) 
            => _Cache.FirstOrDefault(it => it.IsMatch(type, format));
    }
}