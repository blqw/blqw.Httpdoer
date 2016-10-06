using System.Diagnostics;
using System.Net;

namespace blqw.Web
{
    /// <summary>
    /// HTTP 请求入口
    /// </summary>
    public class Httpdoer : HttpRequest
    {
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
        public Httpdoer(string baseUrl) : base(baseUrl)
        {
        }

        /// <summary>
        /// 初始化一个新的 HTTP 请求
        /// </summary>
        public Httpdoer()
        {
        }
    }
}