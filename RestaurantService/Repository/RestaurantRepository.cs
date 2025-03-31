using Microsoft.EntityFrameworkCore;
using RestaurantService.DatabaseContext;
using RestaurantService.Models;

namespace RestaurantService.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantContext _context;

        public RestaurantRepository(RestaurantContext context)
        {
            _context = context;
        }
        public async Task<List<Restaurant>> GetAllRestaurantsAsync()
        {
            var restaurants = await _context.Restaurants.Include(x => x.Menu).ToListAsync();
            return restaurants;
        }
        public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _context.Restaurants.Include(x => x.Menu).FirstOrDefaultAsync(y => y.Id == id);
            return restaurant;
        }
        public async Task<List<Restaurant>> SearchRestaurantsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetAllRestaurantsAsync();
            var restaurantList = await _context.Restaurants.Include(r => r.Menu).Where(r => r.Name.Contains(searchTerm) || r.Description.Contains(searchTerm)).ToListAsync();
            return restaurantList;
        }
        public async Task<List<MenuItem>> GetMenuByRestaurantIdAsync(int restaurantId)
        {
            var menu = await _context.MenuItems.Where(m => m.RestaurantId == restaurantId).ToListAsync();
            return menu;
        }
    }
}
