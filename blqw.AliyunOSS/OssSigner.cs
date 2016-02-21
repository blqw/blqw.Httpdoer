using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web
{
    /// <summary>
    /// 阿里云OSS私有文件签名
    /// </summary>
    public static class OssSigner
    {
        readonly static char[] _separator = new[] { '.' };
        const string NEW_LINE = "\n";

        const string HEAD_CONTENT_TYPE = "content-type";
        const string HEAD_DATE = "date";
        const string HEAD_EXPIRES = "expires";
        const string HEAD_ETAG = "etag";
        const string HEAD_LASTMODIFIED = "last-modified";
        const string HEAD_RANGE = "range";
        const string HEAD_COPYSOURCE = "x-oss-copy-source";
        const string HEAD_COPYSOURCERANGE = "x-oss-copy-source-range";
        const string HEAD_LOCATION = "location";
        const string HEAD_SERVERSIDEENCRYPTION = "x-oss-server-side-encryption";
        const string HEAD_SECURITYTOKEN = "x-oss-security-token";
        const string HEAD_AUTHORIZATION = "Authorization";
        const string HEAD_CACHECONTROL = "cache-control";
        const string HEAD_CONTENTDISPOSITION = "content-disposition";
        const string HEAD_CONTENTENCODING = "content-encoding";
        const string HEAD_CONTENTLENGTH = "content-length";
        const string HEAD_CONTENTMD5 = "content-md5";
        const string HEAD_OSSPREFIX = "x-oss-";

        static readonly HashSet<string> SignKeys = new HashSet<string>("acl,uploadId,partNumber,uploads,cors,logging,website,delete,referer,lifecycle,security-token,response-cache-control,response-content-disposition,response-content-encoding,response-content-language,response-content-type,response-expires".Split(','));

        /// <summary> OSS签名
        /// </summary>
        /// <param name="request"></param>
        public static void Sign(HttpRequest request)
        {
            var accessKeyId = OSS.AliyunID;
            var secretAccessKey = OSS.AliyunSecret;
            var buffer = new StringBuilder();

            buffer.Append(request.Method.ToString().ToUpperInvariant());
            buffer.Append(NEW_LINE);

            //header
            foreach (var item in SignArgs(request).OrderBy(e => e.Key, StringComparer.Ordinal))
            {
                var key = item.Key;
                Object value = item.Value;

                if (key.StartsWith(HEAD_OSSPREFIX))
                    buffer.Append(key).Append(':').Append(value);
                else
                    buffer.Append(value);

                buffer.Append(NEW_LINE);
            }

            //path
            var url = new Uri(request.ToString("f"));
            buffer.Append("/");
            buffer.Append(url.Authority.Split(_separator)[0]);
            buffer.Append(url.PathAndQuery);
            //parameters
            var parameters = GetParameters(request);
            if (parameters.Count > 0)
            {
                var separator = '?';
                foreach (var key in parameters.Keys.Cast<string>().Where(SignKeys.Contains).OrderBy(it => it))
                {
                    buffer.Append(separator);
                    buffer.Append(key);
                    var paramValue = parameters[key];
                    if (paramValue != null)
                        buffer.Append("=").Append(paramValue);
                    if (separator == '?')
                        separator = '&';
                }
            }

            var signature = ComputeSignature(secretAccessKey, buffer.ToString());

            request.Headers.Add(HEAD_AUTHORIZATION, "OSS " + accessKeyId + ":" + signature);
        }

        /// <summary>
        /// 获取参数,如果是post 且body中有参数,则使用body 否则使用queryString
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static HttpParameterCollection GetParameters(HttpRequest request)
        {
            if (request.Method == HttpRequestMethod.POST && request.FormBody.Count > 0)
            {
                return request.FormBody;
            }
            return request.QueryString;
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ComputeSignature(string key, string data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException("data");

            using (var algorithm = KeyedHashAlgorithm.Create("HMACSHA1"))
            {
                algorithm.Key = Encoding.UTF8.GetBytes(key.ToCharArray());
                return Convert.ToBase64String(
                    algorithm.ComputeHash(Encoding.UTF8.GetBytes(data.ToCharArray())));
            }
        }

        /// <summary>
        /// 获取签名参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        static IEnumerable<KeyValuePair<string, string>> SignArgs(HttpRequest request)
        {
            var headers = request.Headers;
            foreach (string key in headers.Keys)
            {
                var lowerKey = key.ToLowerInvariant();

                if (lowerKey.StartsWith(HEAD_OSSPREFIX))
                {
                    yield return new KeyValuePair<string, string>(lowerKey, headers[key]);
                }
            }

            yield return new KeyValuePair<string, string>(HEAD_DATE, headers[HEAD_DATE]);
            yield return new KeyValuePair<string, string>(HEAD_CONTENT_TYPE, headers[HEAD_CONTENT_TYPE] ?? "");
            yield return new KeyValuePair<string, string>(HEAD_CONTENTMD5, headers[HEAD_CONTENTMD5] ?? "");

            var parameters = GetParameters(request);
            foreach (string key in parameters.Keys)
            {
                var lowerKey = key.ToLowerInvariant();

                if (lowerKey.StartsWith(HEAD_OSSPREFIX))
                {
                    yield return new KeyValuePair<string, string>(lowerKey, parameters[key]);
                }
            }


        }


    }
}
