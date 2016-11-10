using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blqw.Web.Tests
{
    [TestClass()]
    public class HttpdoerTests
    {
        [TestMethod()]
        public void GetTest()
        {
            Assert.IsNull(Httpdoer.Get("www.baidu.com", new { id = 1 }).Send().Exception);
            Assert.IsNull(Httpdoer.Get("www.baidu.com/s").Send().Exception);
            var req = Httpdoer.Get("www.baidu.com", new { id = 1 }, new { Authorization = "xxxx" });
            Assert.AreEqual("http://www.baidu.com?id=1", req.ToString("q"));
            Assert.AreEqual("xxxx", req.Headers["Authorization"]);
            Assert.IsNull(req.Send().Exception);
        }


        [TestMethod]
        public void 测试生成Url()
        {
            var www = new Httpdoer("www.baidu.com");
            www.Path = "s";
            Assert.AreEqual("http://www.baidu.com/s", www.ToString("q"));


            www.Path = "/s";
            Assert.AreEqual("http://www.baidu.com/s", www.ToString("q"));

            www.Query.Add("id", "1");
            Assert.AreEqual("http://www.baidu.com/s?id=1", www.ToString("q"));

            www.Path = "/s/";
            Assert.AreEqual("http://www.baidu.com/s/?id=1", www.ToString("q"));


            www.Path = "/s/?id=2";
            Assert.AreEqual("http://www.baidu.com/s/?id=2&id=1", www.ToString("q"));

            www = new Httpdoer("www.baidu.com?id=3");
            www.Path = "s?id=2";
            Assert.AreEqual("http://www.baidu.com/s?id=2", www.ToString("q"));

            www.Query.Add("id", "1");
            Assert.AreEqual("http://www.baidu.com/s?id=2&id=1", www.ToString("q"));

            www.Path = "?id=2";
            Assert.AreEqual("http://www.baidu.com/?id=2&id=1", www.ToString("q"));

            www.Path = "&id=2";
            Assert.AreEqual("http://www.baidu.com/?id=3&id=2&id=1", www.ToString("q"));

            www = new Httpdoer("www.baidu.com/a?id=3");
            www.Path = "s?id=2";
            Assert.AreEqual("http://www.baidu.com/s?id=2", www.ToString("q"));

            www = new Httpdoer("www.baidu.com/a/b");
            www.Path = "c";
            Assert.AreEqual("http://www.baidu.com/a/c", www.ToString("q"));
        }
    }
}