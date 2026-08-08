using Store.CoreLayer.Entirty.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.Entirty
{
    public class Notification : BaseEntity
    {
        public string UserId { get; set; }

        public AppUser User { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
