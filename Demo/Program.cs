using blqw.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        const string START = "<d:Text m:type=\"Edm.String\">";
        const string END = "</d:Text>";
        const string AUTH_TOKEN = "Basic NjBhYzBhNmQtMzkwMi00YTYwLTlhODItOWVhMDU1OTA0OGVhOmVKNk1UY3FRa2tVMlZISWVrcWFLdFBBVi9yQW56Zi9RVGIzY1NCcHFSQkU9";

        static void Main(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());


            var www = new HttpRequest("https://api.datamarket.azure.com");
            www.Method = HttpRequestMethod.Get;
            www.Path = "Bing/MicrosoftTranslator/v1/Translate";
            www.Query.AddModel(new { Text = "'hello world'", To = "'zh-CHS'" });
            www.Headers["Authorization"] = AUTH_TOKEN;

            www.BeginSend(ar =>
            {
                var res = www.EndSend(ar);
                var str = res.Body.ToString();
                Console.WriteLine(GetText(str));
            }, null);

            //var str = www.GetString();

            //Console.WriteLine();
            //Console.WriteLine(GetText(str));

            Console.Read();
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
