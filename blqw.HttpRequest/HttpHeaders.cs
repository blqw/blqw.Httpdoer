using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary> 表示请求头
    /// </summary>
    /// <remarks>周子鉴 2015.08.01</remarks>
    public class HttpHeaders : HttpParameterCollection
    {
        static readonly Action<WebHeaderCollection, string, string> AddInternal = (Action<WebHeaderCollection,string, string>)(typeof(WebHeaderCollection).GetMethod(
                    "AddInternal",
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new Type[] { typeof(string), typeof(string) },
                    null) ??
                    typeof(WebHeaderCollection).GetMethod(
                    "AddWithoutValidate",
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    new Type[] { typeof(string), typeof(string) },
                    null)).CreateDelegate(typeof(Action<WebHeaderCollection, string, string>));

        private static Dictionary<string, HttpResponseHeader> GetSystemHeaders()
        {
            var dict = new Dictionary<string, HttpResponseHeader>(StringComparer.OrdinalIgnoreCase);

            foreach (HttpResponseHeader header in Enum.GetValues(typeof(HttpResponseHeader)))
            {
                dict.Add(header.ToString("g"), header);
            }

            return dict;
        }


        /// <summary> 运算符 加号(+) 重载,实现追加参数的语法糖
        /// </summary>
        /// <param name="headers">需要追加参数的请求头</param>
        /// <param name="param">追加到请求头的实体参数</param>
        /// <returns></returns>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public static HttpHeaders operator +(HttpHeaders headers, object param)
        {
            if (headers == null) return null;

            headers.Add(param);
            return headers;
        }

        /// <summary> 设置请求头
        /// </summary>
        /// <param name="request">http请求</param>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public void SetHeaders(HttpWebRequest request)
        {
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            var useragent = this["UserAgent"];
            if (useragent == null)
            {
                request.UserAgent = "Top.Framework/3.5.0.0";
            }
            else
            {
                request.UserAgent = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.UTF8.GetBytes(useragent)) ?? "Top.Framework/3.5.0.0";
            }
            request.Headers["Accept-Encoding"] = "gzip, deflate, sdch";
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8";
            request.Headers["Cache-Control"] = "max-age=0";

            foreach (string name in this.Keys)
            {
                if (name.Equals("UserAgent", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                AddInternal(request.Headers, name, this[name]);
            }
        }

    }
}
