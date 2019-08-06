using DeParnasso.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeParnasso.Harmoniser.MatterMethod
{
    public class CipheredTone : Tone
    {
        public Pitch Bass { get; set; }
        public Interval Distance => Pitch.Difference(Bass);

        public Tone GetBassTone() => new Tone { Pitch = Bass, StartPosition = StartPosition, Duration = Duration };

        public CipheredTone() { }

        public CipheredTone(Tone note, Interval intervalToBass)
        {
            Bass = note.Pitch.Subtract(intervalToBass);
            Duration = note.Duration;
            Pitch = note.Pitch;
            StartPosition = note.StartPosition;
        }

        public CipheredTone(ToneCipherOption noteOption)
        {
            Bass = noteOption.Bass;
            Duration = noteOption.Duration;
            Pitch = noteOption.Pitch;
            StartPosition = noteOption.StartPosition;
        }

        public override string ToString()
        {
            return Distance.ToString();
        }
    }
}
