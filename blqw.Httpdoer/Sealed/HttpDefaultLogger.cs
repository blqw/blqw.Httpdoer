using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 默认日志记录器
    /// </summary>
    public sealed class HttpDefaultLogger : IHttpLogger
    {
        private HttpDefaultLogger() { }
        private string AssemblyName = typeof(HttpDefaultLogger).Module.Assembly.GetName().Name;


        public static readonly HttpDefaultLogger Instance = new HttpDefaultLogger();

        /// <summary>
        /// 在写日志的时候是否输出应用名称
        /// </summary>
        public static bool WriteAssemblyName { get; set; } = true;

        public void Debug(string message)
        {
            if (WriteAssemblyName)
                Trace.WriteLine(AssemblyName + ": " + message);
            else
                Trace.WriteLine(message);
        }

        public void Information(string message)
        {
            if (WriteAssemblyName)
                Trace.TraceInformation(AssemblyName + ": " + message);
            else
                Trace.TraceInformation(message);
        }

        public void Warning(string message)
        {
            if (WriteAssemblyName)
                Trace.TraceWarning(AssemblyName + ": " + message);
            else
                Trace.TraceWarning(message);
        }

        public void Error(string message)
        {
            if (WriteAssemblyName)
                Trace.TraceError(AssemblyName + ": " + message);
            else
                Trace.TraceError(message);
        }

        public void Error(Exception ex)
        {
            if (WriteAssemblyName)
                Trace.WriteLine(ex, AssemblyName);
            else
                Trace.WriteLine(ex);
        }
    }
}
