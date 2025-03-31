namespace DeliveryService.Service
{
    public interface IDeliveryService
    {
        Task AssignDeliveryPartner(string orderMessage);
    }
}
