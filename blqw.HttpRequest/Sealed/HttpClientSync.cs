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
    sealed class HttpClientSync : IHttpClient
    {
        static HttpClientSync()
        {
            //用于https的请求验证票据
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
        }



        public IHttpResponse Send(IHttpRequest request)
        {
            var data = new HttpRequestData(request);

            Trace.WriteLine(data.Url.ToString(), "HttpRequest.Url");
            var www = WebRequest.CreateHttp(data.Url);
            if (request.Version != null)
            {
                www.ProtocolVersion = request.Version;
            }
            else if (data.Url.Scheme == Uri.UriSchemeHttps)
            {
                www.ProtocolVersion = HttpVersion.Version10;
            }
            www.CookieContainer = request.Cookie;
            www.ContinueTimeout = 3000;
            www.ReadWriteTimeout = 3000;
            www.Timeout = (int)request.Timeout.TotalMilliseconds;
            

            throw new NotImplementedException();
        }


        public IAsyncResult BeginSend(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public IHttpResponse EndSend(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }















        public Task<IHttpResponse> SendAsync(IHttpRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IHttpResponse> SendAsync(IHttpRequest request, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public Task<IHttpResponse> SendAsync(IHttpRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
