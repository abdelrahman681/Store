using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.CoreLayer.Entirty;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.Config
{
    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            builder.Property(b => b.Name).IsRequired(true);
        }
    }
}
