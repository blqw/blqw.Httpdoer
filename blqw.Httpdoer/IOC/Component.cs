using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace blqw.IOC
{
    /// <summary> 组件/插件 等待IOC注入
    /// </summary>
    class Components
    {
        static Components()
        {
            MEFLite.Import(typeof(Components));
        }

        /// <summary>
        /// 获取转换器
        /// </summary>
        [Import]
        static readonly IFormatterConverter Converter;

        public static string ToString(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return Converter?.ToString(obj) ?? obj + "";
        }

        static readonly JavaScriptSerializer JSON = new JavaScriptSerializer();
        
        public static object ToJsonObject1(Type type, string json)
        {
            return JSON.Deserialize(json, type);
        }

        static readonly ConcurrentDictionary<Type, DataContractJsonSerializer> _JsonSerializer = new ConcurrentDictionary<Type, DataContractJsonSerializer>();

        /// <summary> 用于将Json字符串转为实体对象的方法
        /// </summary>
        [Import("ToJsonObject")]
        public readonly static Func<Type, string, object> ToJsonObject =
            delegate (Type type, string json)
            {
                return JSON.Deserialize(json, type);
            };

        /// <summary> 用于将Json字符串转为实体对象的方法
        /// </summary>
        [Import("ToJsonString")]
        public readonly static Func<object, string> ToJsonString = JSON.Serialize;

    }
}
