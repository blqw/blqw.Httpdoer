using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Tests
{
    [TestClass()]
    public class HttpParamsExtensionsTests
    {
        [TestMethod()]
        public void 测试自定义ContentType()
        {
            var www = new Httpdoer("http://www.baidu.com");
            www.Path = "s";
            www.Method = HttpRequestMethod.Put;
            www.Body.ContentType = new HttpContentType("aaabbbccc", null, null, HttpBodyParsers.Stream);
            www.Body.Wirte(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            var str = www.GetString();
            Trace.WriteLine(www.Response.RequestData.Raw);
            Assert.IsNull(www.Exception);
        }
    }
}