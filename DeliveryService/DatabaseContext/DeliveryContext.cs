using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.DatabaseContext
{
    public class DeliveryContext : DbContext
    {
        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options) { }

        public DbSet<DeliveryPartner> DeliveryPartners { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
