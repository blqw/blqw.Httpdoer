using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary> 表示Url参数集合
    /// </summary>
    /// <remarks>周子鉴 2015.08.01</remarks>
    public class HttpQueryString : HttpParameterCollection
    {
        /// <summary> 运算符 加号(+) 重载,实现追加参数的语法糖
        /// </summary>
        /// <param name="frombody">需要追加参数的URL参数集</param>
        /// <param name="param">追加到URL参数集的实体参数</param>
        /// <returns></returns>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public static HttpQueryString operator +(HttpQueryString queryString, object param)
        {
            queryString.Add(param);
            return queryString;
        }
    }
}
