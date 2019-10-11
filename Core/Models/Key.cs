using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DeParnasso.Core.Models
{
    public sealed class Key : IEquatable<Key>
    {
		private class KeyMode
		{
            public static string ScaleRegexString => @"([WH]-)*[WH]";

            public static KeyMode MAJOR => new KeyMode("major", "W-W-H-W-W-W-H", false, true);
            public static KeyMode MINOR => new KeyMode("minor", "W-H-W-W-H-W-W", false, true);
            public static KeyMode IONIAN => new KeyMode("Ionian", "W-W-H-W-W-W-H", true, false);
            public static KeyMode DORIAN => new KeyMode("Dorian", "W-H-W-W-W-H-W", true, false);
            public static KeyMode PHRYGIAN => new KeyMode("Phrygian", "H-W-W-W-H-W-W", true, false);
            public static KeyMode LYDIAN => new KeyMode("Lydian", "W-W-W-H-W-W-H", true, false);
            public static KeyMode MIXOLYDIAN => new KeyMode("Mixolydian", "W-W-H-W-W-H-W", true, false);
            public static KeyMode AEOLIAN => new KeyMode("Aeolian", "W-H-W-W-H-W-W", true, false);

            public static List<KeyMode> ALL => new List<KeyMode>
            {
                MAJOR,
                MINOR,
                IONIAN,
                DORIAN,
                PHRYGIAN,
                LYDIAN,
                MIXOLYDIAN,
                AEOLIAN
            };

            //@TODO: implement jazz and gypsy modes
            public string Name { get; private set; }
			public List<Interval> Scale { get; set; }
			public bool IsModal { get; private set; }
			public bool IsTonal { get; private set; }

            public override string ToString() => Name;

            private KeyMode(string name, string scale, bool isModal, bool isTonal)
            {
                Name = name;
                Scale = ConvertToScale(scale);
                IsModal = isModal;
                IsTonal = isTonal;
            }

            private List<Interval> ConvertToScale(string scale)
            {
                var result = new List<Interval>();

                if (!Regex.Match(scale, ScaleRegexString).Success)
                {
                    throw new InvalidCastException($"Could not parse '{scale}' to a KeyScale");
                }

                var steps = scale.Split('-');
                foreach (var step in steps)
                {
                    switch (step)
                    {
                        case "W": result.Add(new Interval("M2")); break;
                        case "H": result.Add(new Interval("m2")); break;
                        default: throw new InvalidCastException($"Could not parse '{step}' to an Interval");
                    }
                }

                var totalRange = 0;
                foreach (var interval in result)
                {
                    totalRange += (int)interval;
                }
                if (totalRange != 12)
                {
                    throw new InvalidCastException($"The steps of this scale do not add up to an octave: {scale}");
                }
                return result;
            }
        }

		public Pitch Tonic { get; set; }
		private KeyMode Mode { get; set; }

        public Pitch Dominant => Tonic.Add("P5");

        public bool Equals(Key other) => (Tonic == other.Tonic && Mode == other.Mode);

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Key))
            {
                return false;
            }
            return Equals((Key)obj);
        }

        public override int GetHashCode() => Convert.ToInt32((Tonic.ToInt()) & 0xFFFFFFFF);

        public List<Pitch> GetScale()
        {
            var result = new List<Pitch>
            {
                Tonic
            };

            var latest = Tonic;
            foreach (var interval in Mode.Scale)
            {
                latest = latest.Add(interval);
                result.Add(latest.PitchClass);
            }
            return result;
        }

        public string ToLyString()
        {
            var sb = new StringBuilder();
            sb.Append(Tonic.ToLyString());
            sb.Append(" \\");
            sb.Append(Mode.ToString());
            return sb.ToString();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Tonic.ToString());
            sb.Append(" ");
            sb.Append(Mode.ToString());
            return sb.ToString();
        }

        public Key(string input)
		{
			var parts = input.Split(' ');

            if (parts == null || parts.Count() < 2)
                throw new InvalidCastException($"Could not parse '{input}' to type Key");

            Tonic = new Pitch(parts[0]);
			var mode = KeyMode.ALL.SingleOrDefault(km => km.Name.ToLower() == parts[1].ToLower());
			Mode = mode ?? throw new InvalidCastException($"Could not parse '{input}' to type Key");
		}
	}
}
