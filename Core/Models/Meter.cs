using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DeParnasso.Core.Models
{
    public class Meter
    {
        public static string RegexString => @"(\d*)(\/)(\d*)";

        public ushort BeatsPerBar { get; set; }
        public ushort BeatUnit { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(BeatsPerBar);
            sb.Append('/');
            sb.Append(BeatUnit);
            return sb.ToString();
        }

        public Meter(ushort beatsPerBar, ushort beatUnit)
        {
            BeatsPerBar = beatsPerBar;
            BeatUnit = beatUnit;
        }

        public Meter(string input)
        {
            var regexResult = Regex.Match(input, RegexString);

            if (!regexResult.Success)
            {
                throw new InvalidCastException($"Could not parse '{input}' to type Meter");
            }

            BeatsPerBar = Convert.ToUInt16(regexResult.Groups[1].Value);
            BeatUnit = Convert.ToUInt16(regexResult.Groups[3].Value);
        }
    }
}
