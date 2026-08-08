using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.CoreLayer.Entirty;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Store.Repository.Config
{
    public class WishListConfig : IEntityTypeConfiguration<WishList>
    {
        public void Configure(EntityTypeBuilder<WishList> builder)
        {
            builder
                .HasKey(x => new
                {
                    x.CustomerId,
                    x.ProductId
                });
            builder.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId);
            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        }
    }
}
