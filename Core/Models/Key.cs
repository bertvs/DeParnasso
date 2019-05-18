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
			public class ModeScale : List<Interval>
			{

				// to do: implement instantiation of jazz / gypsy scales

				public static string RegexString => @"([WH]-)*[WH]";

				public ModeScale(string scale)
				{
					if (!Regex.Match(scale, RegexString).Success)
					{
						throw new InvalidCastException($"Could not parse '{scale}' to a KeyScale");
					}

					var steps = scale.Split('-');
					foreach (var step in steps)
					{
						switch (step)
						{
							case "W": Add(new Interval("M2")); break;
							case "H": Add(new Interval("m2")); break;
							default: throw new InvalidCastException($"Could not parse '{step}' to an Interval");
						}
					}

					var totalRange = 0;
					foreach (var interval in this)
					{
						totalRange += (int)interval;
					}
					if (totalRange != 12)
					{
						throw new InvalidCastException($"The steps of this scale do not add up to an octave: {scale}");
					}
				}
			}

			public string Name { get; private set; }
			public ModeScale Scale { get; private set; }
			public bool IsModal { get; private set; }
			public bool IsTonal { get; private set; }

			private KeyMode(string name, ModeScale scale, bool isModal, bool isTonal)
			{
				Name = name;
				Scale = scale;
				IsModal = isModal;
				IsTonal = isTonal;
			}

			public override string ToString() => Name;

			public static KeyMode MAJOR => new KeyMode("major", new ModeScale("W-W-H-W-W-W-H"), false, true);
			public static KeyMode MINOR => new KeyMode("minor", new ModeScale("W-H-W-W-H-W-W"), false, true);
			public static KeyMode IONIAN => new KeyMode("Ionian", new ModeScale("W-W-H-W-W-W-H"), true, false);
			public static KeyMode DORIAN => new KeyMode("Dorian", new ModeScale("W-H-W-W-W-H-W"), true, false);
			public static KeyMode PHRYGIAN => new KeyMode("Phrygian", new ModeScale("H-W-W-W-H-W-W"), true, false);
			public static KeyMode LYDIAN => new KeyMode("Lydian", new ModeScale("W-W-W-H-W-W-H"), true, false);
			public static KeyMode MIXOLYDIAN => new KeyMode("Mixolydian", new ModeScale("W-W-H-W-W-H-W"), true, false);
			public static KeyMode AEOLIAN => new KeyMode("Aeolian", new ModeScale("W-H-W-W-H-W-W"), true, false);

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
		}

		private Pitch Tonic { get; set; }
		private KeyMode Mode { get; set; }

		public Key(string input)
		{
			var parts = input.Split(' ');
			Tonic = new Pitch(parts[0]);
			var mode = KeyMode.ALL.SingleOrDefault(km => km.Name.ToLower() == parts[1].ToLower());
			Mode = mode ?? throw new InvalidCastException($"Could not parse '{input}' to type Key");
		}

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

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(Tonic.ToString());
			sb.Append(" ");
			sb.Append(Mode.ToString());
			return sb.ToString();
		}

		public string ToLyString()
		{
			var sb = new StringBuilder();
			sb.Append(Tonic.ToLyString());
			sb.Append(" \\");
			sb.Append(Mode.ToString());
			return sb.ToString();
		}

		public List<Pitch> GetScale()
		{
			var result = new List<Pitch>();
			result.Add(Tonic);
			var latest = Tonic;
			foreach (var interval in Mode.Scale)
			{
				latest = latest.Add(interval);
				result.Add(latest);
			}
			return result;
		}
	}
}
