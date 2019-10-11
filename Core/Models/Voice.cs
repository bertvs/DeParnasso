using DeParnasso.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace DeParnasso.Core.Models
{
    public class Voice
    {
        public string Name { get; set; }
        public Melody Melody { get; set; }

        public Voice(string name, Melody melody)
        {
            Name = name;
            Melody = melody;
        }

        public Voice(string name, string melody) : this(name, new Melody(melody)) { }
    }
}
