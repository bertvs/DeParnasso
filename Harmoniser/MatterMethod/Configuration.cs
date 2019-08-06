using DeParnasso.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeParnasso.Harmoniser.MatterMethod
{
    public class Configuration
    {
        public static List<Interval> AllowedIntervals => new List<Interval>
        {
            new Interval("m3"),
            new Interval("M3"),
            new Interval("P5"),
            new Interval("P1")
        };

        public bool AllowConsecutives { get; set; }
        public bool AllowHiddenConsecutives { get; set; }
        public bool FinishOnTonicOrDominantOnly { get; set; }

        public Configuration(bool allowConsecutives = false, bool allowHiddenConsecutives = false, bool finishOnTonicOrDominantOnly = true)
        {
            AllowConsecutives = allowConsecutives;
            AllowHiddenConsecutives = allowHiddenConsecutives;
            FinishOnTonicOrDominantOnly = finishOnTonicOrDominantOnly;
        }
    }
}
