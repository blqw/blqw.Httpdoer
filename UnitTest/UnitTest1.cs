using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Web;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        const string START = "<d:Text m:type=\"Edm.String\">";
        const string END = "</d:Text>";
        const string AUTH_TOKEN = "Basic NjBhYzBhNmQtMzkwMi00YTYwLTlhODItOWVhMDU1OTA0OGVhOmVKNk1UY3FRa2tVMlZISWVrcWFLdFBBVi9yQW56Zi9RVGIzY1NCcHFSQkU9";
        [TestMethod]
        public void 基本功能测试()
        {
            var www = new HttpRequest("https://api.datamarket.azure.com");
            www.Method = HttpRequestMethod.Get;
            www.Path = "Bing/MicrosoftTranslator/v1/Translate";
            www.Query.AddModel(new
            {
                Text = "'hello world'",
                To = "'zh-CHS'"
            });

            www.Headers["Authorization"] = AUTH_TOKEN;

            var str = www.GetString();

            Assert.AreEqual("世界您好", GetText(str));
        }
        static string GetText(string str)
        {
            var start = str.IndexOf(START);
            if (start < 0)
            {
                return "翻译失败";
            }
            start += START.Length;
            var end = str.IndexOf(END, start);
            return str.Substring(start, end - start);
        }
    }
}
