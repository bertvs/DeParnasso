using DeParnasso.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeParnasso.Core.Tests
{
    [TestClass]
    public class PitchTests
    {
        [TestMethod]
        public void TestParsePitchFromStringAndBack()
        {
            var testStrings = new string[]
            {
                "C",
                "C#",
                "C*",
                "Dbb'",
                "Db'",
                "D'",
                "D#'",
                "D*'",
                "Ebb''",
                "Eb''",
                "E''",
                "E#''",
                "Fb'''",
                "F'''",
                "F#'''",
                "F*'''",
                "Gbb''''",
                "Gb''''",
                "G''''",
                "G#''''",
                "G*''''",
                "Abb",
                "Ab",
                "A",
                "A#",
                "A*",
                "Bbb",
                "Bb",
                "B",
                "B#",
                "Cb'"
            };

            foreach (var testString in testStrings)
            {
                Assert.AreEqual(testString, new Pitch(testString).ToString());
            }
        }

        [TestMethod]
        public void TestParsePitchFromLyStringAndBack()
        {
            var testStrings = new string[]
            {
                "c",
                "cis",
                "cisis",
                "deses'",
                "des'",
                "d'",
                "dis'",
                "disis'",
                "eeses''",
                "ees''",
                "e''",
                "eis''",
                "fes'''",
                "f'''",
                "fis'''",
                "fisis'''",
                "geses''''",
                "ges''''",
                "g''''",
                "gis''''",
                "gisis''''",
                "aeses",
                "aes",
                "a",
                "ais",
                "aisis",
                "beses",
                "bes",
                "b",
                "bis",
                "ces'"
            };

            foreach (var testString in testStrings)
            {
                Assert.AreEqual(testString, new Pitch(testString).ToLyString());
            }
        }

        [TestMethod]
        public void TestSimplify()
        {
            var testStrings = new string[]
            {
                "C",
                "C#",
                "C*",
                "Dbb'",
                "Db'",
                "D'",
                "D#'",
                "D*'",
                "Ebb''",
                "Eb''",
                "E''",
                "E#''",
                "Fb'''",
                "F'''",
                "F#'''",
                "F*'''",
                "Gbb''''",
                "Gb''''",
                "G''''",
                "G#''''",
                "G*''''",
                "Abb",
                "Ab",
                "A",
                "A#",
                "A*",
                "Bbb",
                "Bb",
                "B",
                "B#",
                "Cb'"
            };

            foreach (var testString in testStrings)
            {
                var pitch1 = new Pitch(testString);
                var pitch2 = pitch1.Simplify();
                Assert.AreEqual(pitch1.ToInt(), pitch2.ToInt());
            }
        }

        [TestMethod]
        public void TestAddIntervals()
        {
            var pitch1 = new Pitch("c");
            var pitch2 = pitch1.Add("P1");
            Assert.AreEqual(pitch2, new Pitch("c"));

            pitch1 = new Pitch("c");
            pitch2 = pitch1.Add("A1");
            Assert.AreEqual(pitch2, new Pitch("c#"));

            pitch1 = new Pitch("c");
            pitch2 = pitch1.Add("d2");
            Assert.AreEqual(pitch2, new Pitch("dbb"));

            pitch1 = new Pitch("c");
            pitch2 = pitch1.Add("m2");
            Assert.AreEqual(pitch2, new Pitch("db"));

            pitch1 = new Pitch("c");
            pitch2 = pitch1.Add("M2");
            Assert.AreEqual(pitch2, new Pitch("d"));

            pitch1 = new Pitch("c");
            pitch2 = pitch1.Add("A2");
            Assert.AreEqual(pitch2, new Pitch("d#"));

            pitch1 = new Pitch("c");
            pitch2 = pitch1.Add("d3");
            Assert.AreEqual(pitch2, new Pitch("ebb"));

            pitch1 = new Pitch("c");
            pitch2 = pitch1.Add("d4");
            Assert.AreEqual(pitch2, new Pitch("fb"));

            pitch1 = new Pitch("c");
            pitch2 = pitch1.Add("P4");
            Assert.AreEqual(pitch2, new Pitch("f"));

            pitch1 = new Pitch("c");
            pitch2 = pitch1.Add("A4");
            Assert.AreEqual(pitch2, new Pitch("f#"));

            pitch1 = new Pitch("bbb");
            pitch2 = pitch1.Add("A1");
            Assert.AreEqual(pitch2, new Pitch("bb"));

            pitch1 = new Pitch("bbb");
            pitch2 = pitch1.Add("m2");
            Assert.AreEqual(pitch2, new Pitch("cbb"));

            pitch1 = new Pitch("bbb");
            pitch2 = pitch1.Add("A3");
            Assert.AreEqual(pitch2, new Pitch("d'"));
        }

        //test subtractintervals

        // test diffs of two pitches
    }
}
