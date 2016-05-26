using System;
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
        public CookieContainer Cookie { get; set; }
        public Exception Exception { get; set; }
        public HttpHeaders Headers { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public HttpResponse WriteLog()
        {
            if (IsSuccessStatusCode == false)
            {
                Trace.WriteLine((int)StatusCode, "HttpRequest.StatusCode");
            }
            if (Exception != null)
            {
                Trace.WriteLine(Exception.Message, "HttpRequest.Error");
            }
            return this;
        }
    }
}
