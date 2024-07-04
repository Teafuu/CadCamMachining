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
            new DataGenerator(this);
        }
        
        public DbSet<Article> Articles { get; set; }
        
        public DbSet<Part> Parts { get; set; }
        
        public DbSet<Material> Materials { get; set; }
        
        public DbSet<Order> Orders { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<ArticleStatus> ArticleStatuses { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }

        public DbSet<Contact> Contacts { get; set; }

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

            builder.Entity<Order>().HasOne(x => x.Customer);
            builder.Entity<Order>().HasMany(x => x.Articles);
            builder.Entity<Order>().HasOne(x => x.Status);
            builder.Entity<Order>().HasOne(x => x.InCharge).WithMany(x => x.Orders).IsRequired(false);

            builder.Entity<Order>()
                .HasOne(o => o.InCharge)
                .WithMany(u => u.Orders)
                .IsRequired(false);  // This makes the foreign key optional

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.InCharge)
                .IsRequired(false); // This makes the foreign key optional
            }
        }
}
