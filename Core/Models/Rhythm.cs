using System;
using System.Collections.Generic;
using System.Linq;

namespace DeParnasso.Core.Models
{
    public class Rhythm : List<Accent>
    {
        public void AddAccent(Fraction position, Fraction duration)
        {
            if (GetAccentAtPosition(position) != null)
            {
                throw new InvalidOperationException("There is already an accent at this position in this rhythm!");
            }
            Add(new Accent
            {
                Duration = duration,
                StartPosition = position
            });
        }
        public void AddAccent(Accent accent)
        {
            accent.StartPosition = GetTotalDuration();
            Add(accent);
        }

        public Fraction GetTotalDuration()
        {
            var lastAccent = GetLastAccent();
            return lastAccent.StartPosition + lastAccent.Duration;
        }

        public Accent GetLastAccent() => this.OrderByDescending(a => a.StartPosition).FirstOrDefault();
        
        public bool RemoveLastNote() => Remove(GetLastAccent());

        public Accent GetAccentAtPosition(Fraction position) => this.FirstOrDefault(a => a.StartPosition <= position && a.StartPosition + a.Duration > position);
    }
}
