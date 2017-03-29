using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 表示Url中的参数
    /// </summary>
    public sealed class HttpQuery : HttpStringParams
    {
        public HttpQuery(IHttpParameterContainer @params, HttpParamLocation location)
            : base(@params, location)
        {
            ArrayEncodeMode = ArrayEncodeMode.Default;
            ObjectEncodeMode = ObjectEncodeMode.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        public ArrayEncodeMode ArrayEncodeMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObjectEncodeMode ObjectEncodeMode { get; set; }
    }
}
