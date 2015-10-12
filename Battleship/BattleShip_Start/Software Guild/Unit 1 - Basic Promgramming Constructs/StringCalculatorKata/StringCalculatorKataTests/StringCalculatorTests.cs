using NUnit.Framework;
using StringCalculatorKata;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculatorKataTests
{
    [TestFixture] //tells resharper this is a class of tests
    public class StringCalculatorTests
    {
        private StringCalculator _stringCalculator;

        [SetUp]
        public void BeforeEachTest()
        {
            _stringCalculator = new StringCalculator();
            Console.WriteLine("SetUp Called");
        }

        [Test]
        public void Add_EmptyString_ReturnZero()
        {
            StringCalculator stringCalc = new StringCalculator();

            int result = StringCalculator.Add("");

            Assert.AreEqual(0, result);
        }

        [TestCase("1", 1)] //can pass in multiple test cases
        [TestCase("2", 2)]
        [TestCase("", 0)]
        [TestCase("c", 44)] //parses c into ascii number
        public void Add_OneNumber_ReturnSelf(string blah, int expectedResult)
        {
            int result = StringCalculator.Add(blah);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("1,2", 3)]
        [TestCase("2,3", 5)]
        [TestCase("1, 2,3", 6)] //parses into "1", "", "2", "3"
        [TestCase("1, ,2", 3)] //needs to ahve something inside
        public void Add_TwoNumbers_ReturnSum(string numbers, int expectedResult)
        {
            int result = StringCalculator.Add(numbers);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("1,2,0", 3)]
        public void Add_UnknownNumbers_ReturnSum(string numbers, int expectedResult)
            //I've already done this above. Tested 3
        {
            int result = StringCalculator.Add(numbers);

            Assert.AreEqual(expectedResult, result);
        }
        [TestCase("1,2\n3", 6)]
        public void Add_UnknownNumbers_With_various_Seperator(string numbers, int expectedResult)
        {
            int result = StringCalculator.Add(numbers);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("//;\n1;2",3)]
        public void kataProblem4(string numbers, int expectedResult)
        {
            int result = StringCalculator.Add(numbers);

            Assert.AreEqual(expectedResult, result);
        }

    }
}
