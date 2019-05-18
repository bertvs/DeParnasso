﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DeParnasso.Core.Models
{
    public sealed class Pitch : IEquatable<Pitch>
    {
        private class NaturalTone : IEquatable<NaturalTone>
        {
            public string Name { get; private set; }
            public string LatinName { get; private set; }
            public ushort DiatonicNumber { get; private set; }
            public ushort Semitones { get; private set; }

			private NaturalTone(string name, string latinName, ushort diatonicNumber, ushort semitones)
            {
                Name = name;
                LatinName = latinName;
                DiatonicNumber = diatonicNumber;
                Semitones = semitones;
            }
			public override string ToString() => Name;

            public bool Equals(NaturalTone other) => (DiatonicNumber == other.DiatonicNumber);

            public override bool Equals(object obj)
            {
                if (obj == null || !(obj is NaturalTone))
                {
                    return false;
                }
                return Equals((NaturalTone)obj);
            }

            public override int GetHashCode() => Convert.ToInt32((DiatonicNumber ^ Semitones) & 0xFFFFFFFF);

            public static bool operator ==(NaturalTone naturalTone, Object other) { return naturalTone.Equals(other); }
            public static bool operator !=(NaturalTone naturalTone, Object other) { return (!naturalTone.Equals(other)); }

            public static NaturalTone C => new NaturalTone("C", "Do", 1, 0);
            public static NaturalTone D => new NaturalTone("D", "Re", 2, 2);
            public static NaturalTone E => new NaturalTone("E", "Mi", 3, 4);
            public static NaturalTone F => new NaturalTone("F", "Fa", 4, 5);
            public static NaturalTone G => new NaturalTone("G", "Sol", 5, 7);
            public static NaturalTone A => new NaturalTone("A", "La", 6, 9);
            public static NaturalTone B => new NaturalTone("B", "Si", 7, 11);

            public static List<NaturalTone> ALL => new List<NaturalTone> { C, D, E, F, G, A, B };

            public NaturalTone GetNext()
            {
                var nextValue = DiatonicNumber + 1;
                if (nextValue == 12)
                {
                    nextValue = 0;
                }
                return ALL.SingleOrDefault(next => next.DiatonicNumber == nextValue);
            }

            public NaturalTone GetPrevious()
            {
                var previousValue = DiatonicNumber - 1;
                if (previousValue == -1)
                {
                    previousValue = 11;
                }
                return ALL.SingleOrDefault(previous => previous.DiatonicNumber == previousValue);
            }
        }

        private class Accidental : IEquatable<Accidental>
        {
            public string Name { get; private set; }
            public string Notation { get; private set; }

            public string LyNotation { get; private set; }
            public short SemitoneDifference { get; private set; }

            private Accidental(string name, string notation, string lyNotation, short alteration)
            {
                Name = name;
                Notation = notation;
                LyNotation = lyNotation;
                SemitoneDifference = alteration;
            }
			public override string ToString() => Name;

            public bool Equals(Accidental other) => (Notation == other.Notation);

            public override bool Equals(object obj)
            {
                if (obj == null || !(obj is Accidental))
                {
                    return false;
                }
                return Equals((Accidental)obj);
            }

            public override int GetHashCode() => Convert.ToInt32((SemitoneDifference ^ Name.Count()) & 0xFFFFFFFF);

            public static bool operator ==(Accidental accidental, Object other) { return accidental.Equals(other); }
            public static bool operator !=(Accidental accidental, Object other) { return (!accidental.Equals(other)); }


            public static Accidental DOUBLE_FLAT => new Accidental("Double flat", "bb", "eses", -2);
            public static Accidental FLAT => new Accidental("Flat", "b", "es", -1);
            public static Accidental NATURAL => new Accidental("Natural", "", "", 0);
            public static Accidental SHARP => new Accidental("Sharp", "#", "is", 1);
            public static Accidental DOUBLE_SHARP => new Accidental("Double sharp", "*", "isis", 2);

            public static List<Accidental> ALL => new List<Accidental>
            {
                DOUBLE_FLAT,
                FLAT,
                NATURAL,
                SHARP,
                DOUBLE_SHARP
            };
        }

		public static string BaseToneRegex => @"([a-hA-H])";
		public static string AlterationRegex => @"(is{1,2}|e?s(es)?|b{1,2}|#|\*|)";
		public static string OctaveRegex => @"('*)";
		public static string RegexString => "(" + BaseToneRegex + AlterationRegex + OctaveRegex + ")";

		private NaturalTone BaseTone { get; set; }
        private Accidental Alteration { get; set; }
        public ushort Octave { get; private set; }
        public Pitch PitchClass => new Pitch { BaseTone = BaseTone, Alteration = Alteration };

		public bool IsNatural => (Alteration == Accidental.NATURAL);
		public bool IsSharp => (Alteration == Accidental.SHARP || Alteration == Accidental.DOUBLE_SHARP);
		public bool IsFlat => (Alteration == Accidental.FLAT || Alteration == Accidental.DOUBLE_FLAT);

        private Pitch() { }

		public Pitch(uint value)
        {
			Init(value);
        }

		public Pitch(string input)
		{
			var regexResult = Regex.Match(input, RegexString);

			if (!regexResult.Success)
			{
				throw new InvalidCastException($"Could not parse '{input}' to type Pitch");
			}

			var baseTone = NaturalTone.ALL.SingleOrDefault(nt => nt.Name.ToLower() == regexResult.Groups[2].Value.ToLower());
			
			var alteration = Accidental.ALL.SingleOrDefault(
				a => a.LyNotation.ToLower() == regexResult.Groups[3].Value.ToLower() 
				|| a.Notation.ToLower() == regexResult.Groups[3].Value.ToLower());
			
			BaseTone = baseTone ?? throw new InvalidCastException($"Could not parse '{input}' to type Pitch");
			Alteration = alteration ?? throw new InvalidCastException($"Could not parse '{input}' to type Pitch");
			Octave = (ushort)input.Count(f => f == '\'');
		}

        private Pitch(NaturalTone baseTone, ushort value)
        {
			var semitoneDifference = (value % 12) - baseTone.Semitones;
			var nearestOctaveValue = (int)(Math.Round((decimal)semitoneDifference / 12) * 12);
			semitoneDifference = semitoneDifference - nearestOctaveValue;
			Alteration = Accidental.ALL.SingleOrDefault(acc => acc.SemitoneDifference == semitoneDifference);

			if (Alteration == null)
			{
				Init(value);
				return;
			}

			BaseTone = baseTone;
			Octave = (ushort)((value - BaseTone.Semitones + Alteration.SemitoneDifference) / 12);
		}

		private void Init(uint value)
		{
			Octave = (ushort)(value / 12);
			var pitchClassValue = value % 12;

			BaseTone = NaturalTone.ALL.SingleOrDefault(nt => nt.Semitones == pitchClassValue);

			if (BaseTone != null)
			{
				Alteration = Accidental.NATURAL;
			}
			else
			{
				BaseTone = NaturalTone.ALL.SingleOrDefault(nt => nt.Semitones + 1 == pitchClassValue);
				Alteration = Accidental.SHARP;
			}
		}

        public bool Equals(Pitch other) => (BaseTone == other.BaseTone && Alteration == other.Alteration && Octave == other.Octave);
        
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Pitch))
            {
                return false;
            }
            return Equals((Pitch)obj);
        }

        public override int GetHashCode() => Convert.ToInt32((Octave ^ BaseTone.Semitones * Alteration.SemitoneDifference) & 0xFFFFFFFF);
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(BaseTone.Name);
            sb.Append(Alteration.Notation);
            sb.Append(OctaveToString());
            return sb.ToString();
        }

        public string ToLyString()
        {
            var sb = new StringBuilder();
            sb.Append(BaseTone.Name.ToLower());
            sb.Append(Alteration.LyNotation);
            sb.Append(OctaveToString());
            return sb.ToString();
        }

        private string OctaveToString()
        {
            var sb = new StringBuilder();
            sb.Append('\'', Octave);
            return sb.ToString();
        }

        public ushort ToInt()
        {
            var intval = (Octave * 12) + BaseTone.Semitones + Alteration.SemitoneDifference;
            if (intval < 0)
            {
                throw new InvalidCastException("Pitch too low!");
            }
            return (ushort)intval;
        }

        public bool IsEnharmonicTo(Pitch other) => (ToInt() == other.ToInt());

        public void Simplify()
        {
            var simple = new Pitch((ushort)this);
            BaseTone = simple.BaseTone;
            Alteration = simple.Alteration;
            Octave = simple.Octave;
        }

        public Pitch Add(ushort interval) => (ushort)(interval + ToInt());

        public Pitch Add(Interval interval)
        {
            var newBaseTone = NaturalTone.ALL.SingleOrDefault(nt =>
                (nt.DiatonicNumber % 7) == (BaseTone.DiatonicNumber + interval.DiatonicDistance) % 7);
            var newPitchValue = (ushort)((ushort)interval + ToInt());
            return new Pitch(newBaseTone, newPitchValue);
        }

		public Pitch Add(string interval) => Add(new Interval(interval));
		
        public Pitch Subtract(ushort interval)
        {
            var intValue = ToInt();
            if (intValue < interval)
            {
                throw new InvalidOperationException("Interval to subtract is too large!");
            }
            return (ushort)(intValue - interval);
        }

        public Pitch Subtract(Interval interval)
        {
			ushort intValue = ToInt();
			var semitonesToSubtract = (ushort)interval;

			if (intValue < semitonesToSubtract)
			{
				throw new InvalidOperationException("Interval to subtract is too large!");
			}

			var newBaseTone = NaturalTone.ALL.SingleOrDefault(nt =>
				(nt.DiatonicNumber % 7) == ((BaseTone.DiatonicNumber - interval.DiatonicDistance) % 7 + 7) % 7);
			var newPitchValue = (ushort)(intValue - semitonesToSubtract);
			return new Pitch(newBaseTone, newPitchValue);
		}

		public Pitch Subtract(string interval) => Subtract(new Interval(interval));
		
        public Interval Difference(Pitch other)
        {
			var pitchDifference = (ToInt() - other.ToInt());
			var octaveDifference = (ushort)(pitchDifference / 12);
			other.Octave += octaveDifference;
			pitchDifference = (ToInt() - other.ToInt());
			var diatonicDistance = BaseTone.DiatonicNumber - other.BaseTone.DiatonicNumber;

			if (diatonicDistance < 0)
			{
				return other.Difference(this);
			}

			return new Interval(diatonicDistance, pitchDifference);
        }

        public bool IsPureTo(Pitch other) => Difference(other).IsPure;
        
        public bool IsConsonantTo(Pitch other) => Difference(other).IsConsonant;

        public static bool operator ==(Pitch pitch, Object other) { return pitch.Equals(other); }
        public static bool operator !=(Pitch pitch, Object other) { return (!pitch.Equals(other)); }
        public static implicit operator Pitch(ushort value) { return new Pitch(value); }
        public static explicit operator ushort(Pitch value) { return value.ToInt(); }
    }
}