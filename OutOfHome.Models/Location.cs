using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OutOfHome.Models
{
    public class Location
    {
        private static readonly NumberFormatInfo parsePointFormatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        private const NumberStyles parseStyle = NumberStyles.AllowDecimalPoint;
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
        public static bool TryParse(double latitude, double longitude, out Location result)
        {
            result = null;
            if (latitude < -90 || latitude > 90 || double.IsNaN(latitude)) return false;
            if (longitude < -180 || longitude > 180 || double.IsNaN(longitude)) return false;

            result = new Location(latitude, longitude);
            return true;
        }
        public static bool TryParse(string latitude, string longitude, out Location result)
        {
            result = null;
            if (!TryParseStringToDouble(latitude, out double lat) || !TryParseStringToDouble(longitude, out double lon)) return false;
            return TryParse(lat, lon, out result);
        }
        public static bool TryParse(string address, out Location result)
        {
            result = null;

            if (string.IsNullOrWhiteSpace(address))
                return false;

            if (!char.IsDigit(address.First()))
                return false;

            List<string> parts = new List<string>(4);
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


            if (!TryParseStringToDouble(parts[0] + '.' + parts[1], out double lat) || !TryParseStringToDouble(parts[2] + '.' + parts[3], out double lon)) return false;
            return TryParse(lat, lon, out result);
        }
        public static Location Parse(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentNullException(nameof(address));

            if (!char.IsDigit(address.First()))
                throw new FormatException("address");

            List<string> parts = new List<string>(4);
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

            return new Location(Double.Parse($"{parts[0]}.{parts[1]}", parsePointFormatter), Double.Parse($"{parts[2]}.{parts[3]}", parsePointFormatter));
        }
        protected virtual double ToRadian(double val)
        {            
            return (Math.PI / 180.0) * val;
        }
        public virtual Distance DistanceBetween(Location location)
        {
            return DistanceBetween(location, DistanceUnit.Kilometers);
        }

        public virtual Distance DistanceBetween(Location location, DistanceUnit units)
        {
            if (location == null) throw new ArgumentNullException(nameof(location));

            double earthRadius = (units == DistanceUnit.Miles) ? Distance.EarthRadiusInMiles : Distance.EarthRadiusInKilometers;

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
            return this.ToString().GetHashCode(StringComparison.InvariantCultureIgnoreCase);
        }
        public override string ToString()
        {            
            return string.Format(CultureInfo.InvariantCulture, "{0:0.00000000},{1:0.00000000}", latitude, longitude);
        }
        public static bool IsNullOrEmpty(Location location)
        {
            return location == null || (location.Latitude == 0 && location.Longitude == 0);
        }
        private static bool TryParseStringToDouble(string value, out double result)
        {
            result = 0;
            if (string.IsNullOrEmpty(value)) return false;
            if (double.TryParse(value, parseStyle, parsePointFormatter, out result)) return true;

            List<string> numberParts = new List<string>(2);
            string currentPart = string.Empty;
            foreach (var currChar in value)
            {
                if (char.IsDigit(currChar))
                    currentPart += currChar;
                else
                {
                    if (!string.IsNullOrEmpty(currentPart))
                    {
                        numberParts.Add(new string(currentPart.ToCharArray()));
                        currentPart = string.Empty;
                    }
                }
            }
            if (!string.IsNullOrEmpty(currentPart))
                numberParts.Add(new string(currentPart.ToCharArray()));

            if (numberParts.Count < 2) return false;

            return double.TryParse($"{numberParts[0]}.{numberParts[1]}", parseStyle, parsePointFormatter, out result);
        }
        public static double ConvertDegreeAngleToDouble(string point)
        {
            //Example: 17.21.18S

            var multiplier = (point.Contains('S') || point.Contains('W')) ? -1 : 1; //handle south and west

            point = System.Text.RegularExpressions.Regex.Replace(point, "[^0-9.]", ""); //remove the characters

            var pointArray = point.Split('.'); //split the string.

            //Decimal degrees = 
            //   whole number of degrees, 
            //   plus minutes divided by 60, 
            //   plus seconds divided by 3600

            var degrees = Double.Parse(pointArray[0]);
            var minutes = Double.Parse(pointArray[1]) / 60;
            var seconds = Double.Parse(pointArray[2]) / 3600;

            return (degrees + minutes + seconds) * multiplier;
        }
    }
}
