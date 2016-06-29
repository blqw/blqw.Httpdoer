using blqw.HttpRequestComponent;
using blqw.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace blqw.Web
{
    /// <summary>
    /// 一组操作 Http 参数的拓展方法
    /// </summary>
    public static class HttpParamsExtensions
    {
        /// <summary>
        /// 追加 Url 查询参数字符串到 Http 参数
        /// </summary>
        /// <param name="param"> 参数将被追加到的参数集合 </param>
        /// <param name="query"> 需要解析的 Url 查询参数字符串 </param>
        public static void AddQuery(this HttpParamsBase<string> param, string query)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }
            var nv = HttpUtility.ParseQueryString(query);
            for (int i = 0, length = nv.Count; i < length; i++)
            {
                var key = nv.GetKey(i);
                var val = nv[i];
                param[key] = val;
            }
        }

        /// <summary>
        ///  追加 Url 查询参数字符串到 Http 参数
        /// </summary>
        /// <param name="param"> 参数将被追加到的参数集合 </param>
        /// <param name="query"> 需要解析的 Url 查询参数字符串 </param>
        public static void AddQuery(this HttpParamsBase<object> param, string query)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }
            var nv = HttpUtility.ParseQueryString(query);
            for (int i = 0, length = nv.Count; i < length; i++)
            {
                var key = nv.GetKey(i);
                var val = nv[i];
                param[key] = val;
            }
        }

        /// <summary>
        /// 追加 Json 对象到 Http 参数
        /// </summary>
        /// <param name="param"> 参数将被追加到的参数集合 </param>
        /// <param name="json"> 需要解析的 Json 字符串 </param>
        public static void AddJson(this HttpParamsBase<string> param, string json)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            var nv = Component.ToJsonObject(typeof(NameValueCollection), json)
                        as NameValueCollection;
            for (int i = 0, length = nv.Count; i < length; i++)
            {
                var key = nv.GetKey(i);
                var val = nv[i];
                param[key] = val;
            }
        }

        /// <summary>
        /// 追加 Json 对象到 Http 参数
        /// </summary>
        /// <param name="param"> 参数将被追加到的参数集合 </param>
        /// <param name="json"> 需要解析的 Json 字符串 </param>
        public static void AddJson(this HttpParamsBase<object> param, string json)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            if (string.IsNullOrWhiteSpace(json))
            {
                return;
            }

            var dict = Component.ToJsonObject(typeof(Dictionary<string, object>), json)
                        as Dictionary<string, object>;
            foreach (var item in dict)
            {
                param[item.Key] = item.Value;
            }
        }

        /// <summary>
        /// 追加实体对象到 Http 参数
        /// </summary>
        /// <param name="param"> 参数将被追加到的参数集合 </param>
        /// <param name="model"> 需要追加到 Http 参数的实体对象 </param>
        public static void AddModel(this HttpParamsBase<string> param, object model)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            if (model == null)
            {
                return;
            }
            var props = PropertyGetter.Get(model.GetType());
            for (int i = 0, length = props.Count; i < length; i++)
            {
                var p = props[i];
                param[p.Name] = Component.ToString(p.GetValue(model));
            }
        }

        /// <summary>
        /// 追加实体对象到 Http 参数
        /// </summary>
        /// <param name="param"> 参数将被追加到的参数集合 </param>
        /// <param name="model"> 需要追加到 Http 参数的实体对象 </param>
        public static void AddModel(this HttpParamsBase<object> param, object model)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }
            if (model == null)
            {
                return;
            }
            var props = PropertyGetter.Get(model.GetType());
            for (int i = 0, length = props.Count; i < length; i++)
            {
                var p = props[i];
                param[p.Name] = p.GetValue(model);
            }
        }
    }
}
