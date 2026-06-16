using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.CoreLayer.Entirty;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.Brand).WithMany().HasForeignKey(p => p.ProductBrandId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Category).WithMany().HasForeignKey(p => p.ProductCategoryId).OnDelete(DeleteBehavior.SetNull);
            builder.Property(p => p.Name).IsRequired(true);
            builder.Property(p => p.Description).IsRequired(true);
            builder.Property(p => p.ProductBrandId).IsRequired(false);
            builder.Property(p => p.ProductCategoryId).IsRequired(false);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        }
    }
}
