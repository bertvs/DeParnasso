using DeParnasso.Core.Extensions;
using DeParnasso.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeParnasso.Harmoniser.MatterMethod
{
    public class MatterComposition : Composition
    {
        public List<Harmonisation> Harmonisations { get; set; }
        public MatterComposition(Key key, Meter meter) : base(key, meter) { }
        public MatterComposition(string key, Meter meter) : base(key, meter) { }
        public MatterComposition(string key, string meter) : base(key, meter) { }
        public Melody Melody => Voices.FirstOrDefault().Melody;
        public List<HarmonisedNoteOption> BassOptions { get; set; }

        public MatterComposition(Composition composition) : base(composition.Key, composition.Meter)
        {
            Harmonisations = new List<Harmonisation>();
            BassOptions = new List<HarmonisedNoteOption>();
            Voices = composition.Voices;
        }

        public void AddHarmonisation(HarmonisedNoteOption lastNoteOption)
        {
            if (Melody.GetFirstNoteAfterPosition(lastNoteOption.StartPosition) != null)
            {
                throw new InvalidOperationException("Adding a harmonisation is only allowed when arrived at the last melody note");
            }

            var harmonisation = new Harmonisation
            {
                new HarmonisedNote(lastNoteOption)
            };
            lastNoteOption.PreviousNotes.ForEach(hno => harmonisation.Add(new HarmonisedNote(hno)));
            Harmonisations.Add(harmonisation);
        }
    }
}
