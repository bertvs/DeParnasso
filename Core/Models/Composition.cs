using DeParnasso.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace DeParnasso.Core.Models
{
    public class Composition
    {
        protected class CompositionHarmony : Accent
        {
            public Harmony Harmony { get; set; }

            public override string ToString() => Harmony.ToString();
        }

        public Key Key { get; set; }
        public Meter Meter { get; set; }
        public List<Voice> Voices = new List<Voice>();
        protected List<CompositionHarmony> Harmonies = new List<CompositionHarmony>();

        public void AddVoice(string name, Melody melody)
        {
            OnChangeVoice();

            if (Voices.Any(v => v.Name == name))
            {
                throw new InvalidOperationException($"Voice with name {name} already exists in this composition!");
            }
            Voices.Add(new Voice(name, melody));
        }

        public void AddVoice(string name, string melody) => AddVoice(name, new Melody(melody));

        public void AddVoice(Melody melody)
        {
            OnChangeVoice();

            var voiceNr = 1;
            var existingNumbered = Voices.Where(v => v.Name.Contains("Voice"));
            if (existingNumbered.Any())
            {
                voiceNr += existingNumbered.Max(v => v.Name.Substring(5).TryGetInt()) ?? 0;
            }
            Voices.Add(new Voice("Voice" + voiceNr, melody));
        }

        public void AddVoice(string melody) => AddVoice(new Melody(melody));

        public Fraction GetDuration()
        {
            var result = (Fraction)0;
            foreach (var voice in Voices)
            {
                result = Fraction.Max(result, voice.Melody.GetDuration());
            }
            return result;
        }

        public Harmony GetHarmonyAtPosition(Fraction position)
        {
            var result = Harmonies.FirstOrDefault(h => h.StartPosition <= position && h.StartPosition + h.Duration > position);

            if (result == null)
            {
                result = DetermineHarmonyAtPosition(position);
            }

            return result.Harmony;
        }

        public bool IsStrongNote(Tone note) => (note.StartPosition % Meter.BeatsPerBar == 0);

        public Composition() { }

        public Composition(Key key, Meter meter)
        {
            Key = key;
            Meter = meter;
        }

        public Composition(string key, Meter meter) : this(new Key(key), meter) { }

        public Composition(string key, string meter) : this(key, new Meter(meter)) { }

        private void OnChangeVoice()
        {
            Harmonies = new List<CompositionHarmony>();
        }

        private CompositionHarmony DetermineHarmonyAtPosition(Fraction position)
        {
            var harmony = new CompositionHarmony
            {
                Harmony = new Harmony(),
                StartPosition = 0,
                Duration = GetDuration()
            };

            foreach (var voice in Voices)
            {
                var noteAtPosition = voice.Melody.GetToneAtPosition(position);

                if (noteAtPosition == null)
                {
                    var firstNoteAfterPosition = voice.Melody.GetFirstToneAfterPosition(position);
                    var lastNoteBeforePosition = voice.Melody.GetLastToneBeforePosition(position);

                    if (firstNoteAfterPosition != null)
                    {
                        harmony.Duration = Fraction.Min(harmony.EndPosition, firstNoteAfterPosition.StartPosition) - harmony.StartPosition;
                    }

                    if (lastNoteBeforePosition != null)
                    {
                        harmony.StartPosition = Fraction.Max(harmony.StartPosition, lastNoteBeforePosition.EndPosition);
                    }
                }
                else
                {
                    harmony.Harmony.Add(noteAtPosition.Pitch);
                    harmony.StartPosition = Fraction.Max(harmony.StartPosition, noteAtPosition.StartPosition);
                    harmony.Duration = Fraction.Min(harmony.EndPosition, noteAtPosition.EndPosition) - harmony.StartPosition;
                }
            }

            Harmonies.Add(harmony);
            return harmony;
        }
    }
}
