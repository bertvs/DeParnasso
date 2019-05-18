using DeParnasso.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeParnasso.Harmoniser.MatterMethod
{
    public class HarmonisedNoteOption : HarmonisedNote
    {
        public List<HarmonisedNoteOption> NextNoteOptions = new List<HarmonisedNoteOption>();
        public List<HarmonisedNoteOption> PreviousNotes = new List<HarmonisedNoteOption>();

        public HarmonisedNoteOption(Note note, Interval intervalToBass) : base(note, intervalToBass) { }

        public HarmonisedNoteOption AddAsNextNoteOption(HarmonisedNoteOption option)
        {
            option.PreviousNotes.Add(this);
            option.PreviousNotes.AddRange(PreviousNotes);
            NextNoteOptions.Add(option);
            return option;
        }

        public HarmonisedNoteOption GetFirstWithSameBass()
        {
            var lastPrevious = this;

            foreach (var prePreviousNote in PreviousNotes.OrderByDescending(hno => hno.StartPosition))
            {
                if (Bass != prePreviousNote.Bass)
                {
                    break;
                }
                else
                {
                    lastPrevious = prePreviousNote;
                }
            }
            return lastPrevious;
        }
    }
}
