using DeParnasso.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeParnasso.Harmoniser.MatterMethod
{
    public class HarmonisedNote : Note
    {
        public Pitch Bass { get; set; }
        public Interval Distance => Pitch.Difference(Bass);

        public HarmonisedNote() { }

        public HarmonisedNote(Note note, Interval intervalToBass)
        {
            Bass = note.Pitch.Subtract(intervalToBass);
            Duration = note.Duration;
            Pitch = note.Pitch;
            StartPosition = note.StartPosition;
        }

        public HarmonisedNote(HarmonisedNoteOption noteOption)
        {
            Bass = noteOption.Bass;
            Duration = noteOption.Duration;
            Pitch = noteOption.Pitch;
            StartPosition = noteOption.StartPosition;
        }
    }
}
