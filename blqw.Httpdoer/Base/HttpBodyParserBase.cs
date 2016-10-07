using System;
using System.Collections.Generic;
using System.Text;
using blqw.Reflection;

namespace blqw.Web
{
    /// <summary>
    /// Body解析器的抽象基类
    /// </summary>
    public abstract class HttpBodyParserBase : IHttpBodyParser
    {
        /// <summary>
        /// 将字节流转换为键值对枚举
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public abstract IEnumerable<KeyValuePair<string, object>> Deserialize(byte[] bytes,
            IFormatProvider formatProvider);

        /// <summary>
        /// 匹配解析器,返回 true 表示匹配成功
        /// </summary>
        /// <param name="type"> 类型 </param>
        /// <param name="format"> 格式 </param>
        /// <returns></returns>
        public abstract bool IsMatch(string type, string format);

        /// <summary>
        /// 将正文格式化为字节流
        /// </summary>
        /// <param name="format"> 包含格式规范的格式字符串 </param>
        /// <param name="body"> 请求或响应正文 </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public abstract byte[] Serialize(string format, IEnumerable<KeyValuePair<string, object>> body,
            IFormatProvider formatProvider);

        /// <summary>
        /// 将字节流转换为指定对象
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="formatProvider"> 它提供有关当前实例的格式信息 </param>
        /// <returns> </returns>
        public virtual T Deserialize<T>(byte[] bytes, IFormatProvider formatProvider)
        {
            try
            {
                var instance = Activator.CreateInstance<T>();
                var props = PropertyHandlerCollection.Get(typeof(T));
                var kv = Deserialize(bytes, formatProvider);
                if (kv == null)
                {
                    throw new NotSupportedException("无法转为实体");
                }
                foreach (var o in kv)
                {
                    props[o.Key]?.SetValue(instance, o.Value);
                }
                return instance;
            }
            catch (Exception ex)
            {
                if (bytes?.Length > 0)
                {
                    var charset = formatProvider?.GetFormat(typeof(Encoding)) as Encoding;
                    if (charset == null)
                    {
                        ex.Data["ResponseBody"] = ex.Source = "base64:" + Convert.ToBase64String(bytes);
                    }
                    else
                    {
                        ex.Data["ResponseBody"] = ex.Source = charset.GetString(bytes);
                    }
                }
                throw;
            }
        }

        /// <summary>
        /// 将正文内容(<paramref name="arg" />)按照<paramref name="formatProvider" />中的编码信息转为等效的字符串
        /// </summary>
        /// <exception cref="FormatException"> <paramref name="arg" />必须是 IEnumerable&lt;KeyValuePair&lt;string, object&gt;&gt;. </exception>
        public virtual string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                return null;
            }
            var body = arg as IEnumerable<KeyValuePair<string, object>>;
            if (body == null)
            {
                throw new FormatException(nameof(arg) + "必须是" + nameof(IEnumerable<KeyValuePair<string, object>>));
            }
            var bytes = Serialize(format, body, formatProvider);
            if (bytes?.Length > 0)
            {
                var charset = formatProvider?.GetFormat(typeof(Encoding)) as Encoding ?? Encoding.Default;
                return charset.GetString(bytes);
            }
            return null;
        }

        /// <summary>
        /// 从 <paramref name="formatProvider" /> 中获取编码信息
        /// </summary>
        /// <returns> </returns>
        protected static Encoding GetEncoding(IFormatProvider formatProvider)
            => formatProvider?.GetFormat(typeof(Encoding)) as Encoding;
    }
}