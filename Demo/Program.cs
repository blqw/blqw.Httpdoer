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
            Debug.Listeners.Add(new ConsoleTraceListener());
            while (true)
            {
                //Console.WriteLine();
                //Console.WriteLine(Bind.TranslateAsync("hello world").Result);

                //var www = new HttpRequest("http://cmd-internal.tops001.com");
                //www.Path = "/test/getuser";
                //www.Method = HttpRequestMethod.Post;

                //www.Body["ak"] = Guid.Parse("5893d110-3213-11e6-b28c-288023a0fe60");

                //var obj = www.GetStringAsync().Result;

                var client = HttpGenerator.Create<IMyTestApi>("http://cmd-internal.tops001.com");



                var user = client.GetUser(Guid.Parse("03f53a51-3291-11e6-b28c-288023a0fe60"));


                Console.Read();
            }
        }



    }
}
