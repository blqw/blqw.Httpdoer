using System;
using blqw.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest5
    {
        [TestMethod]
        public void GET数组参数测试()
        {
            var www = new Httpdoer("www.baidu.com");
            www.Method = HttpRequestMethod.Get;
            www.Params.AddModel(new { a = new[] { 1, 2, 3 } });
            www.Query.ArrayEncodeMode = ArrayEncodeMode.Default;
            Assert.AreEqual("http://www.baidu.com?a=1&a=2&a=3", www.ToString("q"));
            www.Query.ArrayEncodeMode = ArrayEncodeMode.Asp;
            Assert.AreEqual("http://www.baidu.com?a[0]=1&a[1]=2&a[2]=3", Uri.UnescapeDataString(www.ToString("q")));
            www.Query.ArrayEncodeMode = ArrayEncodeMode.JQuery;
            Assert.AreEqual("http://www.baidu.com?a[]=1&a[]=2&a[]=3", Uri.UnescapeDataString(www.ToString("q")));
            www.Query.ArrayEncodeMode = ArrayEncodeMode.Json;
            Assert.AreEqual("http://www.baidu.com?a=" + Uri.EscapeDataString("[1,2,3]"), www.ToString("q"));
            www.Query.ArrayEncodeMode = ArrayEncodeMode.Merge;
            Assert.AreEqual("http://www.baidu.com?a=1,2,3", www.ToString("q"));
        }



        [TestMethod]
        public void GET对象参数测试()
        {
            var www = new Httpdoer("www.baidu.com");
            www.Method = HttpRequestMethod.Get;
            www.Params.AddModel(new { user = new { id = 1, name = "blqw", sex = true } });
            www.Query.ObjectEncodeMode = ObjectEncodeMode.Default;
            Assert.AreEqual("http://www.baidu.com?user.id=1&user.name=blqw&user.sex=true", www.ToString("q"));
            www.Query.ObjectEncodeMode = ObjectEncodeMode.JQuery;
            Assert.AreEqual("http://www.baidu.com?user[id]=1&user[name]=blqw&user[sex]=true", Uri.UnescapeDataString(www.ToString("q")));
            www.Query.ObjectEncodeMode = ObjectEncodeMode.Json;
            Assert.AreEqual("http://www.baidu.com?user="+ Uri.EscapeDataString("{\"id\":1,\"name\":\"blqw\",\"sex\":true}"), www.ToString("q"));
            www.Query.ObjectEncodeMode = ObjectEncodeMode.NameOnly;
            Assert.AreEqual("http://www.baidu.com?id=1&name=blqw&sex=true", www.ToString("q"));

        }


        [TestMethod]
        public void POST数组参数测试()
        {
            var www = new Httpdoer("www.baidu.com");
            www.Method = HttpRequestMethod.Post;
            www.Body.AddModel(new { a = new[] { 1, 2, 3 } });
            www.Query.ArrayEncodeMode = ArrayEncodeMode.Default;
            Assert.AreEqual("a=1&a=2&a=3", www.Body.ToString());
            www.Query.ArrayEncodeMode = ArrayEncodeMode.Asp;
            Assert.AreEqual("a[0]=1&a[1]=2&a[2]=3", Uri.UnescapeDataString(www.Body.ToString()));
            www.Query.ArrayEncodeMode = ArrayEncodeMode.JQuery;
            Assert.AreEqual("a[]=1&a[]=2&a[]=3", Uri.UnescapeDataString(www.Body.ToString()));
            www.Query.ArrayEncodeMode = ArrayEncodeMode.Json;
            Assert.AreEqual("a=" + Uri.EscapeDataString("[1,2,3]"), www.Body.ToString());
            www.Query.ArrayEncodeMode = ArrayEncodeMode.Merge;
            Assert.AreEqual("a=1,2,3", www.Body.ToString());
        }

        [TestMethod]
        public void POST对象参数测试()
        {
            var www = new Httpdoer("www.baidu.com");
            www.Method = HttpRequestMethod.Post;
            www.Body.AddModel(new { user = new { id = 1, name = "blqw", sex = true } });
            www.Query.ObjectEncodeMode = ObjectEncodeMode.Default;
            Assert.AreEqual("user.id=1&user.name=blqw&user.sex=true", www.Body.ToString());
            www.Query.ObjectEncodeMode = ObjectEncodeMode.JQuery;
            Assert.AreEqual("user[id]=1&user[name]=blqw&user[sex]=true", Uri.UnescapeDataString(www.Body.ToString()));
            www.Query.ObjectEncodeMode = ObjectEncodeMode.Json;
            Assert.AreEqual("user=" + Uri.EscapeDataString("{\"id\":1,\"name\":\"blqw\",\"sex\":true}"), www.Body.ToString());
            www.Query.ObjectEncodeMode = ObjectEncodeMode.NameOnly;
            Assert.AreEqual("id=1&name=blqw&sex=true", www.Body.ToString());

        }
    }
}
