using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 请求池,用于管理意外卡死的请求
    /// </summary>
    public static class RequestPool
    {
        static Task KillTask = Task.Run((Action)Kill);

        static void Kill()
        {
            var free = new ConcurrentQueue<HttpRequest>();
            while (true)
            {
                try
                {
                    System.Threading.Thread.Sleep(_Interval);
                    Swap(ref free, ref _Pool);
                    HttpRequest req;
                    while (free.TryDequeue(out req))
                    {
                        if (req.IsCompletedOrTimeout() == false)
                        {
                            Add(req);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex, "HttpRequest守护线程异常");
                }
            }
        }

        static void Swap(ref ConcurrentQueue<HttpRequest> q1, ref ConcurrentQueue<HttpRequest> q2)
        {
            var t = q1;
            q1 = q2;
            q2 = t;
        }

        private static int _Interval = 15000;

        /// <summary>
        /// 检查间隔时间,设置大于60秒或小于5秒无效,默认15秒
        /// </summary>
        public static int Interval
        {
            get
            {
                return _Interval;
            }
            set
            {
                if (value > 60000)
                {
                    _Interval = 60000;
                }
                else if (value < 5000)
                {
                    _Interval = 5000;
                }
                else
                {
                    _Interval = value;
                }
            }
        }

        private static ConcurrentQueue<HttpRequest> _Pool = new ConcurrentQueue<HttpRequest>();

        public static void Add(HttpRequest request)
        {
            _Pool.Enqueue(request);
        }


    }
}
