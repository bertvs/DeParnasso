using DeParnasso.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeParnasso.Core.Tests
{
    [TestClass]
    public class AccentTests : Accent
    {
        [TestMethod]
        public void TestParseDurationFromStringAndBack()
        {
            var testString = "1";
            var testFraction = new Fraction(1);
            ParseDurationFromString(testString);
            Assert.AreEqual(Duration, testFraction);
            Assert.AreEqual(DurationToString(), testString);

            testString = "1.";
            testFraction = new Fraction(3, 2);
            ParseDurationFromString(testString);
            Assert.AreEqual(Duration, testFraction);
            Assert.AreEqual(DurationToString(), testString);
            
            testString = "2";
            testFraction = new Fraction(1, 2);
            ParseDurationFromString(testString);
            Assert.AreEqual(Duration, testFraction);
            Assert.AreEqual(DurationToString(), testString);

            testString = "2.";
            testFraction = new Fraction(3, 4);
            ParseDurationFromString(testString);
            Assert.AreEqual(Duration, testFraction);
            Assert.AreEqual(DurationToString(), testString);

            testString = "4";
            testFraction = new Fraction(1, 4);
            ParseDurationFromString(testString);
            Assert.AreEqual(Duration, testFraction);
            Assert.AreEqual(DurationToString(), testString);

            testString = "4.";
            testFraction = new Fraction(3, 8);
            ParseDurationFromString(testString);
            Assert.AreEqual(Duration, testFraction);
            Assert.AreEqual(DurationToString(), testString);
            
            testString = "8";
            testFraction = new Fraction(1, 8);
            ParseDurationFromString(testString);
            Assert.AreEqual(Duration, testFraction);
            Assert.AreEqual(DurationToString(), testString);

            testString = "8.";
            testFraction = new Fraction(3, 16);
            ParseDurationFromString(testString);
            Assert.AreEqual(Duration, testFraction);
            Assert.AreEqual(DurationToString(), testString);

            testString = "16";
            testFraction = new Fraction(1, 16);
            ParseDurationFromString(testString);
            Assert.AreEqual(Duration, testFraction);
            Assert.AreEqual(DurationToString(), testString);
        }
    }
}
