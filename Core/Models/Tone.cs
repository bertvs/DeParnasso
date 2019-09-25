using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DeParnasso.Core.Models
{
    public class Tone : Accent
    {
        public Pitch Pitch { get; set; }

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

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Pitch.ToString());
            sb.Append(DurationToString());
            return sb.ToString();
        }

        public string ToLyString()
        {
            var sb = new StringBuilder();
            sb.Append(Pitch.ToLyString());
            sb.Append(DurationToString());
            return sb.ToString();
        }

        public Interval Difference(Tone other, bool octaveNeutral = false, bool absolute = false) => Pitch.Difference(other.Pitch, octaveNeutral, absolute);

        public Tone Add(String interval) => new Tone { Pitch = Pitch.Add(interval), Duration = Duration, StartPosition = StartPosition };

		public Tone Transpose(string interval) => Transpose(new Interval(interval));
		
		public Tone Transpose(Interval interval)
		{
			var newNote = this;
			Pitch = Pitch.Add(interval);
			return newNote;
		}
    }
}
