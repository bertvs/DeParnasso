using DeParnasso.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeParnasso.Harmoniser.MatterMethod
{
    public class ToneCipherOption : CipheredTone
    {
        public List<ToneCipherOption> NextNoteOptions = new List<ToneCipherOption>();
        public List<ToneCipherOption> PreviousNotes = new List<ToneCipherOption>();

        public ToneCipherOption(Tone note, Interval intervalToBass) : base(note, intervalToBass) { }

        public ToneCipherOption AddAsNextNoteOption(ToneCipherOption option)
        {
            option.PreviousNotes.Add(this);
            option.PreviousNotes.AddRange(PreviousNotes);
            NextNoteOptions.Add(option);
            return option;
        }

        public ToneCipherOption GetFirstWithSameBass()
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
