using System;

namespace OutOfHome.Models
{
    public abstract class Address
    {
        string formattedAddress = string.Empty;
        Location location;
        string provider = string.Empty;

        public Address(string formattedAddress, Location location, string provider)
        {
            FormattedAddress = formattedAddress;
            Location = location;
            Provider = provider;
        }
        public virtual string FormattedAddress
        {
            get { return formattedAddress; }
            set {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("FormattedAddress is null or blank");

                formattedAddress = value.Trim();
            }
        }

        public virtual Location Location
        {
            get { return location; }
            set {
                if(value == null)
                    throw new ArgumentNullException("Coordinates");

                location = value;
            }
        }

        public virtual string Provider
        {
            get { return provider; }
            protected set {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Provider can not be null or blank");

                provider = value;
            }
        }

        public virtual Distance DistanceBetween(Address address)
        {
            return this.Location.DistanceBetween(address.Location);
        }

        public virtual Distance DistanceBetween(Address address, DistanceUnit unit)
        {
            return this.Location.DistanceBetween(address.Location, unit);
        }

        public override string ToString()
        {
            return FormattedAddress;
        }
    }
}
