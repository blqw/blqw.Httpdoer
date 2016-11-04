namespace blqw.Web
{
    /// <summary>
    /// 表示 HTTP 请求的方式
    /// </summary>
    public enum HttpRequestMethod
    {
        /// <summary>
        /// GET
        /// </summary>
        Get,

        /// <summary>
        /// POST
        /// </summary>
        Post,

        /// <summary>
        /// HEAD
        /// </summary>
        Head,

        /// <summary>
        /// TRACE
        /// </summary>
        Trace,

        /// <summary>
        /// PUT
        /// </summary>
        Put,

        /// <summary>
        /// DELETE
        /// </summary>
        Delete,

        /// <summary>
        /// OPTIONS
        /// </summary>
        Options,

        /// <summary>
        /// CONNECT
        /// </summary>
        Connect,

        /// <summary>
        /// PATCH
        /// </summary>
        Patch,

        /// <summary>
        /// 自定义
        /// </summary>
        Custom
    }
}