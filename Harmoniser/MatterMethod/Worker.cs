using DeParnasso.Core.Models;
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
            {
                return;
            }

            FindHarmonies();
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

        private void FindHarmonies()
        {
            
            foreach (var intervalOption in Configuration.AllowedIntervals)
            {
                var firstNote = Music.Melody.FirstOrDefault(n => n.IsFirst);
                var option = new HarmonisedNoteOption(firstNote, intervalOption);

                if (!Music.Key.GetScale().Contains(option.Bass.PitchClass))
                {
                    continue;
                }

                Music.BassOptions.Add(option);
                var nextnote = Music.Melody.GetNextNote(firstNote);
                FindHarmoniesForNote(nextnote, option);
            }
            
        }

        private void FindHarmoniesForNote(Note note, HarmonisedNoteOption previous)
        {
            foreach (var intervalOption in Configuration.AllowedIntervals)
            {
                if (!Configuration.AllowParallells && previous.Distance == intervalOption)
                {
                    continue;
                }

                // check bedekte parallel
                if (!Configuration.AllowHiddenParallells)
                {
                    var prePrevious = previous.GetFirstWithSameBass();

                    if (prePrevious.Distance == intervalOption)
                    {
                        continue;
                    }
                }

                var option = new HarmonisedNoteOption(note, intervalOption);

                if (!Music.Key.GetScale().Contains(option.Bass.PitchClass))
                {
                    continue;
                }

                // from here: valid, we add it as an option
                var validOption = previous.AddAsNextNoteOption(option);
                var nextNote = Music.Melody.GetNextNote(note);

                if (nextNote == null) // end of melody, success!
                {
                    Music.AddHarmonisation(validOption);
                }
                else
                {
                    FindHarmoniesForNote(nextNote, validOption);
                }
                
            }
        }


        //function getNextChords(positionHarmony)
        //        {
        //            var foundone = false;
        //            foreach (var option in options) // 8, 5, of 3
        //            {


        //                if () // geen bedekte parallel. als vorige akkoord zelfde is als daarvoor, ook niet zelfde option als voorvorige nemen. Telt ook door: voor-vorige...
        //                {
        //                    continue;
        //                }

        //                if () // geen sprongen in dezelfde richting: dan alleen 3 toegestaan.
        //                {
        //                    continue;
        //                }

        //                if () // laatste harmonie: alleen akkoord op I of V toestaan.
        //                {
        //                    continue;
        //                }
        //                if () // geen eindeloze herhalingen: als pendel abab is geweest, geen a meer toestaan.
        //                {
        //                    continue;
        //                }
        //                foundone = true;
        //                positionHarmony.Add(option);

        //                if (is last chord)
        //                {
        //                    register sequence as option
        //                }
        //                else
        //                {
        //                    getChords(positionHarmony.Next());
        //                }

        //            }

        //            if (foundone == false)
        //            {
        //                positionHarmony.Remove();    // dead end.
        //            }


        //        }

        //        voorbeeld. 12 vd 48 opties.
        //- VII trap toestaan??? alleen in 1e omkering.let op bastoon en parallellen.
        //- bij melodiesprong geen sekundgang bas in zelfde richting of sprong in bas naar 8 of 5

        //f 8F              5Bb                    3d-
        //c 5F  3a-         8C?        3a-         8C             5F-
        //a 3F- 8a  5d-     5d     3F- 8a  5d-     5d     3F-     8a      3F-
        //g 5C- 5C- 8g 3e-? 8g 3e- 5C- 5C- 8g  3e- 8g 3e- 8g  5C- 5C  3e- 8g  5C-
        //f 8F- 8F- X  8F-  X  8F- 8F- 8F- X   8F- X  8F- X   8F- 8F- 8F- X   8F- 
        //  1   2      3X      4X  5X  6       7X     8X      9   10X 11X     12X
        //4 goede.

    }
}
