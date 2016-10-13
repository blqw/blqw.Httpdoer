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

        [TestMethod]
        public async Task 测试Cookie()
        {
            var www = new Httpdoer("https://www.baidu.com");
            www.CookieMode = HttpCookieMode.UserCustom;
            var res = await www.SendAsync();
            Assert.IsTrue(res.Cookies.Count > 0);


            var www1 = new Httpdoer("https://www.baidu.com");
            www1.CookieMode = HttpCookieMode.UserCustom;
            foreach (Cookie cookie in res.Cookies)
            {
                www1.Cookies.Add(cookie);
            }
            var res1 = await www1.SendAsync();
            Assert.IsTrue(res1.Cookies.Count > 0);

            var www2 = new Httpdoer("https://www.baidu.com");
            www2.CookieMode = HttpCookieMode.UserCustom;
            int i = 0;
            foreach (Cookie cookie in res.Cookies)
            {
                cookie.Name = i.ToString();
                cookie.Value = "xxxxxx";
                www2.Cookies.Add(cookie);
                i++;
            }
            var res2 = www2.Send();
            Assert.IsTrue(res2.Cookies.Count == 4);


            var www3 = new Httpdoer("https://www.baidu.com");
            www3.CookieMode = HttpCookieMode.UserCustom;
            int j = 0;
            foreach (Cookie cookie in res.Cookies)
            {
                cookie.Name = j.ToString();
                cookie.Value = "xxxxxx";
                www3.Cookies.Add(cookie);
                j++;
            }
            var res3 = await www3.SendAsync();
            Assert.IsTrue(res3.Cookies.Count == 3);
        }

    }
}
