namespace blqw.Web
{
    /// <summary>
    /// 请求动作的跟踪事件委托
    /// </summary>
    /// <param name="request"> 请求 </param>
    public delegate void HttpTrackingHandler(IHttpRequest request);

    /// <summary>
    /// 请求动作的跟踪响应事件委托
    /// </summary>
    /// <param name="request"> 请求 </param>
    /// <param name="response"> 响应 </param>
    public delegate void HttpResponseTrackingHandler(IHttpRequest request, IHttpResponse response);

    /// <summary>
    /// 请求动作的跟踪参数事件委托
    /// </summary>
    /// <typeparam name="T"> 参数类型 </typeparam>
    /// <param name="request"> 请求 </param>
    /// <param name="name"> 参数名 </param>
    /// <param name="value"> 参数值 </param>
    public delegate void HttpParamFoundTrackingHandler<T>(IHttpRequest request, ref string name, ref T value);
}