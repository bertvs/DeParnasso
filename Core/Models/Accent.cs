using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DeParnasso.Core.Models
{
    public class Accent
    {
		public static string DurationRegex => @"((\d+)(\.*))";
        public Fraction Duration { get; set; }
        public Fraction StartPosition { get; set; }
        public Fraction EndPosition => StartPosition + Duration;
        public bool IsFirst => StartPosition == new Fraction(0);

		public override string ToString()
		{
			if (Duration.Numerator == 1)
			{
				return Duration.Denominator.ToString();
			}
			else if (Duration.Numerator == 3)
			{
				var sb = new StringBuilder();
				sb.Append((Duration.Denominator / 2).ToString()).Append(".");
				return sb.ToString();
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		public void ParseDurationFromString(string input)
		{
			var regexResult = Regex.Match(input, DurationRegex);
			if (!regexResult.Success)
			{
				throw new InvalidCastException($"Could not parse '{input}' to a Duration");
			}
			if (regexResult.Groups[3].Value == ".")
			{
				Duration = new Fraction(3, Convert.ToUInt64(regexResult.Groups[2].Value) * 2);
			}
			else if (regexResult.Groups[3].Value == "")
			{
				Duration = new Fraction(1, Convert.ToUInt64(Convert.ToUInt64(regexResult.Groups[2].Value)));
			}
			else
			{
				throw new InvalidCastException($"Could not parse '{input}' to a Duration");
			}
		}
	}
}
