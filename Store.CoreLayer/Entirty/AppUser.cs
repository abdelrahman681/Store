using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.Entirty
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string? ResetOtp { get; set; }
        public DateTime? OtpExpiry { get; set; }
        public bool IsOtpVerified { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Address> Addresses { get; set; }= new HashSet<Address>();
        public string?  RefreshTokens { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}
