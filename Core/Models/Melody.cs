using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeParnasso.Core.Models
{
    public class Melody : List<Tone>
    {
        public void AddNote(Tone note)
        {
            note.StartPosition = GetDuration();
            Add(note);
        }
        public void AddNote(string note) => AddNote(new Tone(note));

        public void AddNote(Fraction position, Fraction duration, Pitch pitch)
        {
            if (GetToneAtPosition(position) != null)
            {
                throw new InvalidOperationException("There is already a note at this position in this melody!");
            }
            Add(new Tone
            {
                Duration = duration,
                Pitch = pitch,
                StartPosition = position
            });
        }

        public Fraction GetDuration()
        {
            var lastNote = GetLastNote();
			if (lastNote == null)
			{
				return 0;
			}
            return lastNote.StartPosition + lastNote.Duration;
        }

		public Tone GetToneAtPosition(Fraction position) => this.FirstOrDefault(n => n.StartPosition <= position && n.StartPosition + n.Duration > position);
        public Tone GetFirstToneAfterPosition(Fraction position) => this.Where(n => n.StartPosition > position).OrderBy(n => n.StartPosition).FirstOrDefault();
        public Tone GetLastToneBeforePosition(Fraction position) => this.Where(n => n.EndPosition < position).OrderByDescending(n => n.EndPosition).FirstOrDefault();
        public Tone GetNextTone(Tone note) => GetFirstToneAfterPosition(note.StartPosition);
        public Tone GetPreviousTone(Tone note) => GetLastToneBeforePosition(note.StartPosition);
        public Tone GetLastNote() => this.OrderByDescending(n => n.StartPosition).FirstOrDefault();

        public bool RemoveLastNote() => Remove(GetLastNote());

        public Melody Transpose(Interval interval)
        {
            var newMelody = new Melody();
            ForEach(n => newMelody.Add(n.Transpose(interval)));
            return newMelody;
        }

        public Melody Transpose(string interval) => Transpose(new Interval(interval));
        public override string ToString() => string.Join(' ', this.Select(n => n.ToString()));

        public string ToLyString() => string.Join(' ', this.Select(n => n.ToLyString()));

        public Melody() { }

        public Melody(string input)
        {
            var noteStrings = input.Split(' ');
            foreach (var noteString in noteStrings)
            {
                AddNote(new Tone(noteString));
            }
        }
    }
}
