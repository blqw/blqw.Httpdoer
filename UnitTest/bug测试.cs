using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Web;

namespace UnitTest
{
    [TestClass]
    public class UnitTest4
    {
        [TestMethod]
        public async Task 不设置Method()
        {
            var www = new Httpdoer("www.baidu.com");
            var result = www.Send();
            Assert.IsNull(result.Exception);

            www = new Httpdoer("www.baidu.com");
            result = await www.SendAsync();
            Assert.IsNull(result.Exception);
        }

        [TestMethod]
        public async Task 测试302跳转()
        {
            var www = new Httpdoer("http://oauth.apitops.com/oauth/login");
            www.AutoRedirect = false;
            var result = www.Send();
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Redirect);

            www = new Httpdoer("http://oauth.apitops.com/oauth/login");
            www.AutoRedirect = true;
            result = www.Send();
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            www = new Httpdoer("http://oauth.apitops.com/oauth/login");
            www.AutoRedirect = false;
            result = await www.SendAsync();
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Redirect);

            www = new Httpdoer("http://oauth.apitops.com/oauth/login");
            www.AutoRedirect = true;
            result = await www.SendAsync();
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

        }


    }
}
