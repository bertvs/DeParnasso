using System;
using System.Text;

namespace DeParnasso.Core.Models
{
	// inspired by https://stackoverflow.com/questions/23595820/c-sharp-fractions-calculator
    public sealed class Fraction : IEquatable<Fraction>, IComparable<Fraction>
    {
        public ulong Numerator { get; private set; }
        public ulong Denominator { get; private set; }

        public static Fraction Max(Fraction fraction, Fraction other) => fraction > other ? fraction : other;
        public static Fraction Min(Fraction fraction, Fraction other) => fraction < other ? fraction : other;
        public static Fraction operator +(Fraction frac1, Fraction frac2) { return (Add(frac1, frac2)); }
        public static Fraction operator -(Fraction frac1, Fraction frac2) { return (Subtract(frac1, frac2)); }
        public static Fraction operator *(Fraction frac1, Fraction frac2) { return (Multiply(frac1, frac2)); }
        public static Fraction operator /(Fraction frac1, Fraction frac2) { return (Multiply(frac1, Inverse(frac2))); }
        public static Fraction operator %(Fraction frac1, Fraction frac2) { return (Remainder(frac1, frac2)); }

        public static bool operator ==(Fraction frac1, Object other) { return frac1.Equals(other); }
        public static bool operator !=(Fraction frac1, Object other) { return (!frac1.Equals(other)); }
        public static bool operator <(Fraction frac1, Fraction frac2) { return frac1.Numerator * frac2.Denominator < frac2.Numerator * frac1.Denominator; }
        public static bool operator >(Fraction frac1, Fraction frac2) { return frac1.Numerator * frac2.Denominator > frac2.Numerator * frac1.Denominator; }
        public static bool operator <=(Fraction frac1, Fraction frac2) { return frac1.Numerator * frac2.Denominator <= frac2.Numerator * frac1.Denominator; }
        public static bool operator >=(Fraction frac1, Fraction frac2) { return frac1.Numerator * frac2.Denominator >= frac2.Numerator * frac1.Denominator; }

        public static implicit operator Fraction(ulong value) { return new Fraction(value); }

        public bool Equals(Fraction other) => (Numerator == other.Numerator && Denominator == other.Denominator);
        
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Fraction))
            {
                return false;
            }

            return Equals((Fraction)obj);
        }

        public override int GetHashCode() => Convert.ToInt32((Numerator ^ Denominator) & 0xFFFFFFFF);
        
        public override string ToString()
        {
            if (Denominator == 1)
            {
                return Numerator.ToString();
            }

            var sb = new StringBuilder();
            var wholePart = GetWholePart();

            if (wholePart > 0)
            {
                sb.Append(wholePart).Append(" ");
            }

            var nonWholePart = GetNonWholePart();
            sb.Append(nonWholePart.Numerator).Append("/").Append(nonWholePart.Denominator);
            return sb.ToString();
        }

        public ulong GetWholePart() => (uint)Numerator / Denominator;

        public int CompareTo(Fraction other)
        {
            Reduce();
            other.Reduce();

            if (this == other)
            {
                return 0;
            }

            if (this < other)
            {
                return -1;
            }

            return 1;
        }

        public Fraction GetNonWholePart() => new Fraction(Numerator % Denominator, Denominator);

        public Fraction(ulong value)
        {
            Numerator = value;
            Denominator = 1;
        }

        public Fraction(ulong numerator, ulong denominator)
        {
            if (denominator == 0)
            {
                throw new InvalidOperationException("Denominator cannot be assigned a zero value.");
            }

            Numerator = numerator;
            Denominator = denominator;
            Reduce();
        }

        private static Fraction Add(Fraction fraction, Fraction other)
        {
            checked
            {
                return new Fraction(
                    fraction.Numerator * other.Denominator + other.Numerator * fraction.Denominator,
                    fraction.Denominator * other.Denominator);
            }
        }

        private static ulong GetGcd(ulong num, ulong other)
        {
            if (other == 0)
            {
                return num;
            }
            return GetGcd(other, num % other);
        }

        private static Fraction Inverse(Fraction fraction)
        {
            if (fraction.Numerator == 0)
            {
                throw new InvalidOperationException("Denominator cannot be assigned a zero value.");
            }

            return new Fraction(fraction.Denominator, fraction.Numerator);
        }

        private static Fraction Multiply(Fraction fraction, Fraction other)
        {
            checked
            {
                return new Fraction(
                    fraction.Numerator * other.Numerator,
                    fraction.Denominator * other.Denominator);
            }
        }

        private void Reduce()
        {
            if (Numerator == 0)
            {
                Denominator = 1;
                return;
            }

            var gcd = GetGcd(Numerator, Denominator);
            Numerator /= gcd;
            Denominator /= gcd;
        }
        private static Fraction Remainder(Fraction fraction, Fraction other)
        {
            var division = fraction / other;
            return new Fraction(
                division.Numerator % division.Denominator,
                fraction.Denominator);
        }


        private static Fraction Subtract(Fraction fraction, Fraction other)
        {
            checked
            {
                return new Fraction(
                    fraction.Numerator * other.Denominator - other.Numerator * fraction.Denominator,
                    fraction.Denominator * other.Denominator);
            }
        }
    }
}
