using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeParnasso.Core.Models
{
    public class Melody : List<Note>
    {
		public Melody() { }

		public Melody(string input)
		{
			var noteStrings = input.Split(' ');
			foreach (var noteString in noteStrings)
			{
				AddNote(new Note(noteString));
			}
		}

		public override string ToString() => string.Join(' ', this.Select(n => n.ToString()));
		
		public string ToLyString() => string.Join(' ', this.Select(n => n.ToLyString()));

		public void AddNote(Note note)
		{
			note.StartPosition = GetDuration();
			Add(note);
		}

        public void AddNote(string note) => AddNote(new Note(note));

        public void AddNote(Fraction position, Fraction duration, Pitch pitch)
        {
            if (GetNoteAtPosition(position) != null)
            {
                throw new InvalidOperationException("There is already a note at this position in this melody!");
            }
            Add(new Note
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

        public Note GetLastNote() => this.OrderByDescending(n => n.StartPosition).FirstOrDefault();

        public bool RemoveLastNote() => Remove(GetLastNote());

        public Melody Transpose(Interval interval)
        {
			var newMelody = new Melody();
			ForEach(n => newMelody.Add(n.Transpose(interval)));
			return newMelody;
        }

		public Melody Transpose(string interval) => Transpose(new Interval(interval));

		public Note GetNoteAtPosition(Fraction position) => this.FirstOrDefault(n => n.StartPosition <= position && n.StartPosition + n.Duration > position);
        public Note GetFirstNoteAfterPosition(Fraction position) => this.Where(n => n.StartPosition > position).OrderBy(n => n.StartPosition).FirstOrDefault();
        public Note GetLastNoteBeforePosition(Fraction position) => this.Where(n => n.EndPosition < position).OrderByDescending(n => n.EndPosition).FirstOrDefault();
        public Note GetNextNote(Note note) => GetFirstNoteAfterPosition(note.StartPosition);
        public Note GetPreviousNote(Note note) => GetLastNoteBeforePosition(note.StartPosition);
        
    }
}
