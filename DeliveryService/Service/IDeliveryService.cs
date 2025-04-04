using DeliveryService.Models;

namespace DeliveryService.Service
{
    public interface IDeliveryService
    {
        public Task AssignDeliveryPartner(Order order);
    }
}
