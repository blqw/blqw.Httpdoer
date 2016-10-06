using System;
using System.Collections;
using System.Net;
using System.Text;

namespace blqw.Web
{
    /// <summary>
    /// 表示一个 HTTP 响应
    /// </summary>
    internal sealed class HttpResponse : IHttpResponse
    {
        /// <summary>
        /// 回车换行符
        /// </summary>
        private const string CRLF = "\r\n";

        [ThreadStatic]
        private static StringBuilder _Buffer;

        /// <summary>
        /// 响应状态说明
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 请求方案/版本 (Scheme.ToUpperInvariant()/Version;)
        /// </summary>
        public string SchemeVersion { get; set; }

        /// <summary>
        /// 响应正文
        /// </summary>
        public HttpBody Body { get; set; }

        /// <summary>
        /// Cookie
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// 响应异常
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 响应头
        /// </summary>
        public HttpHeaders Headers { get; set; }


        /// <summary>
        /// 获取一个值，该值指示 HTTP 响应是否成功。StatusCode 在 200-299 范围中，则为 true；否则为 false
        /// </summary>
        public bool IsSuccessStatusCode { get; set; }

        public HttpRequestData RequestData { get; set; }

        /// <summary>
        /// 返回的原始数据
        /// </summary>
        public string ResponseRaw => $"{SchemeVersion} {(int) StatusCode} {Status}{CRLF}{HeadersDebugInfo()}{CRLF}{CRLF}{Body}";

        /// <summary>
        /// HTTP 响应的状态代码
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        private string HeadersDebugInfo()
        {
            if (Headers == null)
            {
                return string.Empty;
            }
            if (_Buffer == null)
            {
                _Buffer = new StringBuilder();
            }
            else
            {
                _Buffer.Clear();
            }
            // ReSharper disable once GenericEnumeratorNotDisposed
            var ee = Headers.GetEnumerator();

            while (ee.MoveNext())
            {
                var header = ee.Current;
                var arr = header.Value as IEnumerable;
                if ((arr != null) && (arr is string == false))
                {
                    foreach (var value in arr)
                    {
                        _Buffer.Append(header.Key);
                        _Buffer.Append(": ");
                        _Buffer.Append(value);
                        _Buffer.Append(CRLF);
                    }
                }
                else
                {
                    _Buffer.Append(header.Key);
                    _Buffer.Append(": ");
                    _Buffer.Append(header.Value);
                    _Buffer.Append(CRLF);
                }
            }

            if (_Buffer.Length == 0)
            {
                return string.Empty;
            }

            var str = _Buffer.ToString();
            _Buffer.Clear();
            return str;
        }
    }
}