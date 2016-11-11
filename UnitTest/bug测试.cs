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
        [TestInitialize]
        public void TestInitialize()
        {
            HttpRequest.ClearLocalCookies();
        }

        [TestMethod]
        public async Task 不设置Method()
        {
            var www = new Httpdoer("www.baidu.com");
            var result = www.Send();
            Assert.IsNull(result.Exception);

            www = new Httpdoer("www.baidu.com");
            result = await www.SendAsync();
            Assert.IsNull(result.Exception);

            www = new Httpdoer("www.baidu.com");
            www.Body.Add("name", "value");
            Assert.IsNotNull(www.Body.ToString());
            Assert.AreEqual(HttpContentType.Form, www.Body.ContentType);


        }


        [TestMethod]
        public async Task 测试302跳转()
        {
            var www = new Httpdoer("http://i.baidu.com/");
            www.AutoRedirect = false;
            var result = www.Send();
            Assert.AreEqual(HttpStatusCode.Redirect, result.StatusCode);

            www = new Httpdoer("http://i.baidu.com/");
            www.AutoRedirect = true;
            result = www.Send();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            www = new Httpdoer("http://i.baidu.com/");
            www.AutoRedirect = false;
            result = await www.SendAsync();
            Assert.AreEqual(HttpStatusCode.Redirect, result.StatusCode);

            www = new Httpdoer("http://i.baidu.com/");
            www.AutoRedirect = true;
            result = await www.SendAsync();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        }

        [TestMethod]
        public void 测试同步模式下的Cookie_None()
        {
            var www = new Httpdoer("http://baidu.com/");
            www.CookieMode = HttpCookieMode.None;
            var res = www.Send();

            Assert.IsTrue(HttpRequest.LocalCookies.Count == 0);
            Assert.IsTrue(www.Cookies == null);
            Assert.IsTrue(res.Cookies == null);
        }

        [TestMethod]
        public void 测试同步模式下的Cookie_UserCustom()
        {
            var www = new Httpdoer("http://www.baidu.com/");
            www.CookieMode = HttpCookieMode.UserCustom;
            var res = www.Send();
            Assert.IsTrue(HttpRequest.LocalCookies.Count == 0);
            Assert.IsTrue(www.Cookies != HttpRequest.LocalCookies);
            Assert.IsTrue(www.Cookies.Count > 0);
            Assert.IsTrue(res.Cookies.Count > 0);
        }

        [TestMethod]
        public void 测试同步模式下的Cookie_ApplicationCache()
        {
            var www = new Httpdoer("http://www.baidu.com/");
            www.CookieMode = HttpCookieMode.ApplicationCache;
            var res = www.Send();
            Assert.IsTrue(www.Cookies == HttpRequest.LocalCookies);
            Assert.IsTrue(www.Cookies.Count > 0);
            Assert.IsTrue(res.Cookies.Count > 0);
        }

        [TestMethod]
        public void 测试同步模式下的Cookie_CustomOrCache()
        {
            var www = new Httpdoer("http://www.baidu.com/");
            www.CookieMode = HttpCookieMode.UserCustom;
            var res = www.Send();
            Assert.IsTrue(HttpRequest.LocalCookies.Count == 0);
            Assert.IsTrue(www.Cookies != HttpRequest.LocalCookies);
            Assert.IsTrue(www.Cookies.Count > 0);
            Assert.IsTrue(res.Cookies.Count > 0);
            var count = www.Cookies.Count;

            www = new Httpdoer("http://www.baidu.com/");
            www.CookieMode = HttpCookieMode.CustomOrCache;
            HttpRequest.LocalCookies.Add(new Cookie("aa", "11", "/", "www.baidu.com"));
            res = www.Send();
            Assert.IsTrue(www.Cookies != HttpRequest.LocalCookies);
            Assert.IsTrue(www.Cookies.Count == count + 1);
            Assert.IsTrue(res.Cookies.Count > 0);
        }

        [TestMethod]
        public async Task 测试异步模式下的Cookie_None()
        {
            var www = new Httpdoer("http://www.baidu.com/");
            www.CookieMode = HttpCookieMode.None;
            var res = await www.SendAsync();
            Assert.IsTrue(HttpRequest.LocalCookies.Count == 0);
            Assert.IsTrue(www.Cookies == null);
            Assert.IsTrue(res.Cookies == null);
        }

        [TestMethod]
        public async Task 测试异步模式下的Cookie_UserCustom()
        {
            var www = new Httpdoer("http://www.baidu.com/");
            www.CookieMode = HttpCookieMode.UserCustom;
            var res = await www.SendAsync();
            Assert.IsTrue(HttpRequest.LocalCookies.Count == 0);
            Assert.IsTrue(www.Cookies != HttpRequest.LocalCookies);
            Assert.IsTrue(www.Cookies.Count > 0);
            Assert.IsTrue(res.Cookies.Count > 0);
        }

        [TestMethod]
        public async Task 测试异步模式下的Cookie_ApplicationCache()
        {
            var www = new Httpdoer("http://www.baidu.com/");
            www.CookieMode = HttpCookieMode.ApplicationCache;
            var res = await www.SendAsync();
            Assert.IsTrue(www.Cookies == HttpRequest.LocalCookies);
            Assert.IsTrue(www.Cookies.Count > 0);
            Assert.IsTrue(res.Cookies.Count > 0);
        }

        [TestMethod]
        public async Task 测试异步模式下的Cookie_CustomOrCache()
        {
            var www = new Httpdoer("http://www.baidu.com/");
            www.CookieMode = HttpCookieMode.UserCustom;
            var res = await www.SendAsync();
            Assert.IsTrue(HttpRequest.LocalCookies.Count == 0);
            Assert.IsTrue(www.Cookies != HttpRequest.LocalCookies);
            Assert.IsTrue(www.Cookies.Count > 0);
            Assert.IsTrue(res.Cookies.Count > 0);
            var count = www.Cookies.Count;

            www = new Httpdoer("http://www.baidu.com/");
            www.CookieMode = HttpCookieMode.CustomOrCache;
            HttpRequest.LocalCookies.Add(new Cookie("aa", "11", "/", "www.baidu.com"));
            res = await www.SendAsync();
            Assert.IsTrue(www.Cookies != HttpRequest.LocalCookies);
            Assert.IsTrue(www.Cookies.Count == count + 1);
            Assert.IsTrue(res.Cookies.Count > 0);
        }

    }
}
