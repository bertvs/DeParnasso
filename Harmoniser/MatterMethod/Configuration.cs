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
            new Interval("M3"),
            new Interval("P5"),
            new Interval("P1")
        };

        public bool AllowParallells { get; set; }
        public bool AllowHiddenParallells { get; set; }

        public Configuration(bool allowParallells = false, bool allowHiddenParallells = false)
        {
            AllowParallells = allowParallells;
            AllowHiddenParallells = allowHiddenParallells;
        }
    }
}
