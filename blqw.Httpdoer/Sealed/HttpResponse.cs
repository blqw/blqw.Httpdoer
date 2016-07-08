using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    internal sealed class HttpResponse : IHttpResponse
    {
        public HttpBody Body { get; set; }
        public CookieCollection Cookies { get; set; }
        public Exception Exception { get; set; }
        public HttpHeaders Headers { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string Status { get; set; }
        public string Version { get; set; }
        public HttpRequestData RequestData { get; set; }
        
        public string ResponseRaw
        {
            get
            {
                return $"{Version} {(int)StatusCode} {Status}\r\n{string.Join("\r\n", GetAllHeaders())}\r\n\r\n{Body.ToString()}" ;
            }
        }

        public HttpStatusCode StatusCode { get; set; }

        private IEnumerable<string> GetAllHeaders()
        {
            foreach (var header in Headers)
            {
                var arr = header.Value as IEnumerable;
                if (arr!=null && arr is string == false )
                {
                    foreach (var value in arr)
                    {
                        yield return $"{header.Key}: {value}";
                    }
                }
                else
                {
                    yield return $"{header.Key}: {header.Value}";
                }
            }
        } 
        
    }
}
