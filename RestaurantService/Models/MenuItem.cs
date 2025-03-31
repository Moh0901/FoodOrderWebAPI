using System.Text.Json.Serialization;

namespace RestaurantService.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public int RestaurantId { get; set; }
        [JsonIgnore]
        public Restaurant? Restaurant { get; set; }
    }
}
