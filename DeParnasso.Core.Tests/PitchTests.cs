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

            public PitchTestSet(string basePitch, string difference, string modifiedPitch)
            {
                BasePitch = basePitch;
                Difference = difference;
                ModifiedPitch = modifiedPitch;
            }
        }

        [TestMethod]
        public void TestAddIntervals()
        {
            var testSets = new List<PitchTestSet>()
            {
                new PitchTestSet("c", "P1", "c"),
                new PitchTestSet("c", "A1", "C#"),
                new PitchTestSet("c", "d2", "dbb"),
                new PitchTestSet("c", "m2", "db"),
                new PitchTestSet("c", "M2", "d"),
                new PitchTestSet("c", "A2", "d#"),
                new PitchTestSet("c", "d3", "ebb"),
                new PitchTestSet("c", "d4", "fb"),
                new PitchTestSet("c", "P4", "f"),
                new PitchTestSet("c", "A4", "f#"),
                new PitchTestSet("bbb", "A1", "bb"),
                new PitchTestSet("bbb", "m2", "cbb'"),
                new PitchTestSet("bbb", "A3", "d'"),
                new PitchTestSet("c'", "-P1", "c'"),
                new PitchTestSet("c'", "-d2", "b#"),
                new PitchTestSet("c'", "-m2", "b"),
                new PitchTestSet("c'", "-M2", "bb"),
                new PitchTestSet("c'", "-A2", "bbb"),
                new PitchTestSet("c'", "-d3", "a#"),
                new PitchTestSet("c'", "-d4", "g#"),
                new PitchTestSet("c'", "-P4", "g"),
                new PitchTestSet("c'", "-A4", "gb"),
                new PitchTestSet("f*", "-m2", "e*"),
                new PitchTestSet("f*", "-A3", "d"),
            };

            foreach (var pitchTestSet in testSets)
            {
                var basePitch = new Pitch(pitchTestSet.BasePitch);
                var difference = new Interval(pitchTestSet.Difference);
                var modifiedPitch = basePitch.Add(difference);
                Assert.AreEqual(modifiedPitch, new Pitch(pitchTestSet.ModifiedPitch));
                Assert.AreEqual(modifiedPitch.Difference(basePitch), difference);
            }
        }
    }
}
