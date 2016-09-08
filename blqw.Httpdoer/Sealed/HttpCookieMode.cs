using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// Cookie模式
    /// </summary>
    [Flags]
    public enum HttpCookieMode
    {
        /// <summary>
        /// 不使用 Cookie
        /// </summary>
        None = 0,
        /// <summary>
        /// 使用应用程序缓存的Cookie
        /// </summary>
        ApplicationCache = 1,
        /// <summary>
        /// 使用用户自定义Cookie
        /// </summary>
        UserCustom = 2,
        /// <summary>
        /// 使用自定义或缓存Cookie(自定义优先)
        /// </summary>
        CustomOrCache = 3,
    }
}
