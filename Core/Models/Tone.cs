using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DeParnasso.Core.Models
{
    public class Tone : Accent
    {
        public Pitch Pitch { get; set; }

		public static string RegexString => Pitch.RegexString + DurationRegex;

		public Tone() { }

		public Tone(string input)
		{
			var regexResult = Regex.Match(input, RegexString);

			if (!regexResult.Success)
			{
				throw new InvalidCastException($"Could not parse '{input}' to type Tone");
			}

			Pitch = new Pitch(regexResult.Groups[1].Value);
			ParseDurationFromString(regexResult.Groups[6].Value);
		}

		public Tone Transpose(string interval) => Transpose(new Interval(interval));
		
		public Tone Transpose(Interval interval)
		{
			var newNote = this;
			Pitch = Pitch.Add(interval);
			return newNote;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(Pitch.ToString());
			sb.Append(base.ToString());
			return sb.ToString();
		}

		public string ToLyString()
		{
			var sb = new StringBuilder();
			sb.Append(Pitch.ToLyString());
			sb.Append(base.ToString());
			return sb.ToString();
		}
	}
}
