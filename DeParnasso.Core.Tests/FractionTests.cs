using DeParnasso.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeParnasso.Core.Tests
{
    [TestClass]
    public class FractionTests
    {
        [TestMethod]
        public void TestSumsWithZero()
        {
            var frac1 = new Fraction(0);

            var frac2 = new Fraction(1);
            Assert.AreEqual(frac2, frac1 + frac2);

            frac2 = new Fraction(1, 2);
            Assert.AreEqual(frac2, frac1 + frac2);

            frac2 = new Fraction(3, 5);
            Assert.AreEqual(frac2, frac1 + frac2);

            frac2 = new Fraction(2053, 123);
            Assert.AreEqual(frac2, frac1 + frac2);
        }

        [TestMethod]
        public void TestSums()
        {
            var frac1 = new Fraction(5823);
            var frac2 = new Fraction(324, 8);
            var sum = frac1 + frac2;
            var newNumerator = 5823 * 8 + 324;
            Assert.AreEqual(sum, new Fraction((ulong)newNumerator, 8));
        }

        [TestMethod]
        public void TestSubtractZero()
        {
            var frac1 = new Fraction(0);

            var frac2 = new Fraction(7);
            Assert.AreEqual(frac2, frac2 - frac1);

            frac2 = new Fraction(8, 9);
            Assert.AreEqual(frac2, frac2 - frac1);

            frac2 = new Fraction(9, 2);
            Assert.AreEqual(frac2, frac2 - frac1);

            frac2 = new Fraction(253, 123);
            Assert.AreEqual(frac2, frac2 - frac1);
        }

        [TestMethod]
        public void TestSubtracts()
        {
            var frac1 = new Fraction(5923);
            var frac2 = new Fraction(324, 8);
            var diff = frac1 - frac2;
            var newNumerator = 5923 * 8 - 324;
            Assert.AreEqual(diff, new Fraction((ulong)newNumerator, 8));
        }
    }
}
