using DeParnasso.Core.Extensions;
using DeParnasso.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeParnasso.Harmoniser.MatterMethod
{
    public class MatterComposition : Composition
    {
        public List<CipheredMelody> CipherSchemes = new List<CipheredMelody>();
        public List<Composition> Harmonisations = new List<Composition>();
        public Melody Melody => Voices.FirstOrDefault().Melody;
        public MatterComposition(Key key, Meter meter) : base(key, meter) { }
        
        public MatterComposition(Composition composition) : this(composition.Key, composition.Meter)
        {
            Voices = composition.Voices;
        }

        public void AddCipheredMelody(ToneCipherOption lastNoteOption)
        {
            if (Melody.GetFirstToneAfterPosition(lastNoteOption.StartPosition) != null)
            {
                throw new InvalidOperationException("Adding a harmonisation is only allowed when arrived at the last melody note");
            }

            var cipherScheme = new CipheredMelody { new CipheredTone(lastNoteOption) };
            lastNoteOption.PreviousNotes.ForEach(ct => cipherScheme.Add(new CipheredTone(ct)));
            CipherSchemes.Add(cipherScheme);
        }
    }
}
