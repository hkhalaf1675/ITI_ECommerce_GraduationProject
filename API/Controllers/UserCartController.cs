using Core.DTOs.ShopingCartDto;
using Core.DTOs.ShopingCartDtos;
using Core.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserCartController : ControllerBase
    {
        private readonly IShopingCartRepository shopingCartRepository;

        public UserCartController(IShopingCartRepository _shopingCartRepository)
        {
            shopingCartRepository = _shopingCartRepository;
        }

        [HttpGet("CartProducts")]
        public async Task<IActionResult> GetCatProducts()
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier).ToString(), out int userId))
            {
                ICollection<CartProductsDto> cartProducts = await shopingCartRepository.GetUserCartProducts(userId);

                if(cartProducts?.Count() == 0)
                {
                    return NotFound();
                }
                return Ok(cartProducts);
            }
            return BadRequest();
        }

        [HttpPost("AddProductToCart")]
        public async Task<IActionResult> PostProductToCart(ProductToCartDto toCartDto)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier).ToString(), out int userId))
            {
                bool check = await shopingCartRepository.AddProductToCart(userId,toCartDto);

                if (check)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpDelete("DeleteProductFromCart")]
        public async Task<IActionResult> DeleteProductFromcart(int productId)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier).ToString(), out int userId))
            {
                bool check = await shopingCartRepository.RemoveProductFromCart(userId, productId);

                if (check)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPut("EditCartProductQuntity")]
        public async Task<IActionResult> EditCartProductQuntity(int productId,int newQuantity)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier).ToString(), out int userId))
            {
                bool check = await shopingCartRepository.EditProductQuantity(userId, productId,newQuantity);

                if (check)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest();
        }
    }
}
