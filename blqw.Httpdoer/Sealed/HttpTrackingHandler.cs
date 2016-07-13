using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 请求动作的跟踪事件委托
    /// </summary>
    /// <param name="request"></param>
    public delegate void HttpTrackingHandler(IHttpRequest request);
    public delegate void HttpResponseTrackingHandler(IHttpRequest request,IHttpResponse response);
    public delegate void HttpParamFoundTrackingHandler<T>(IHttpRequest request, ref string name,ref T value);
}
 