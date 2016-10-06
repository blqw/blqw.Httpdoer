using System.Diagnostics;

namespace blqw.Web
{
    /// <summary>
    /// 用于包装计时器,并提供简单的操作
    /// </summary>
    internal struct HttpRequestTimer
    {
        /// <summary>
        /// 计时器
        /// </summary>
        private Stopwatch _watch;
        /// <summary>
        /// 本地操作耗时
        /// </summary>
        private long _ready;
        /// <summary>
        /// 发送请求耗时
        /// </summary>
        private long _send;
        /// <summary>
        /// 请求完成耗时
        /// </summary>
        private long _end; 
        /// <summary>
        /// 异常操作耗时
        /// </summary>
        private long _err; 

        /// <summary>
        /// 请求开始
        /// </summary>
        /// <returns></returns>
        public static HttpRequestTimer OnStart()
        {
            return new HttpRequestTimer
            {
                _ready = -1,
                _send = -1,
                _end = -1,
                _err = -1,
                _watch = Stopwatch.StartNew()
            };
        }

        /// <summary>
        /// 准备完毕
        /// </summary>
        public void OnReady()
        {
            _watch.Stop();
            _ready = _watch.ElapsedMilliseconds;
            _watch.Restart();
        }

        /// <summary>
        /// 发送完成
        /// </summary>
        public void OnSend()
        {
            _watch.Stop();
            _send = _watch.ElapsedMilliseconds;
            _watch.Restart();
        }

        /// <summary>
        /// 异常
        /// </summary>
        public void OnError()
        {
            _watch.Stop();
            _err = _watch.ElapsedMilliseconds;
            _watch.Restart();
        }

        /// <summary>
        /// 请求完成
        /// </summary>
        public void OnEnd()
        {
            _watch.Stop();
            _end = _watch.ElapsedMilliseconds;
        }


        /// <summary>
        /// 返回当前计时器的记录值。
        /// </summary>
        public override string ToString()
        {
            if (_send >= 0)
            {
                return $"ready:{_ready} ms, send:{_send} ms, end:{_end} ms";
            }
            if (_err >= 0)
            {
                return $"ready:{_ready} ms, err:{_err} ms, end:{_end} ms";
            }
            return $"ready:{_ready} ms";
        }
    }
}