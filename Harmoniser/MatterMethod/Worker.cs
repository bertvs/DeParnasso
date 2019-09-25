using DeParnasso.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace DeParnasso.Harmoniser.MatterMethod
{
    public class Worker
    {
        public Configuration Configuration { get; set; }
		private MatterComposition Music { get; set; }

        public Worker(Configuration configuration, Composition music)
        {
            Configuration = configuration;
            Music = new MatterComposition(music);
        }

        public void Execute()
        {
            if (!ValidateMusic())
                return;

            FindValidSchemes();
            GenerateMusicFromSchemes();
        }

        private bool ValidateMusic()
        {
            if (Music.Voices.Count != 1)
            {
                return false;
            }

            if (Music.Key == null)
            {
                return false;
            }

            if (Music.Voices.FirstOrDefault().Melody.Count < 3)
            {
                return false;
            }

            return true;
        }

        private void FindValidSchemes()
        {
            
            foreach (var intervalOption in Configuration.AllowedIntervals)
            {
                var firstTone = Music.Melody.FirstOrDefault(n => n.IsFirst);
                var option = new ToneCipherOption(firstTone, intervalOption);

                if (!Music.Key.GetScale().Contains(option.Bass.PitchClass))
                {
                    continue;
                }

                var nextTone = Music.Melody.GetNextTone(firstTone);
                FindValidCipherForTone(nextTone, option);
            }   
        }

        // soprano, alto, tenor harmonisations
        public void GenerateMusicFromSchemes()
        {
            foreach (var cipherScheme in Music.CipherSchemes)
            {
                // initialise variant
                var variant = new Composition(Music.Key, Music.Meter);
                variant.AddVoice("Melody", Music.Melody);

                // generate bassLine
                var bassLine = new Melody();

                var previousBassTone = default(Tone);

                foreach (var cipheredTone in cipherScheme)
                {
                    var bassTone = cipheredTone.GetBassTone();

                    if (previousBassTone != default(Tone))
                    {
                        bassTone.Pitch = bassTone.Pitch.GetClosestFromPitchClass(previousBassTone.Pitch);

                        // except: outside boundaries (too close to melody / too low)
                    }

                    bassLine.Add(previousBassTone = bassTone);
                }
                
                variant.AddVoice("Bass", bassLine);

                // fill in chords

                // add variant
                Music.Harmonisations.Add(variant);
            }
        }

        private void FindValidCipherForTone(Tone tone, ToneCipherOption previous)
        {
            foreach (var intervalOption in Configuration.AllowedIntervals)
            {
                // check consecutives
                if (!Configuration.AllowConsecutives 
                    && tone.Pitch != previous.Pitch
                    && previous.Distance.DiatonicDistance == intervalOption.DiatonicDistance)
                {
                    continue;
                }

                // check hidden consecutives
                if (!Configuration.AllowHiddenConsecutives
                    && tone.Pitch != previous.Pitch)
                {
                    var prePrevious = previous.GetFirstWithSameBass();

                    if (prePrevious.Distance.DiatonicDistance == intervalOption.DiatonicDistance)
                    {
                        continue;
                    }
                }

                var option = new ToneCipherOption(tone, intervalOption);
                var scale = Music.Key.GetScale();

                // bass should be in scale
                if (!option.Bass.IsInScale(scale))
                {
                    continue;
                }

                // fifth of bass should be in scale
                if (!option.Bass.Add("P5").IsInScale(scale))
                {
                    continue;
                }

                // from here: valid, we add it as an option
                var validOption = previous.AddAsNextNoteOption(option);
                var nextNote = Music.Melody.GetNextTone(tone);

                if (nextNote != null)
                {
                    // go on with next note
                    FindValidCipherForTone(nextNote, validOption);
                }
                else // end of melody
                {
                    if (Configuration.FinishOnTonicOrDominantOnly
                        && option.Bass.PitchClass != Music.Key.Tonic
                        && option.Bass.PitchClass != Music.Key.Dominant)
                    {
                        // no valid harmonisation
                        continue;
                    }

                    // success
                    Music.AddCipheredMelody(validOption);
                }
            }
        }

        //        voorbeeld. 12 vd 48 opties.

        //f 8F              5Bb                    3d-
        //c 5F  3a-         8C?        3a-         8C             5F-
        //a 3F- 8a  5d-     5d     3F- 8a  5d-     5d     3F-     8a      3F-
        //g 5C- 5C- 8g 3e-? 8g 3e- 5C- 5C- 8g  3e- 8g 3e- 8g  5C- 5C  3e- 8g  5C-
        //f 8F- 8F- X  8F-  X  8F- 8F- 8F- X   8F- X  8F- X   8F- 8F- 8F- X   8F- 
        //  1   2      3X      4X  5X  6       7X     8X      9   10X 11X     12X
        //4 goede.

    }
}
