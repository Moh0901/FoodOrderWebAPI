using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.DatabaseContext
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }
        public DbSet<Order> Orders { get; set; }
    }

}
