using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 计时器
    /// </summary>
    struct HttpTimer
    {
        Stopwatch _Watch;
        long ready; //本地操作耗时
        long send;  //发送请求耗时
        long end;   //
        long err;   //异常操作耗时
        public static HttpTimer Start()
        {
            var timer = new HttpTimer();
            timer._Watch = Stopwatch.StartNew();
            return timer;
        }

        /// <summary>
        /// 准备完毕
        /// </summary>
        public void Readied()
        {
            _Watch.Stop();
            ready = _Watch.ElapsedMilliseconds;
            _Watch.Restart();
            ready = -1;
            send = -1;
            end = -1;
            err = -1;
        }

        /// <summary>
        /// 发送完成
        /// </summary>
        public void Sent()
        {
            _Watch.Stop();
            send = _Watch.ElapsedMilliseconds;
            _Watch.Restart();
        }

        /// <summary>
        /// 异常
        /// </summary>
        public void Error()
        {
            _Watch.Stop();
            err = _Watch.ElapsedMilliseconds;
            _Watch.Restart();
        }

        /// <summary>
        /// 结束
        /// </summary>
        public void Ending()
        {
            _Watch.Stop();
            end = _Watch.ElapsedMilliseconds;
        }

        public override string ToString()
        {
            if (send >= 0)
            {
                return $"init:{ready} ms, send:{send} ms, end:{end} ms";
            }
            else if(err >= 0)
            {
                return $"init:{ready} ms, err:{err} ms, end:{end} ms";
            }
            else
            {
                return $"init:{ready} ms";
            }
        }
    }
}
