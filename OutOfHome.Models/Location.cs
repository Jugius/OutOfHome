using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OutOfHome.Models
{
    public class Location
    {
        double latitude;
        double longitude;
        public virtual double Latitude
        {
            get { return latitude; }
            set
            {
                if (value < -90 || value > 90)
                    throw new ArgumentOutOfRangeException("Latitude", value, "Value must be between -90 and 90 inclusive.");

                if (double.IsNaN(value))
                    throw new ArgumentException("Latitude must be a valid number.", "Latitude");

                latitude = value;
            }
        }

        public virtual double Longitude
        {
            get { return longitude; }
            set
            {
                if (value < -180 || value > 180)
                    throw new ArgumentOutOfRangeException("Longitude", value, "Value must be between -180 and 180 inclusive.");

                if (double.IsNaN(value))
                    throw new ArgumentException("Longitude must be a valid number.", "Longitude");

                longitude = value;
            }
        }

        protected Location()
            : this(0, 0)
        {
        }
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public static bool TryParse(string address, out Location result)
        {
            result = null;
            
            if (string.IsNullOrWhiteSpace(address))
                return false;

            if (!char.IsDigit(address.First()))
                return false;

            List<string> parts = new List<string>();
            string currentPart = string.Empty;
            foreach (var ch in address)
            {
                if (char.IsDigit(ch))
                    currentPart += ch;
                else
                {
                    if (!string.IsNullOrEmpty(currentPart))
                    {
                        parts.Add(new string(currentPart.ToCharArray()));
                        currentPart = string.Empty;
                    }
                }
            }
            if (!string.IsNullOrEmpty(currentPart))
                parts.Add(new string(currentPart.ToCharArray()));

            if (parts.Count < 4)
                return false;

            NumberFormatInfo formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            const NumberStyles style = NumberStyles.AllowDecimalPoint;

            if (double.TryParse(parts[0] + '.' + parts[1], style, formatter, out double lat) && double.TryParse(parts[2] + '.' + parts[3], style, formatter, out double lon))
            {
                result = new Location(lat, lon);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Location Parse(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentNullException("address");

            if (!char.IsDigit(address.First()))
                throw new FormatException("address");

            List<string> parts = new List<string>();
            string currentPart = string.Empty;
            foreach (var ch in address)
            {
                if (char.IsDigit(ch))
                    currentPart += ch;
                else
                {
                    if (!string.IsNullOrEmpty(currentPart))
                    {
                        parts.Add(new string(currentPart.ToCharArray()));
                        currentPart = string.Empty;
                    }
                }
            }
            if (!string.IsNullOrEmpty(currentPart))
                parts.Add(new string(currentPart.ToCharArray()));
            if (parts.Count < 4)
                throw new Exception($"Unable to parse. Found {parts.Count} parts only.");
            NumberFormatInfo formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            return new Location(Double.Parse($"{parts[0]}.{parts[1]}", formatter), Double.Parse($"{parts[2]}.{parts[3]}", formatter));
           
        }

        protected virtual double ToRadian(double val)
        {
            return (Math.PI / 180.0) * val;
        }
        public virtual Distance DistanceBetween(Location location)
        {
            return DistanceBetween(location, DistanceUnits.Kilometers);
        }

        public virtual Distance DistanceBetween(Location location, DistanceUnits units)
        {
            double earthRadius = (units == DistanceUnits.Miles) ? Distance.EarthRadiusInMiles : Distance.EarthRadiusInKilometers;

            double latRadian = ToRadian(location.Latitude - this.Latitude);
            double longRadian = ToRadian(location.Longitude - this.Longitude);

            double a = Math.Pow(Math.Sin(latRadian / 2.0), 2) +
                Math.Cos(ToRadian(this.Latitude)) *
                Math.Cos(ToRadian(location.Latitude)) *
                Math.Pow(Math.Sin(longRadian / 2.0), 2);

            double c = 2.0 * Math.Asin(Math.Min(1, Math.Sqrt(a)));

            double distance = earthRadius * c;
            return new Distance(distance, units);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        public bool Equals(Location coor)
        {
            if (coor == null)
                return false;

            return (this.Latitude == coor.Latitude && this.Longitude == coor.Longitude);
        }
        public Location Clone() => new Location(this.Latitude, this.Longitude);

        public override int GetHashCode()
        {
            //return Latitude.GetHashCode() ^ Latitude.GetHashCode();
            return (Latitude.ToString() + Longitude.ToString()).GetHashCode();
        }
        public override string ToString()
        {            
            return string.Format(CultureInfo.InvariantCulture, "{0:0.00000000},{1:0.00000000}", latitude, longitude);
        }
        public static bool IsNullOrEmpty(Location location)
        {
            return location == null || (location.Latitude == 0 && location.Longitude == 0);
        }
    }
}
