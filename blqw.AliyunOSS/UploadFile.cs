using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary> 描述一个待上传的文件
    /// </summary>
    public struct UploadFile
    {
        /// <summary>
        /// 静态文件存储节点,目前可选有 top-image
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// 要上传的内容
        /// </summary>
        public Byte[] Content { get; set; }

        /// <summary>
        /// 静态文件全路径
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 以文件流的形式设置上传内容
        /// </summary>
        /// <param name="stream"></param>
        public void SetContent(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            Content = ReadBytes(stream).ToArray();
        }

        /// <summary>
        /// 以字符串的形式设置上传内容
        /// </summary>
        /// <param name="text"></param>
        public void SetContent(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            Content = Encoding.UTF8.GetBytes(text);
        }

        /// <summary> 读取流中的所有字节
        /// </summary>
        /// <param name="stream"></param>
        static IEnumerable<byte> ReadBytes(Stream stream)
        {
            int length = 1024;
            byte[] buffer = new byte[length];
            int index = 0;
            while ((index = stream.Read(buffer, 0, length)) > 0)
            {
                for (int i = 0; i < index; i++)
                {
                    yield return buffer[i];
                }
            }
        }

        /// <summary> 读取流中的所有字节
        /// </summary>
        /// <param name="stream"></param>
        static byte[] GetBytes(Stream stream)
        {
            return ReadBytes(stream).ToArray();
        }

        /// <summary>
        /// 开始上传
        /// </summary>
        /// <returns></returns>
        public Task<string> Upload()
        {
            if (string.IsNullOrWhiteSpace(Bucket)) throw new ArgumentNullException("Bucket");
            if (string.IsNullOrWhiteSpace(FullName)) throw new ArgumentNullException("FullName");
            if (Content == null) throw new ArgumentNullException("Content");

            if (Bucket.Equals("top-configs", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException("不支持操作该节点");
            }

            var host = string.Format("{0}.oss-cn-hangzhou{1}.aliyuncs.com", Bucket, OSS.AliyunECS ? "-internal" : "");
            var www = new HttpRequest(host);
            www.Method = HttpRequestMethod.PUT;
            www.Path = FullName;
            www.Headers["Content-Type"] = ContentType ?? "application/octet-stream";
            www.Headers["Content-Length"] = "-1";
            www.Headers["Date"] = DateTime.Now.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T", CultureInfo.InvariantCulture);
            www.Headers["Host"] = host;
            //www.Headers["Expect"] = "100-continue";

            www.FormBody.Write(Content);
            OssSigner.Sign(www);
            return www.GetString();
        }
    }
}
