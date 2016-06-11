using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 日志记录器
    /// </summary>
    public interface IHttpLogger
    {
        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="message">日志内容</param>
        void Debug(string message);
        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="message">日志内容</param>
        void Information(string message);
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message">日志内容</param>
        void Warning(string message);
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="message">日志内容</param>
        void Error(string message);
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="ex">异常对象</param>
        void Error(Exception ex);
    }
}
