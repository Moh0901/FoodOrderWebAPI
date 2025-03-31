using Microsoft.EntityFrameworkCore;
using OrderService.DatabaseContext;
using OrderService.Models;

namespace OrderService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;
        public OrderRepository(OrderContext context)
        {
            _context = context;
        }
        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
