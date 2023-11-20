using Core.IRepositories;
using Core.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class OrderController : ControllerBase
    {
        // Modification :
        // -> edit BadRequest() to Unauthorized()

        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository _orderRepository)
        {
            orderRepository = _orderRepository;
        }

        [Authorize]
        [HttpPost("addOrder")]
        public async Task<IActionResult> AddNewOrder(int addressId,string payMethod,string phoneNumebr)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                bool check = await orderRepository.AddNewOrder(userId, addressId, payMethod, phoneNumebr);
                if (check)
                    return Ok();

                return BadRequest();
            }
            return Unauthorized();
        }

        
        [HttpGet("GetOrdersCount")]
        public async Task<IActionResult> GetOrdersCount()
        {
            return Ok(await orderRepository.GetOrdersCount());
        }

        [HttpGet("GetAllOrders/{pageNumber:int}")]
        public async Task<IActionResult> GetAllOrders(int pageNumber)
        {
            return Ok(await orderRepository.GetAllOrders(pageNumber));
        }

        [HttpDelete("DeleteOrder/{orderId:int}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            bool check = await orderRepository.AdminDeleteOrder(orderId);

            if (check) 
                return Ok();

            return BadRequest();
        }

        [HttpGet("TotalSell")]
        public async Task<IActionResult> GetTotalSell()
        {
            return Ok(await orderRepository.TotalSell());
        }

        // add the get order by Id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await orderRepository.GetById(id);

            if (order is null)
                return NotFound();

            return Ok(order);
        }
    }
}
