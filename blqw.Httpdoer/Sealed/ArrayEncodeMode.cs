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
        /// a=1&amp;a=2&amp;a=3
        /// </summary>
        Default = 0,
        /// <summary>
        /// a=1,2,3
        /// </summary>
        Merge = 1,
        /// <summary>
        /// a[]=1&amp;a[]=2&amp;a[]=3
        /// </summary>
        JQuery = 2,
        /// <summary>
        /// a=[1,2,3]
        /// </summary>
        Json = 3,
        /// <summary>
        /// a[0]=1&amp;a[1]=2&amp;a[2]=3
        /// </summary>
        Asp = 4
    }
}
