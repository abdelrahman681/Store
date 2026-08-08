using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.Entirty
{
    public class Address :BaseEntity
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
