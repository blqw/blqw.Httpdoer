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
            www.FormBody.Add(model);
            www.Method = HttpRequestMethod.POST;
            www.FormBody.ContentType = ContentType.ApplicationJson;
            var json = www.Encoding.GetString(www.FormBody.GetBytes(www.Encoding));
            Assert.AreEqual(
                "{\"OwnerId\":510789,\"BrokerKId\":49005,\"BrokerName\":\"肖佳\",\"Phone\":\"15012345600\",\"EarnestMoney\":15200,\"HouseList\":[{\"VillageName\":\"凤凰城\",\"Building\":\"1幢\",\"HouseId\":\"12\"}]}"
                , json);
            var str = www.FormBody.ToString();
            Assert.AreEqual(
                "OwnerId=510789&BrokerKId=49005&BrokerName=肖佳&Phone=15012345600&EarnestMoney=15200&HouseList.VillageName=凤凰城&HouseList.Building=1幢&HouseList.HouseId=12"
                , Uri.UnescapeDataString(str)
                );
        }

        [TestMethod]
        public void 多次赋值测试()
        {
            var www = new HttpRequest();
            www.QueryString.Add("a", 1);
            www.QueryString.Add("a", 2);
            www.QueryString.Add("a", 3);
            www.QueryString.Add("a", 4);
            Assert.AreEqual("1,2,3,4", www.QueryString["a"]);
        }

        [TestMethod]
        public void 多次数组赋值测试()
        {
            var www = new HttpRequest();
            www.QueryString.Add("a", 1);
            www.QueryString.Add("a", new[] { 3, 4, 5 });
            www.QueryString.Add("a", 2);
            www.QueryString.Add("a", new object[] { 6, 7 });
            Assert.AreEqual("1,3,4,5,2,6,7", www.QueryString["a"]);
        }

        [TestMethod]
        public void 字符串数字数组混合赋值测试()
        {
            var www = new HttpRequest();
            www.FormBody.Add("a", "1,2,3");
            www.FormBody.Add("a", "4,5,6");
            www.FormBody.Add("a", 7);
            www.FormBody.Add("a", new object[] { 8, 9 });
            Assert.AreEqual("1,2,3,4,5,6,7,8,9", www.FormBody["a"]);
            Assert.AreEqual(
                "a=1,2,3&a=4,5,6&a=7&a=8&a=9"
                , Uri.UnescapeDataString(www.FormBody.ToString())
                );
            www.FormBody.ContentType = ContentType.ApplicationJson;
            var json = www.Encoding.GetString(www.FormBody.GetBytes(www.Encoding));
            Assert.AreEqual("{\"a\":[\"1,2,3\",\"4,5,6\",7,8,9]}", json);
        }


        [TestMethod]
        public void 数组嵌套测试()
        {
            var www = new HttpRequest();
            www.FormBody.Add("a", new object[] {
                 new[] { 1, 2 },
                  new[] { 3, 4, 5 }
            });
            www.FormBody.ContentType = ContentType.ApplicationJson;
            var json = www.Encoding.GetString(www.FormBody.GetBytes(www.Encoding));
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

            var www = new HttpRequest("www.baidu.com");
            www.KeepAlive = true;
            www.Cookie.Add(new Uri("http://www.baidu.com"),new Cookie("sessionid","123456789"));
            www.FormBody.Add(model);
            www.Method = HttpRequestMethod.POST;
            www.Encoding = System.Text.Encoding.Default;
            www.FormBody.ContentType = ContentType.ApplicationJson + ";charset=utf-8";
            www.GetString().Wait();
        }

    }
}
