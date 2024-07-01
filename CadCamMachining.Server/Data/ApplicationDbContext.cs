using CadCamMachining.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadCamMachining.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        
        public DbSet<Article> Articles { get; set; }
        
        public DbSet<Part> Parts { get; set; }
        
        public DbSet<Material> Materials { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<Customer> Customers { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>().HasMany(x => x.Contacts);
            builder.Entity<Customer>().HasMany(x => x.Orders);
            
            builder.Entity<Article>().HasOne(x => x.Order);
            builder.Entity<Article>().HasOne(x => x.Part);

            builder.Entity<Order>().HasOne(x => x.Customer);
            builder.Entity<Order>().HasMany(x => x.Articles);
        }
        public DbSet<CadCamMachining.Server.Models.Contact> Contact { get; set; } = default!;
    }
}
