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
                Console.WriteLine(Bing.Translate2("hello world"));
                //var user = new User("03f53a51-3291-11e6-b28c-288023a0fe60");












                Console.Read();
            }
        }



    }
}
