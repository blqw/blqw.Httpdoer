using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 表示发送请求的数据类型
    /// </summary>
    public static class ContentType
    {
        /// <summary>
        /// Body数据类型 application/json 
        /// </summary>
        public const string ApplicationJson = "application/json";
        /// <summary>
        /// Body数据类型 application/xml 
        /// </summary>
        [Obsolete("还不支持!", true)]
        public const string ApplicationXML = "application/xml";
        /// <summary>
        /// Body数据类型 multipart/form-data
        /// </summary>
        [Obsolete("还不支持!", true)]
        public const string MultipartFormData = "multipart/form-data";
        /// <summary>
        /// Body数据类型 application/protobuf 
        /// </summary>
        [Obsolete("还不支持!", true)]
        public const string ApplicationProtobuf = "application/protobuf";
    }
}
