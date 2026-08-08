using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.Entirty
{
    public class RefreshToken :BaseEntity
    {
        public string Token { get; set; }

        public DateTime ExpiresOn { get; set; }

        public bool IsRevoked { get; set; }

        public string UserId { get; set; }

        public AppUser User { get; set; }
    }
}
