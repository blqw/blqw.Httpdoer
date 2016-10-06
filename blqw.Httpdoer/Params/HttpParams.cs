namespace blqw.Web
{
    /// <summary>
    /// 表示自动匹配的参数集合
    /// </summary>
    public sealed class HttpParams : HttpParamsBase<object>
    {
        /// <summary>
        /// 初始化参数集合
        /// </summary>
        /// <param name="params"></param>
        internal HttpParams(IHttpParameterContainer @params)
            : base(@params, HttpParamLocation.Auto)
        {
        }
    }
}