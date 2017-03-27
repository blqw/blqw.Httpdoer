using blqw.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var www = new Httpdoer("www.baidu.com");
            www.Body.ContentType = HttpContentType.Json;
            www.Method = HttpRequestMethod.Post;
            www.Body.Wirte(Encoding.UTF8.GetBytes("{'a':1}"));
            // www.Body.AddModel(new { a = 1, b = "fdsfdas", c = new { d = true, e = DateTime.Now, f = new[] { 1, 2, 3, 4, 5 } } });
            var str = www.GetString();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(www.Response.RequestData.Raw);
            Console.WriteLine("-----------------------------------");
            Console.WriteLine(str);

        }



    }
}
