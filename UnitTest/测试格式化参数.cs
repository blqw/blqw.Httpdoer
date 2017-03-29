using System;
using blqw.Web;
using blqw.Web.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest6
    {
        [TestMethod]
        public void TestMethod1()
        {
            var www = HttpGenerator.Create<IApiTest>("http://baidu.com");
            var time = new DateTime(1970, 1, 1, 12, 12, 12);
            var req0 = www.Test0(time);
            var url0 = req0.RequestData.Url;
            Assert.AreEqual($"http://baidu.com/test/?now={Uri.EscapeDataString(time.ToString())}", url0);


            var req1 = www.Test1(time, time, new object());
            var url1 = req1.RequestData.Url;
            Assert.AreEqual($"http://baidu.com/test/?now={Uri.EscapeDataString(time.ToString("yyyy-MM-dd"))}", url1);
        }

        public interface IApiTest
        {
            [HttpGet("/test/")]
            IHttpResponse Test0([Query]DateTime now);

            [HttpGet("/test/")]
            IHttpResponse Test1([Query(Format = "yyyy-MM-dd")]DateTime now, [Header(Format = "yyyy-MM-dd")]DateTime head, [Header(Format = "yyyy-MM-dd")]object head2);
        }
    }

}
