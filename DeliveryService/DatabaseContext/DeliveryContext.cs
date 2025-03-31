using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.DatabaseContext
{
    public class DeliveryContext : DbContext
    {
        public DbSet<DeliveryPartner> DeliveryPartners { get; set; }
        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options) { }
    }
}
