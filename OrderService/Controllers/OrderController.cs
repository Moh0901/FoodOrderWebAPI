using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderService.Models;
using OrderService.Producer;
using OrderService.Repository;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly IOrderProducer _orderPublisher;
        public OrderController(IOrderRepository repository, IOrderProducer orderPublisher)
        {
            _repository = repository;
            _orderPublisher = orderPublisher;
        }
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            var createdOrder = await _repository.CreateOrderAsync(order);
            _orderPublisher.SendMessage(JsonConvert.SerializeObject(createdOrder));
            return Ok(createdOrder);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _repository.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _repository.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}
