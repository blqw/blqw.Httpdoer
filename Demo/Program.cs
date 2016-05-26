﻿using blqw.Web;
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
            var request = new HttpRequest("https://api.datamarket.azure.com");
            request.Timeout = new TimeSpan(1, 0, 0);
            request.Method = System.Net.Http.HttpMethod.Post;
            request.Query["a"] = "3";
            request.Query["b"] = "4";
            request.Params["a"] = 1;
            request.Params["b"] = 2;
            var client = new HttpClientSync();
            var res = client.Send(request);
            Console.WriteLine(res.Body.ToString());


            //var www = new HttpRequest("http://localhost:27214/api/values");
            //www.Timeout = new TimeSpan(0,0,5);
            //www.QueryString.Add("id", "1");
            //var task = www.GetString();
            //www.Abort();
            //task.Wait();
            //var www = new HttpRequest("https://api.datamarket.azure.com");
            //www.Method = HttpRequestMethod.GET;
            //www.Path = "Bing/MicrosoftTranslator/v1/Translate";
            //www.AcceptCookie = true;
            //www.AcceptHeader = true;
            //www.QueryString += new
            //{
            //    Text = "'hello world'",
            //    To = "'zh-CHS'"
            //};

            //www.Headers.Add("Authorization", AUTH_TOKEN);

            //var str = www.GetString().Result;

            //Console.WriteLine();
            //Console.WriteLine(GetText(str));
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
