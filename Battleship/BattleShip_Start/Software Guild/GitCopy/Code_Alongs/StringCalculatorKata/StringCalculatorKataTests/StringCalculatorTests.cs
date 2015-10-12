using NUnit.Framework;
using StringCalculatorKata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculatorKataTests
{
    [TestFixture]
    public class StringCalculatorTests
    {
        private StringCalculator _stringCalculator;

        [SetUp]
        public void BeforeEachTest()
        {
            _stringCalculator = new StringCalculator();
            Console.WriteLine("SetUp Called.");
        }

        [Test]
        public void Add_EmptyString_ReturnZero()
        {
            int result = _stringCalculator.Add("");

            Assert.AreEqual(0, result);
        }

        [TestCase("1", 1)]
        [TestCase("2", 2)]
        public void Add_OneNumber_ReturnSelf(string numbers, int expectedResult)
        {
            int result = _stringCalculator.Add(numbers);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("1,2", 3)]
        [TestCase("2,3", 5)]
        public void Add_TwoNumbers_ReturnSum(string numbers, int expectedResult)
        {
            int result = _stringCalculator.Add(numbers);

            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("1,2,3", 6)]
        public void Add_UnknownNumbers_ReturnSum(string numbers, int expectedResult)
        {
            int result = _stringCalculator.Add(numbers);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
