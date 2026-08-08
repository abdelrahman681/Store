using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.CoreLayer.Entirty;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.Config
{
    public class NotificationConfig : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasOne(n => n.User)
                   .WithMany()
                   .HasForeignKey(n => n.UserId);
        }
    }
}
