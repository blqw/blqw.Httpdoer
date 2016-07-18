using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blqw.Web
{
    public class Httpdoer : HttpRequest
    {
        static Httpdoer()
        {
            ServicePointManager.MaxServicePointIdleTime = 30000;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.SetTcpKeepAlive(true, 30000, 30000);
        }

        public Httpdoer(string baseUrl) : base(baseUrl)
        {
        }

        public Httpdoer()
        {
        }
    }
}
