using System;

namespace OutOfHome.Models
{
    public struct DateTimePeriod : IEquatable<DateTimePeriod>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTimePeriod(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }

        public bool Equals(DateTimePeriod other)
        {
            return this.Start.Equals(other.Start) && this.End.Equals(other.End);
        }
        public override bool Equals(object obj)
        {
            return obj is DateTimePeriod other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Start.GetHashCode() ^ this.End.GetHashCode();
        }

        public static bool operator ==(DateTimePeriod left, DateTimePeriod right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DateTimePeriod left, DateTimePeriod right)
        {
            return !(left == right);
        }
    }
}
