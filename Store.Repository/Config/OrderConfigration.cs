using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.CoreLayer.Entirty;
using Store.CoreLayer.Entirty.Enum;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Store.Repository.Config
{
    public class OrderConfigration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(o => o.Items).WithOne();
            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(o => o.PaymentIntentId).IsRequired(false);
            builder.OwnsOne(o => o.Address, o => o.WithOwner());
            builder.Property(o => o.Status).HasConversion(oStatus => oStatus.ToString(), oStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), oStatus));
        }
    }
}
