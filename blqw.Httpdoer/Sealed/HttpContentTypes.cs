namespace blqw.Web
{
    /// <summary>
    /// 系统定义的 HTTP 正文内容类型枚举
    /// </summary>
    public enum HttpContentTypes
    {
        /// <summary>
        /// null
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        Form = 1,

        /// <summary>
        /// application/json;charset=utf-8
        /// </summary>
        Json = 2,

        /// <summary>
        /// application/octet-stream
        /// </summary>
        OctetStream = 3,

        /// <summary>
        /// text/xml;charset=utf-8
        /// </summary>
        XML = 4,

        /// <summary>
        /// application/x-protobuf
        /// </summary>
        Protobuf = 5,

        /// <summary>
        /// text/plain;charset=utf-8
        /// </summary>
        UTF8Text = 6,

        /// <summary>
        /// text/plain
        /// </summary>
        Text = 7
    }
}