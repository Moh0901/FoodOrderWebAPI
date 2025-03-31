using DeliveryService.Models;

namespace DeliveryService.Repository
{
    public interface IDeliverRepository
    {
        public Task<DeliveryPartner> AssignPartnerToOrder();
    }
}
