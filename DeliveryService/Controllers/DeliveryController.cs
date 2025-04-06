using DeliveryService.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliverRepository _repository;
        public DeliveryController(IDeliverRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("assign")]
        public IActionResult AssignDelivery()
        {

            var partner = _repository.AssignPartnerToOrder();
            if (partner == null) return NotFound("No available delivery partners.");
            return Ok(partner);
        }
    }
}
