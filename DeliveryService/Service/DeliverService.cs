using DeliveryService.Models;
using DeliveryService.Repository;

namespace DeliveryService.Service
{
    public class DeliverService : IDeliveryService
    {
        private readonly IDeliverRepository _deliveryRepository;

        public DeliverService(IDeliverRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task AssignDeliveryPartner(Order order)
        {
            var partner = await _deliveryRepository.AssignPartnerToOrder();
            if (partner != null)
            {
                Console.WriteLine($" Order {order.Id} assigned to: {partner.Name} ({partner.PhoneNumber})");

                // You can also update the order status in DB (if needed)
            }
            else
            {
                Console.WriteLine("No available delivery partners.");
            }
        }
    }
}
