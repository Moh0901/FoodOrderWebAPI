using RestaurantService.Models;

namespace RestaurantService.Repository
{
    public interface IRestaurantRepository
    {
        Task<List<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant?> GetRestaurantByIdAsync(int id);
        Task<List<Restaurant>> SearchRestaurantsAsync(string searchTerm);
        Task<List<MenuItem>> GetMenuByRestaurantIdAsync(int restaurantId);
    }
}
