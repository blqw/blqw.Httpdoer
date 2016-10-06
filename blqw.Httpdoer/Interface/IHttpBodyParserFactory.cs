namespace blqw.Web
{
    /// <summary>
    /// Body 解析器工厂
    /// </summary>
    public interface IHttpBodyParserFactory
    {
        /// <summary>
        /// 创建一个解析器
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="format"> 格式 </param>
        /// <returns> </returns>
        IHttpBodyParser Create(string type, string format);
    }
}