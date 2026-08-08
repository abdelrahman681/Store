using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.Entirty
{
    public class ShippingAddress
    {
        public ShippingAddress()
        {
        }

        public ShippingAddress(string fName, string lName, string city, string street, string country)
        {
            FName = fName;
            LName = lName;
            City = city;
            Street = street;
            Country = country;
        }

        public string FName { get; set; }
        public string LName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
    }
}
