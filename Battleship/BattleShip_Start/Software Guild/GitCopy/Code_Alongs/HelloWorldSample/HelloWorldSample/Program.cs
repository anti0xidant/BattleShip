using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorldSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = "Hello World!";
            string name = "Victor";

            Console.WriteLine("Hello World!");
            Console.WriteLine("{0} {1}", message, name);
            Console.WriteLine(message);

            Console.ReadLine();
        }
    }
}
