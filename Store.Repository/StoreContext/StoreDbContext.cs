using Microsoft.EntityFrameworkCore;
using Store.CoreLayer.Entirty;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Store.Repository.StoreContext
{
    public class StoreDbContext:DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

    }
}
