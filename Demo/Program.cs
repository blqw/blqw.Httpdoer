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
            www.Query.AddModel(new {a = new[] {1, 2, 3}});
            www.Query.ArrayEncodeMode =ArrayEncodeMode.Json;
            Console.WriteLine(www.ToString("q"));

        }



    }
}
