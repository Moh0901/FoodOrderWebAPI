using Microsoft.EntityFrameworkCore;
using RestaurantService.Models;
using System.Collections.Generic;

namespace RestaurantService.DatabaseContext
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Menu)
                .WithOne(m => m.Restaurant)
                .HasForeignKey(m => m.RestaurantId);
        }
    }
}
