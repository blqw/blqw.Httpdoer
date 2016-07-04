using blqw.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Bing
    {
        const string START = "<d:Text m:type=\"Edm.String\">";
        const string END = "</d:Text>";
        const string AUTH_TOKEN = "Basic NjBhYzBhNmQtMzkwMi00YTYwLTlhODItOWVhMDU1OTA0OGVhOmVKNk1UY3FRa2tVMlZISWVrcWFLdFBBVi9yQW56Zi9RVGIzY1NCcHFSQkU9";

        public static string Translate(string text)
        {
            var www = new Httpdoer("https://api.datamarket.azure.com");
            www.Method = HttpRequestMethod.Get;
            www.Path = "Bing/MicrosoftTranslator/v1/Translate";
            www.Query.AddModel(new { Text = $"'{text}'", To = "'zh-CHS'" });
            www.Headers["Authorization"] = AUTH_TOKEN;
            
            return GetText(www.GetString());
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




        public static string Translate2(string text)
        {
            var domain = "https://api.datamarket.azure.com";
            var trans = HttpGenerator.Create<ITranslate>(domain);
            var xml = trans.Translate(AUTH_TOKEN, $"'{text}'", "'zh-CHS'");
            return GetText(xml);
        }

        public static async Task<string> TranslateAsync(string text)
        {
            var domain = "https://api.datamarket.azure.com";
            var trans = HttpGenerator.Create<ITranslate>(domain);
            var xml = await trans.TranslateAsync(AUTH_TOKEN, $"'{text}'", "'zh-CHS'");
            return GetText(xml);
        }
    }
}
