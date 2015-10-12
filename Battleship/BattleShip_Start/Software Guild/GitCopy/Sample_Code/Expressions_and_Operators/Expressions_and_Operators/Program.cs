using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expressions_and_Operators
{
    class Program
    {
        static void Main(string[] args)
        {
            Program prog = new Program();

            // Comment out any of the methods that you do not want to execute
            prog.Literals();
            prog.OrderOfOperations();
            prog.Bitwise();
            prog.Assignment();
            prog.Conditional();

            Console.ReadLine();
        }

        public void Literals()
        {
            Console.WriteLine("***** Literals");

            // will print different types of literals in the C# language
            Console.WriteLine("{0}", 123); // integer value
            Console.WriteLine("{0}", true); // boolean value
            Console.WriteLine("{0}", 3.14); // double value
            Console.WriteLine("{0}", 3.14F); // float value
            Console.WriteLine("{0}", 'a'); // character value
            Console.WriteLine("{0}", "HI!"); // string value

            Console.WriteLine();
        }

        public void OrderOfOperations()
        {
            Console.WriteLine("***** Order of Operations");

            // this shows the order of operations
            // in the case of 2 + 3 * 5, 3 * 5 is performed first
            // that result is then added to 2
            Console.WriteLine("{0}", 2 + 3 * 5);

            // using the increment and decrement operators. 
            // this example also shows the order of operations
            int i = 0;
            i++;
            Console.WriteLine(i++);
            Console.WriteLine(--i);
            Console.WriteLine(i);

            Console.WriteLine();
        }

        public void Bitwise()
        {
            Console.WriteLine("***** Bitwise");

            // demonstrate a bitwise AND operator
            // additional code used to display the bit string of a number
            int x = 3, y = 123;
            Console.WriteLine("{0}", x & y);
            Console.WriteLine();
            Console.WriteLine(Convert.ToString(x, 2).PadLeft(8, '0'));
            Console.WriteLine(Convert.ToString(y, 2).PadLeft(8, '0'));
            Console.WriteLine(Convert.ToString(x & y, 2).PadLeft(8, '0'));

            Console.WriteLine();
        }

        public void Assignment()
        {
            Console.WriteLine("***** Assignment");

            // examples of using the assignment operator with another operation.
            int x = 3;
            x *= 2;
            Console.WriteLine("{0}", x);

            x += 2 + 3;
            Console.WriteLine("{0}", x);

            x %= 3;
            Console.WriteLine("{0}", x);

            Console.WriteLine();
        }

        public void Conditional()
        {
            Console.WriteLine("***** Conditional");

            // instead of an if statement, a new way to do a conditional assignment
            int x = 3, y = 2;
            string Message = ((x < y) && (x > 0)) ? "X is smaller" : "X is bigger";

            Console.WriteLine(Message);

            Console.WriteLine();
        }
    }
}
