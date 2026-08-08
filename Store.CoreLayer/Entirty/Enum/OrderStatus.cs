using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Store.CoreLayer.Entirty.Enum
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        pending,
        [EnumMember(Value = "PaymentSuccssed")]
        PaymentSuccssed,
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed,
        [EnumMember(Value = "Cancelled")]
        Cancelled,
        [EnumMember(Value = "Delivered")]
        Delivered,
    }
}
