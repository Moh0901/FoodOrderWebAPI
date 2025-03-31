using DeliveryService.Repository;

namespace DeliveryService.Service
{
    public class DeliverService: IDeliveryService
    {
        private readonly IDeliverRepository _deliveryRepository;

        public DeliverService(IDeliverRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task AssignDeliveryPartner(string orderMessage)
        {
            var partner = await _deliveryRepository.AssignPartnerToOrder();
            if (partner != null)
            {
                Console.WriteLine($"Order assigned to delivery partner: {partner.Name}");
            }
            else
            {
                Console.WriteLine("No available delivery partners.");
            }
        }
    }
}
