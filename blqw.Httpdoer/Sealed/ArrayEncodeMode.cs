using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 数组编码模式
    /// </summary>
    public enum ArrayEncodeMode
    {
        /// <summary>
        /// a=x&amp;a=y&amp;a=z
        /// </summary>
        Default = 0,
        /// <summary>
        /// a=x,y,z
        /// </summary>
        Merge = 1,
        /// <summary>
        /// a[]=x&amp;a[]=y&amp;a[]=z
        /// </summary>
        JQuery = 2,
        /// <summary>
        /// a=["x","y","z"]
        /// </summary>
        Json = 3,
        /// <summary>
        /// a[0]=x&amp;a[1]=y&amp;a[2]=z
        /// </summary>
        Asp = 4
    }
}
