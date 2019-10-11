using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeParnasso.Core.Models
{
    public class Harmony : List<Pitch>, IEquatable<Harmony>
    {
        public bool Equals(Harmony other)
        {
            return ToString() == other.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Harmony))
                return false;

            return Equals((Harmony)obj);
        }

        public Pitch GetBase() => this.OrderBy(pitch => (uint)pitch).FirstOrDefault();

        public Pitch GetRoot()
        {
            throw new NotImplementedException();
        }
        public string ToLyString() => string.Join(' ', this.Select(p => p.ToLyString()));

        public override string ToString() => string.Join(' ', this.OrderBy(p => p.ToInt()).Select(p => p.ToString()));

        public Harmony Transpose(string interval) => Transpose(new Interval(interval));

        public Harmony Transpose(Interval interval)
        {
            var newHarmony = new Harmony();
            ForEach(p => newHarmony.Add(p.Add(interval)));
            return newHarmony;
        }

        public Harmony() { }
		
		public Harmony(string input)
		{
			var pitches = input.Split(' ');
			foreach (var pitchString in pitches)
			{
				Add(new Pitch(pitchString));
			}
		}
    }
}
