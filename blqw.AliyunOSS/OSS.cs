using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using System.Configuration;

namespace blqw.Web
{
    public partial class OSS
    {
        public static bool IsInitialized { get; } = Initialize();

        private static bool Initialize()
        {
            CacheMinutes = 5;
            InitializeDefaultValue();
            var settings = ConfigurationManager.AppSettings;
            var aliyunID = settings["AliyunID"];
            var aliyunSecret = settings["AliyunSecret"];
            var aliyunECS = settings["AliyunECS"];
            if (aliyunID != null)
            {
                AliyunID = aliyunID;
            }
            if (aliyunSecret != null)
            {
                AliyunSecret = aliyunSecret;
            }
            if (aliyunECS != null)
            {
                AliyunECS = "true".Equals(aliyunECS, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }

        static partial void InitializeDefaultValue();

        /// <summary> 阿里云ID
        /// </summary>
        public static string AliyunID { get; set; }
        /// <summary> 阿里云密钥
        /// </summary>
        public static string AliyunSecret { get; set; }
        /// <summary> 是否部署在阿里云ECS上
        /// </summary>
        public static bool AliyunECS { get; set; }
        /// <summary> 缓存时间(分钟)
        /// </summary>
        public static int CacheMinutes { get; set; }

        private readonly static ConcurrentDictionary<string, byte[]> _ossFiles = new ConcurrentDictionary<string, byte[]>();
        private static ConcurrentDictionary<string, byte[]> OSSFiles
        {
            get
            {
                if (OSSFileUpdate < DateTime.Now)
                {
                    _ossFiles.Clear();
                    OSSFileUpdate = DateTime.Now.AddMinutes(CacheMinutes);
                }
                return _ossFiles;
            }
        }

        private static DateTime OSSFileUpdate;

        /// <summary> 尝试检查AppCode和AppKey是否有值
        /// </summary>
        /// <remarks>周子鉴 2015.08.01</remarks>
        internal static void TryThrowNullException()
        {
            if (string.IsNullOrWhiteSpace(AliyunID))
                throw new ArgumentNullException("AliyunID");
            if (string.IsNullOrWhiteSpace(AliyunSecret))
                throw new ArgumentNullException("AliyunSecret");
        }

        private static async Task<byte[]> GetOrAdd(string key, Func<HttpRequest> request)
        {
            TryThrowNullException();
            var files = OSSFiles;
            byte[] value;
            if (files.TryGetValue(key, out value))
            {
                return value;
            }
            var www = request();
            value = await www.GetBytes();
            if (www.Exception != null)
            {
                throw www.Exception;
            }
            if (www.ResponseCode == HttpStatusCode.OK)
            {
                files.TryAdd(key, value);
            }
            return value;
        }

        /// <summary> 获取阿里云OSS私有节点的文本文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="bucket">Bucket名称</param>
        /// <returns></returns>
        public static async Task<string> GetPrivateText(string name, string bucket = "top-configs")
        {
            return Encoding.UTF8.GetString(
                await GetOrAdd(bucket + "&" + name, () =>
                                GetOssSafeRequest(name, bucket, true))
            );
        }

        /// <summary> 获取那里云私有节点的文件数据
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="bucket">Bucket名称</param>
        /// <returns></returns>
        public static async Task<Byte[]> GetPrivateFile(string name, string bucket = "top-configs")
        {
            return await GetOrAdd(bucket + ">" + name, () =>
            {
                return GetOssSafeRequest(name, bucket, true);
            });
        }

        /// <summary> 获取阿里云OSS的文本文件
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="bucket">Bucket名称</param>
        /// <returns></returns>
        public static async Task<string> GetText(string name, string bucket)
        {
            return Encoding.UTF8.GetString(await GetOrAdd(bucket + "&" + name, () =>
            {
                return GetOssSafeRequest(name, bucket, false);
            }));
        }

        /// <summary> 获取阿里云OSS的文件流
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <param name="bucket">Bucket名称</param>
        /// <returns></returns>
        public static async Task<Byte[]> GetFile(string name, string bucket)
        {
            return await GetOrAdd(bucket + "&" + name, () =>
            {
                return GetOssSafeRequest(name, bucket, false);
            });
        }

        /// <summary> 获取阿里云OSS文件流
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bucket"></param>
        /// <param name="isPrivate"></param>
        /// <returns></returns>
        private static HttpRequest GetOssSafeRequest(string name, string bucket, bool isPrivate)
        {
            Trace.WriteLine(bucket + "/" + name, "RequestOSS");
            TryThrowNullException();
            var www = new HttpRequest(string.Format("http://{0}.oss-cn-hangzhou{2}.aliyuncs.com/{1}", bucket, name, AliyunECS ? "-internal" : ""));
            if (isPrivate == false)
            {
                return www;
            }
            www.QueryString.Add("OSSAccessKeyId", AliyunID);
            www.QueryString.Add("Expires", (3600 + (int)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds).ToString());
            using (var algorithm = KeyedHashAlgorithm.Create("HMACSHA1"))
            {
                algorithm.Key = Encoding.UTF8.GetBytes(AliyunSecret);
                //GET\n\n\n{expires}\n/{bucket}/{name}  //加密数据格式
                var data = "GET\n\n\n" + www.QueryString["Expires"] + "\n/" + bucket + "/" + name;
                var sign = Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(data)));
                www.QueryString.Add("Signature", sign);
            }
            return www;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bucket"></param>
        /// <returns></returns>
        public static Task DeleteFile(string name, string bucket)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(bucket)) throw new ArgumentNullException("bucket");
            if (bucket.Equals("top-configs", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotSupportedException("不支持操作该节点");
            }

            const string xml = @"<?xml version=""1.0""?>
<Delete>
  <Quiet>true</Quiet>
  <Object>
    <Key>{0}</Key>
  </Object>
</Delete>";

            var content = Encoding.UTF8.GetBytes(string.Format(xml, name));

            var host = string.Format("{0}.oss-cn-hangzhou{1}.aliyuncs.com", bucket, OSS.AliyunECS ? "-internal" : "");
            var www = new HttpRequest(host);
            www.Method = HttpRequestMethod.POST;
            www.Path = "?delete";
            www.Headers["Date"] = DateTime.Now.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T", System.Globalization.CultureInfo.InvariantCulture);
            www.Headers["Content-Type"] = "";
            www.Headers["Content-Length"] = content.Length.ToString();
            www.Headers["Content-MD5"] = MD5Content(content);

            www.FormBody.Write(content);
            OssSigner.Sign(www);
            Trace.WriteLine(bucket + "/" + name, "删除OSS文件");
            return www.GetString();

        }

        /// <summary>
        /// MD5内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string MD5Content(byte[] content)
        {
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(content);
                var charset = DefaultBaseChars;
                var sBuilder = new StringBuilder();
                foreach (var b in data)
                {
                    sBuilder.Append(charset[b >> 4]);
                    sBuilder.Append(charset[b & 0x0F]);
                }

                return Convert.ToBase64String(data);
            }
        }

        static readonly char[] DefaultBaseChars = "0123456789ABCDEF".ToCharArray();

    }
}
