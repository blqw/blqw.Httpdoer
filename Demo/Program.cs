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
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine(Bind.Translate("hello world"));

                Console.Read();
            }
        }



    }
}
