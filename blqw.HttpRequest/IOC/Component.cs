using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace blqw.HttpRequestComponent
{
    /// <summary> 组件/插件 等待IOC注入
    /// </summary>
    class Component
    {
        static Component()
        {
            MEFPart.Import(typeof(Component));
        }

        [Import]
        public readonly static IFormatterConverter Converter = new FormatterConverter();
        
        static readonly JavaScriptSerializer JSON = new JavaScriptSerializer();
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
        public readonly static Func<object, string> ToJsonString =
            delegate (object obj)
            {
                return JSON.Serialize(obj);
            };


    }
}
