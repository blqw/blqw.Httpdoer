using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using blqw.Web;
using blqw.Web.Extensions;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTest3
    {
        public interface ITranslate : ITranslateAsync
        {
            [HttpGet("Bing/MicrosoftTranslator/v1/Translate")]
            string Translate([Header("Authorization")]string token, string Text, string To);
        }

        public interface ITranslateAsync
        {
            [HttpGet("Bing/MicrosoftTranslator/v1/Translate")]
            Task<string> TranslateAsync([Header]string Authorization, string Text, string To);
        }

        const string START = "<d:Text m:type=\"Edm.String\">";
        const string END = "</d:Text>";
        const string AUTH_TOKEN = "Basic NjBhYzBhNmQtMzkwMi00YTYwLTlhODItOWVhMDU1OTA0OGVhOmVKNk1UY3FRa2tVMlZISWVrcWFLdFBBVi9yQW56Zi9RVGIzY1NCcHFSQkU9";

        [TestMethod]
        public void 泛型同步测试()
        {
            Httpdoer.DefaultLogger.Switch.Level = SourceLevels.All;
            var text = "hello world";
            var domain = "https://api.datamarket.azure.com";
            var trans = HttpGenerator.Create<ITranslate>(domain);
            var xml = trans.Translate(AUTH_TOKEN, $"'{text}'", "'zh-CHS'");
            var result = GetText(xml);
            Assert.AreEqual("世界您好", result);
        }


        [TestMethod]
        public async Task 泛型异步测试()
        {
            var text = "hello world";
            var domain = "https://api.datamarket.azure.com";
            var trans = HttpGenerator.Create<ITranslate>(domain);
            var xml = await trans.TranslateAsync(AUTH_TOKEN, $"'{text}'", "'zh-CHS'");
            var result = GetText(xml);
            Assert.AreEqual("世界您好", result);
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
