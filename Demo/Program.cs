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
        class User
        {
            public User(string ak)
            {
                this.ak = Guid.Parse(ak);
            }
            public Guid ak { get; set; }
        }
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());
            while (true)
            {
                //Console.WriteLine();
                Console.WriteLine(Bind.Translate("hello world"));
                //var user = new User("03f53a51-3291-11e6-b28c-288023a0fe60");

                //var www = new Httpdoer("http://cmd-internal.tops001.com");
                //www.Path = "/test/getuser";
                //www.Body.ContentType = "dfsfdasfdsafdsafdsa/json";
                //www.Method = HttpRequestMethod.Post;
                //www.Body.AddModel(user);













                //var client = HttpGenerator.Create<IMyTestApi>("http://cmd-internal.tops001.com");
                //var user = client.GetUser(Guid.Parse("03f53a51-3291-11e6-b28c-288023a0fe60"));


                Console.Read();
            }
        }



    }
}
