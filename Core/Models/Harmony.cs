using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeParnasso.Core.Models
{
    public class Harmony : List<Pitch>
    {
		public Harmony() { }
		
		public Harmony(string input)
		{
			var pitches = input.Split(' ');
			foreach (var pitchString in pitches)
			{
				Add(new Pitch(pitchString));
			}
		}

		public override string ToString() => string.Join(' ', this.Select(p => p.ToString()));

		public string ToLyString() => string.Join(' ', this.Select(p => p.ToLyString()));
		
		public Pitch GetBase() => this.OrderBy(pitch => (uint)pitch).FirstOrDefault();

		public Pitch GetRoot()
        {
            throw new NotImplementedException();
        }

		public Harmony Transpose(string interval) => Transpose(new Interval(interval));

		public Harmony Transpose(Interval interval)
		{
			var newHarmony = new Harmony();
			ForEach(p => newHarmony.Add(p.Add(interval)));
			return newHarmony;
		}
    }
}
