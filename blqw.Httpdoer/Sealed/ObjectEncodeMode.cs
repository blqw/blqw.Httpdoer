using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 对象编码模式
    /// </summary>
    public enum ObjectEncodeMode
    {
        /// <summary>
        /// user.Id=1&amp;user.Name=blqw
        /// </summary>
        Default = 0,
        /// <summary>
        /// Id=1&amp;Name=blqw
        /// </summary>
        NameOnly = 1,
        /// <summary>
        /// user[Id]=1&amp;user[Name]=blqw
        /// </summary>
        JQuery = 2,
        /// <summary>
        /// user={"Id":1,"Name":"blqw"}
        /// </summary>
        Json = 3
    }
}
