using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Models;
using RestaurantService.Repository;

namespace RestaurantService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantController(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetAllRestaurants()
        {
            var restaurants = await _restaurantRepository.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurantById(int id)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return NotFound(new { message = $"Restaurant with ID {id} not found" });

            return Ok(restaurant);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> SearchRestaurants([FromQuery] string searchTerm)
        {
            var restaurants = await _restaurantRepository.SearchRestaurantsAsync(searchTerm);
            return Ok(restaurants);
        }
        [HttpGet("{restaurantId}/menu")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems(int restaurantId)
        {
            // First validate the restaurant exists
            var restaurantExists = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurantExists == null)
            {
                return NotFound(new { message = $"Restaurant with ID {restaurantId} not found" });
            }

            // Get menu items for the restaurant
            var menuItems = await _restaurantRepository.GetMenuByRestaurantIdAsync(restaurantId);
            return Ok(menuItems);
        }
    }
}
