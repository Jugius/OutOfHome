using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfHome.Models
{
    public enum DistanceUnit
    {
        Miles,
        Kilometers
    }
    public readonly struct Distance : IEquatable<Distance>
    {
        public const double EarthRadiusInMiles = 3956.545;
        public const double EarthRadiusInKilometers = 6378.135;
        private const double ConversionConstant = 0.621371192;

        public double Value { get; }

        public DistanceUnit Units { get; }

        public Distance(double value, DistanceUnit units)
        {
            this.Value = Math.Round(value, 8);
            this.Units = units;
        }

        #region Helper Factory Methods

        public static Distance FromMiles(double miles)
        {
            return new Distance(miles, DistanceUnit.Miles);
        }

        public static Distance FromKilometers(double kilometers)
        {
            return new Distance(kilometers, DistanceUnit.Kilometers);
        }

        #endregion

        #region Unit Conversions

        private Distance ConvertUnits(DistanceUnit units)
        {
            if (this.Units == units) return this;

            double newValue;
            switch (units)
            {
                case DistanceUnit.Miles:
                    newValue = Value * ConversionConstant;
                    break;
                case DistanceUnit.Kilometers:
                    newValue = Value / ConversionConstant;
                    break;
                default:
                    newValue = 0;
                    break;
            }

            return new Distance(newValue, units);
        }

        public Distance ToMiles()
        {
            return ConvertUnits(DistanceUnit.Miles);
        }

        public Distance ToKilometers()
        {
            return ConvertUnits(DistanceUnit.Kilometers);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Distance other)
        {
            return base.Equals(other);
        }

        public bool Equals(Distance other, bool normalizeUnits)
        {
            if (normalizeUnits)
                other = other.ConvertUnits(Units);
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} {1}", this.Value, Units);
        }

        #region Operators

        public static Distance operator *(Distance d1, double d)
        {
            double newValue = d1.Value * d;
            return new Distance(newValue, d1.Units);
        }

        public static Distance operator +(Distance left, Distance right)
        {
            double newValue = left.Value + right.ConvertUnits(left.Units).Value;
            return new Distance(newValue, left.Units);
        }

        public static Distance operator -(Distance left, Distance right)
        {
            double newValue = left.Value - right.ConvertUnits(left.Units).Value;
            return new Distance(newValue, left.Units);
        }

        public static bool operator ==(Distance left, Distance right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Distance left, Distance right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Distance left, Distance right)
        {
            return (left.Value < right.ConvertUnits(left.Units).Value);
        }

        public static bool operator <=(Distance left, Distance right)
        {
            return (left.Value <= right.ConvertUnits(left.Units).Value);
        }

        public static bool operator >(Distance left, Distance right)
        {
            return (left.Value > right.ConvertUnits(left.Units).Value);
        }

        public static bool operator >=(Distance left, Distance right)
        {
            return (left.Value >= right.ConvertUnits(left.Units).Value);
        }

        public static implicit operator double(Distance distance)
        {
            return distance.Value;
        }
        #endregion
    }
}
