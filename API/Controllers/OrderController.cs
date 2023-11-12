using Core.IRepositories;
using Core.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository _orderRepository)
        {
            orderRepository = _orderRepository;
        }

        [HttpPost("addOrder")]
        public async Task<IActionResult> AddNewOrder(int addressId)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                bool check = await orderRepository.AddNewOrder(userId, addressId);
                if (check)
                    return Ok();
            }
            return BadRequest();
        }
    }
}
