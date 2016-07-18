using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Web;
using System.Net;

namespace UnitTest
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void Json提交bug()
        {
            var model = new
            {
                OwnerId = 510789,
                BrokerKId = 49005,
                BrokerName = "肖佳",
                Phone = "15012345600",
                EarnestMoney = 15200,
                HouseList = new[] {
                    new { VillageName = "凤凰城", Building = "1幢", HouseId = "12" }
                }
            };

            var www = new HttpRequest();
            www.Body.AddModel(model);
            www.Method = HttpRequestMethod.Post;
            www.Body.ContentType = HttpContentType.Json;
            var json = www.Body.ToString();
            Assert.AreEqual(
                "{\"OwnerId\":510789,\"BrokerKId\":49005,\"BrokerName\":\"肖佳\",\"Phone\":\"15012345600\",\"EarnestMoney\":15200,\"HouseList\":[{\"VillageName\":\"凤凰城\",\"Building\":\"1幢\",\"HouseId\":\"12\"}]}"
                , json);
            www.Body.ContentType = HttpContentType.Form;
            var str = Uri.UnescapeDataString(www.Body.ToString());
            Assert.AreEqual(
                "OwnerId=510789&BrokerKId=49005&BrokerName=肖佳&Phone=15012345600&EarnestMoney=15200&HouseList[][VillageName]=凤凰城&HouseList[][Building]=1幢&HouseList[][HouseId]=12"
                , str
                );
        }

        [TestMethod]
        public void 多次赋值测试()
        {
            var www = new HttpRequest();
            www.Query.Add("a", "1");
            www.Query.Add("a", "2");
            www.Query.Add("a", "3");
            www.Query.Add("a", "4");
            Assert.AreEqual("1,2,3,4", www.Query["a"]);
            www.Query["a"] = "5";
            Assert.AreEqual("5", www.Query["a"]);
        }

        [TestMethod]
        public void 字符串数字数组混合赋值测试()
        {
            var www = new HttpRequest();
            www.Body.Add("a", "1,2,3");
            www.Body.Add("a", "4,5,6");
            www.Body.Add("a", 7);
            www.Body.Add("a", new object[] { 8, 9 });
            Assert.AreEqual("1,2,3", www.Body["a"]);
            var arr = www.Body.GetValues("a");
            Assert.AreEqual(5, arr.Count);
            Assert.AreEqual("1,2,3", arr[0]);
            Assert.AreEqual("4,5,6", arr[1]);
            Assert.AreEqual(7, arr[2]);
            Assert.AreEqual(8, arr[3]);
            Assert.AreEqual(9, arr[4]);

            Assert.AreEqual("1,2,3,4,5,6,7,8,9", string.Join(",", www.Body.GetValues("a")));
            www.Body.ContentType = HttpContentType.Form;
            Assert.AreEqual(
                "a[]=1,2,3&a[]=4,5,6&a[]=7&a[]=8&a[]=9"
                , Uri.UnescapeDataString(www.Body.ToString())
                );
            www.Body.ContentType = HttpContentType.Json;
            var json = www.Body.ToString();
            Assert.AreEqual("{\"a\":[\"1,2,3\",\"4,5,6\",7,8,9]}", json);
        }


        [TestMethod]
        public void 数组嵌套测试()
        {
            var www = new HttpRequest();
            www.Body.Add("a", new object[] {
                 new[] { 1, 2 },
                  new[] { 3, 4, 5 }
            });
            www.Body.ContentType = HttpContentType.Json;
            var json = www.Body.ToString();
            Assert.AreEqual("{\"a\":[[1,2],[3,4,5]]}", json);
        }


        [TestMethod]
        public void 测试头编码()
        {
            var model = new
            {
                OwnerId = 510789,
                BrokerKId = 49005,
                BrokerName = "肖佳",
                Phone = "15012345600",
                EarnestMoney = 15200,
                HouseList = new[] {
                    new { VillageName = "凤凰城", Building = "1幢", HouseId = "12" }
                }
            };

            var www = new Httpdoer("www.baidu.com");
            //www.Timeout = new TimeSpan(0,0,1);
            www.Headers.KeepAlive = true;
            www.Cookies.Add(new Uri("http://www.baidu.com"), new Cookie("sessionid", "123456789"));
            www.Body.AddModel(model);
            www.Method = HttpRequestMethod.Post;
            www.Encoding = System.Text.Encoding.Default;
            www.Body.ContentType = HttpContentType.Json;
            www.GetString();
        }

    }
}
