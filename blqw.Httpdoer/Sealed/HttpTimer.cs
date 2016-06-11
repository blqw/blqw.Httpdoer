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
        long _Ready; //本地操作耗时
        long _Send;  //发送请求耗时
        long _End;   //
        long _Err;   //异常操作耗时
        public static HttpTimer Start()
        {
            return new HttpTimer()
            {
                _Ready = -1,
                _Send = -1,
                _End = -1,
                _Err = -1,
                _Watch = Stopwatch.StartNew(),
            };
        }

        /// <summary>
        /// 准备完毕
        /// </summary>
        public void Readied()
        {
            _Watch.Stop();
            _Ready = _Watch.ElapsedMilliseconds;
            _Watch.Restart();
        }

        /// <summary>
        /// 发送完成
        /// </summary>
        public void Sent()
        {
            _Watch.Stop();
            _Send = _Watch.ElapsedMilliseconds;
            _Watch.Restart();
        }

        /// <summary>
        /// 异常
        /// </summary>
        public void Error()
        {
            _Watch.Stop();
            _Err = _Watch.ElapsedMilliseconds;
            _Watch.Restart();
        }

        /// <summary>
        /// 结束
        /// </summary>
        public void Ending()
        {
            _Watch.Stop();
            _End = _Watch.ElapsedMilliseconds;
        }

        public override string ToString()
        {
            if (_Send >= 0)
            {
                return $"ready:{_Ready} ms, send:{_Send} ms, end:{_End} ms";
            }
            else if (_Err >= 0)
            {
                return $"ready:{_Ready} ms, err:{_Err} ms, end:{_End} ms";
            }
            else
            {
                return $"ready:{_Ready} ms";
            }
        }
    }
}
