namespace DeliveryService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int MenuItemId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
