using DeParnasso.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeParnasso.Core.Models
{

    public class Interval : IEquatable<Interval>
    {
        private class IntervalNumber
        {
            public string Name { get; private set; }
            public ushort DiatonicNumber { get; private set; }
            public ushort Semitones { get; private set; }
            public bool IsPure { get; private set; }

            private IntervalNumber(string name, ushort diatonicNumber, ushort semitones, bool isPure)
            {
                Name = name;
                DiatonicNumber = diatonicNumber;
                Semitones = semitones;
                IsPure = isPure;
            }
            public override string ToString() => Name;

            public static IntervalNumber UNISON = new IntervalNumber("Unison", 1, 0, true);
            public static IntervalNumber SECOND = new IntervalNumber("Second", 2, 2, false);
            public static IntervalNumber THIRD = new IntervalNumber("Third", 3, 4, false);
            public static IntervalNumber FOURTH = new IntervalNumber("Fourth", 4, 5, true);
            public static IntervalNumber FIFTH = new IntervalNumber("Fifth", 5, 7, true);
            public static IntervalNumber SIXTH = new IntervalNumber("Sixth", 6, 9, false);
            public static IntervalNumber SEVENTH = new IntervalNumber("Seventh", 7, 11, false);
            public static IntervalNumber OCTAVE = new IntervalNumber("Octave", 8, 12, true);

            public static List<IntervalNumber> ALL = new List<IntervalNumber>
            {
                UNISON,
                SECOND,
                THIRD,
                FOURTH,
                FIFTH,
                SIXTH,
                SEVENTH,
                OCTAVE
            };
        }

        private class IntervalQuality
        {
            public string Name { get; private set; }
            public short Alteration { get; private set; }
            public bool ForPure { get; private set; }
            public bool IsConsonant { get; private set; }

            private IntervalQuality(string name, short alteration, bool forPure, bool isConsonant)
            {
                Name = name;
                Alteration = alteration;
                ForPure = forPure;
                IsConsonant = isConsonant;
            }

            public override string ToString() => Name;

            public static IntervalQuality DIMINISHED_MINOR = new IntervalQuality("diminished", -2, false, false);
            public static IntervalQuality MINOR = new IntervalQuality("minor", -1, false, true);
            public static IntervalQuality MAJOR = new IntervalQuality("Major", 0, false, true);
            public static IntervalQuality AUGMENTED_MAJOR = new IntervalQuality("Augmented", 1, false, false);
            public static IntervalQuality DIMINISHED = new IntervalQuality("diminished", -1, true, false);
            public static IntervalQuality PERFECT = new IntervalQuality("Perfect", 0, true, true);
            public static IntervalQuality AUGMENTED = new IntervalQuality("Augmented", 1, true, false);

            public static List<IntervalQuality> ALL = new List<IntervalQuality>
            {
                DIMINISHED_MINOR,
                MINOR,
                MAJOR,
                AUGMENTED_MAJOR,
                DIMINISHED,
                PERFECT,
                AUGMENTED
            };

            // retrieve the quality based on the interval class and the chosen interval number
            public static IntervalQuality Factory(IntervalNumber intervalNumber, int intervalClass)
            {
                return ALL.SingleOrDefault(
                iq => iq.Alteration == intervalClass - intervalNumber.Semitones
                && iq.ForPure == intervalNumber.IsPure);
            }
        }

        public enum IntervalDirection
        {
            Down = 0,
            Up = 1
        }

        private IntervalNumber Number { get; set; }
        private IntervalQuality Quality { get; set; }
        private IntervalDirection Direction = IntervalDirection.Up;

        public virtual int IntervalClass => Direction == IntervalDirection.Down ? -1 * (Number.Semitones + Quality.Alteration) : Number.Semitones + Quality.Alteration;
        public bool IsPure => (Quality == IntervalQuality.PERFECT);
        public bool IsConsonant => (Quality.IsConsonant);
        public int DiatonicDistance => Direction == IntervalDirection.Down ? -1 * (Number.DiatonicNumber - 1) : Number.DiatonicNumber - 1;

        private Interval() { }

        public Interval(int diatonicDistance, int intervalClass)
        {
            if (intervalClass > 12)
                throw new InvalidOperationException("Interval class value exceeds 12.");

            if (diatonicDistance < 0)
            {
                Direction = IntervalDirection.Down;
                diatonicDistance *= -1;
                intervalClass *= -1;
            }

            Number = IntervalNumber.ALL.SingleOrDefault(inr => inr.DiatonicNumber - 1 == diatonicDistance);

            if (Number == null)
                throw new InvalidOperationException($"No IntervalNumber found for diatonic distance {diatonicDistance}");

            Quality = IntervalQuality.Factory(Number, intervalClass);

            if (Quality == null)
                Init(intervalClass);
        }

        public Interval(int intervalClass)
        {
            Init(intervalClass);
        }

        private void Init(int intervalClass)
        {
            if (intervalClass < 0)
            {
                Direction = IntervalDirection.Down;
                intervalClass = Math.Abs(intervalClass);
            }

            if (intervalClass > 12)
                throw new InvalidOperationException("Interval class value exceeds 12.");

            // first try pure or major intervals
            Number = IntervalNumber.ALL.SingleOrDefault(inr => inr.Semitones == intervalClass);

            // then try minor or diminished (only fifth) intervals
            if (Number == null)
                Number = IntervalNumber.ALL.SingleOrDefault(inr => inr.Semitones - 1 == intervalClass);

            Quality = IntervalQuality.Factory(Number, intervalClass);
        }

        public Interval(string input)
        {
            if (input[0] == '-')
            {
                Direction = IntervalDirection.Down;
                input = input.TrimStart('-');
            }

            var number = IntervalNumber.ALL.SingleOrDefault(inr => inr.DiatonicNumber == input[1].ToInt());

            if (number == null)
                throw new InvalidCastException($"Could not parse '{input}' to type Interval");

            var quality = IntervalQuality.ALL.SingleOrDefault(iq => iq.ForPure == number.IsPure && iq.Name[0] == input[0]);

            Number = number;
            Quality = quality ?? throw new InvalidCastException($"Could not parse '{input}' to type Interval");

            if (Number == IntervalNumber.UNISON && Quality == IntervalQuality.PERFECT)
                Direction = IntervalDirection.Up;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Direction == IntervalDirection.Down)
                sb.Append("-");

            sb.Append(Quality.Name.Substring(0, 1));
            sb.Append(Number.DiatonicNumber);
            return sb.ToString();
        }

        public void Simplify()
        {
            var simple = new Interval(IntervalClass);
            Number = simple.Number;
            Quality = simple.Quality;
        }

        public Interval Revert()
        {
            return new Interval
            {
                Direction = Direction == IntervalDirection.Up ? IntervalDirection.Down : IntervalDirection.Up,
                Number = Number,
                Quality = Quality
            };
        }

        public bool Equals(Interval other)
        {
            return (Direction == other.Direction && Number == other.Number && Quality == other.Quality);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Interval))
                return false;

            return Equals((Interval)obj);
        }

        public override int GetHashCode() => Convert.ToInt32((Number.Semitones ^ Quality.Alteration) & 0xFFFFFFFF);

        public static implicit operator Interval(ushort value) { return new Interval(value); }
        public static explicit operator int(Interval value) { return value.IntervalClass; }

        public static bool operator ==(Interval int1, Interval int2) { return int1.Equals(int2); }
        public static bool operator !=(Interval int1, Interval int2) { return (!int1.Equals(int2)); }

        public static Interval operator -(Interval interval) { return interval.Revert(); }

    }
}
