using DeParnasso.Core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DeParnasso.Core.Tests
{
    [TestClass]
    public class CompositionTests : Composition
    {
        [TestMethod]
        public void TestGetHarmonyAtPosition()
        {
            var testComposition = new Composition("C Major", "4/4");
            testComposition.AddVoice("c''4 e''4 d''4 c''4");
            testComposition.AddVoice("e''4 g''2");
            testComposition.AddVoice("g'8 a'8 b'2");
            testComposition.AddVoice("e'4 d'4 f'4 e'4");
            testComposition.AddVoice("c'4 g4 g4 c'4");

            Assert.AreEqual(testComposition.GetDuration(), new Fraction(1));

            Assert.AreEqual(testComposition.GetHarmonyAtPosition(0), new Harmony("c' e' g' c'' e''"));
            Assert.AreEqual(testComposition.GetHarmonyAtPosition(new Fraction(1,8)), new Harmony("c' e' a' c'' e''"));

            // test at other positions

            // change one voice

            // test again at all positions
        }
    }
}
