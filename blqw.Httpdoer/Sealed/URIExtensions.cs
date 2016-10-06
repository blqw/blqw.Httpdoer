using System;

namespace blqw.Web
{
    /// <summary>
    /// 一组用于处理 URI 的扩展方法
    /// </summary>
    public static class URIExtensions
    {
        /// <summary>
        /// 拼接2个url
        /// </summary>
        /// <param name="baseUrl"> 基础url </param>
        /// <param name="newUrl"> 新的url </param>
        /// <returns> </returns>
        public static Uri Combine(this Uri baseUrl, string newUrl)
        {
            if ((newUrl == null) && (baseUrl == null))
            {
                return null;
            }

            Uri url;
            if (baseUrl == null)
            {
                if (Uri.TryCreate(newUrl, UriKind.Absolute, out url) == false)
                {
                    throw new UriFormatException($"{nameof(newUrl)}错误:不是url");
                }
                return url;
            }
            if (string.IsNullOrWhiteSpace(newUrl))
            {
                return baseUrl;
            }
            if (Uri.TryCreate(baseUrl, newUrl, out url))
            {
                return url;
            }
            throw new UriFormatException($"{nameof(baseUrl)} + {nameof(newUrl)}错误:不是url");
        }
    }
}