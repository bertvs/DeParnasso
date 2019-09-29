using DeParnasso.Core.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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

        private class PitchTestSet
        {
            public string BasePitch { get; set; }
            public string Difference { get; set; }
            public string ModifiedPitch { get; set; }
        }

        [TestMethod]
        public void TestAddIntervals()
        {
            var testSets = new List<PitchTestSet>()
            {
                new PitchTestSet
                {
                    BasePitch = "c",
                    Difference = "P1",
                    ModifiedPitch = "c"
                },
                new PitchTestSet
                {
                    BasePitch = "c",
                    Difference = "A1",
                    ModifiedPitch = "c#"
                },
            };


            foreach (var pitchTestSet in testSets)
            {
                var basePitch = new Pitch(pitchTestSet.BasePitch);
                var difference = new Interval(pitchTestSet.Difference);
                var modifiedPitch = basePitch.Add(difference);
                Assert.AreEqual(modifiedPitch, new Pitch(pitchTestSet.ModifiedPitch));
                Assert.AreEqual(modifiedPitch.Difference(basePitch), difference);
            }

            var pitch1 = new Pitch("c");
            var interval = new Interval("P1");
            var pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("c"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c");
            interval = new Interval("A1");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("c#"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c");
            interval = new Interval("d2");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("dbb"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c");
            interval = new Interval("m2");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("db"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c");
            interval = new Interval("M2");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("d"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c");
            interval = new Interval("A2");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("d#"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c");
            interval = new Interval("d3");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("ebb"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c");
            interval = new Interval("d4");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("fb"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c");
            interval = new Interval("P4");
            pitch2 = pitch1.Add("P4");
            Assert.AreEqual(pitch2, new Pitch("f"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c");
            interval = new Interval("A4");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("f#"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("bbb");
            interval = new Interval("A1");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("bb"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("bbb");
            interval = new Interval("m2");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("cbb'"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("bbb");
            interval = new Interval("A3");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("d'"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);
        }

        [TestMethod]
        public void TestSubtractIntervals()
        {
            var pitch1 = new Pitch("c'");
            var interval = new Interval("-P1");
            var pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("c'"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            //@TODO Fix this one
            pitch1 = new Pitch("c'");
            interval = new Interval("-A1");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("cb'"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c'");
            interval = new Interval("-d2");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("b#"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c'");
            interval = new Interval("-m2");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("b"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c'");
            interval = new Interval("-M2");
            pitch2 = pitch1.Add("-M2");
            Assert.AreEqual(pitch2, new Pitch("bb"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c'");
            interval = new Interval("-A2");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("bbb"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c'");
            interval = new Interval("-d3");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("a#"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c'");
            interval = new Interval("-d4");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("g#"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c'");
            interval = new Interval("-P4");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("g"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("c'");
            interval = new Interval("-A4");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("gb"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("f*");
            interval = new Interval("-A1");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("f#"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("f*");
            interval = new Interval("-m2");
            pitch2 = pitch1.Add("-m2");
            Assert.AreEqual(pitch2, new Pitch("e*"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);

            pitch1 = new Pitch("f*");
            interval = new Interval("-A3");
            pitch2 = pitch1.Add(interval);
            Assert.AreEqual(pitch2, new Pitch("d"));
            Assert.AreEqual(pitch2.Difference(pitch1), interval);
        }
    }
}
