using DeliveryService.DatabaseContext;
using DeliveryService.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Repository
{
    public class DeliveryRepository : IDeliverRepository
    {
        private readonly DeliveryContext _context;

        public DeliveryRepository(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<DeliveryPartner> AssignPartnerToOrder()
        {
            try
            {
                var partner = await _context.DeliveryPartners.FirstOrDefaultAsync(p => p.IsAvailable);
                if (partner != null)
                {
                    partner.IsAvailable = false;
                    _context.DeliveryPartners.Update(partner);
                    await _context.SaveChangesAsync();
                }
                return partner;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning delivery partner: {ex.Message}");
                throw;
            }
        }
    }
}
