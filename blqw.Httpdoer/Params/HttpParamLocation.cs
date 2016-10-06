namespace blqw.Web
{
    /// <summary>
    /// 参数位置
    /// </summary>
    public enum HttpParamLocation
    {
        /// <summary>
        /// 参数在 url 的 ? 之后
        /// </summary>
        Query = 1,

        /// <summary>
        /// 参数在请求正文中
        /// </summary>
        Body = 2,

        /// <summary>
        /// 参数在 url 的路径中
        /// </summary>
        Path = 3,

        /// <summary>
        /// 参数在请求头中
        /// </summary>
        Header = 4,

        /// <summary>
        /// 自动匹配
        /// </summary>
        Auto = 5
    }
}