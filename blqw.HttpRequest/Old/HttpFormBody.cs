using blqw.HttpRequestComponent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace blqw.Web
{
    /// <summary> 用于表示表单参数
    /// </summary>
    /// <remarks>周子鉴 2015.08.01</remarks>
    public class HttpFormBody : HttpParameterCollection
    {
        /// <summary> 运算符 加号(+) 重载,实现追加参数的语法糖
        /// </summary>
        /// <param name="frombody">需要追加参数的表单参数集</param>
        /// <param name="param">加载到参数集的实体参数</param>
        /// <returns></returns>
        /// <remarks>周子鉴 2015.08.01</remarks>
        public static HttpFormBody operator +(HttpFormBody frombody, object param)
        {
            if (frombody == null) return null;
            frombody.Add(param);
            return frombody;
        }

        /// <summary> 根据当前表单参数的类型设定请求头 ,默认字符集UTF8
        /// </summary>
        /// <param name="request">http请求</param 
        /// <remarks>周子鉴 2015.08.01</remarks>
        public void SetHeaders(HttpWebRequest request)
        {
            SetHeaders(request, Encoding.UTF8);
        }
        /// <summary> 根据当前表单参数的类型设定请求头
        /// </summary>
        /// <param name="request">http请求</param>
        /// <param name="encoding">字符集</param>
        /// <remarks>周子鉴 2016.01.21</remarks>
        public void SetHeaders(HttpWebRequest request, Encoding encoding)
        {
            if (HasData
                && string.IsNullOrWhiteSpace(request.ContentType)
                && request.Headers["Content-Type"] == null)
            {
                switch (ContentType)
                {
                    case Web.ContentType.ApplicationJson:
                        request.ContentType = ContentType + ";charset=" + encoding.WebName;
                        break;
                    default:
                        request.ContentType = ContentType ?? "application/x-www-form-urlencoded";
                        break;
                }
            }
            Trace.WriteLine(request.ContentType, "HttpRequest.ContentType");
        }

        /// <summary> 是否有数据
        /// </summary>
        public bool HasData
        {
            get
            {
                return Count > 0 || _buffer != null;
            }
        }

        /// <summary> 设定头
        /// </summary>
        public string ContentType { get; set; }
        
        /// <summary> 返回表单参数的字节
        /// </summary>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public byte[] GetBytes(Encoding encoding)
        {
            if (_buffer != null)
            {
                return _buffer;
            }
            switch (ContentType)
            {
                case Web.ContentType.ApplicationJson:
                    var json = Component.ToJsonString(this);
                    Trace.WriteLine(json, "HttpRequest.FormBody"); //打印日志
                    return encoding.GetBytes(json);
                //case Web.ContentType.ApplicationXML:
                //    using (var stream = new MemoryStream())
                //    {
                //        _XmlSerializer.Serialize(stream, this);
                //        bytes = stream.GetBuffer();
                //        Trace.WriteLine(encoding.GetString(bytes), "HttpRequest.FormBody"); //打印日志
                //        return bytes;
                //    }
                default:
                    var str = this.ToString();
                    if (string.IsNullOrWhiteSpace(str) == false)
                    {
                        Trace.WriteLine(str, "HttpRequest.FormBody"); //打印日志
                    }
                    return encoding.GetBytes(str);
            }
            
        }

        Byte[] _buffer;
        /// <summary> 写入流数据
        /// </summary>
        /// <param name="bytes"></param>
        public void Write(byte[] bytes)
        {
            if (Count != 0)
            {
                throw new NotImplementedException("当前无法写入");
            }
            if (_buffer != null)
            {
                throw new NotImplementedException("正文已存在");
            }
            IsReadOnly = true;
            _buffer = bytes;
        }

    }
}
