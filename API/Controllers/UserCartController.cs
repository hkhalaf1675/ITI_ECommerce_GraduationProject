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
        #region Injection
        private readonly IShopingCartRepository shopingCartRepository;

        public UserCartController(IShopingCartRepository _shopingCartRepository)
        {
            shopingCartRepository = _shopingCartRepository;
        } 
        #endregion

        #region Get All Cart Products
        [HttpGet("CartProducts")]
        public async Task<IActionResult> GetCatProducts()
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                ICollection<CartProductsDto> cartProducts = await shopingCartRepository.GetUserCartProducts(userId);

                return Ok(cartProducts);
            }
            return Unauthorized();
        } 
        #endregion

        #region Add Product To Cart 
        [HttpPost("AddProductToCart")]
        public async Task<IActionResult> PostProductToCart(ProductToCartDto toCartDto)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                bool check = await shopingCartRepository.AddProductToCart(userId, toCartDto);

                if (check)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        #endregion

        #region Delete Prodcut From Cart 
        [HttpDelete("DeleteProductFromCart")]
        public async Task<IActionResult> DeleteProductFromcart(int productId)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                bool check = await shopingCartRepository.RemoveProductFromCart(userId, productId);

                if (check)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        #endregion

        #region Update Product Amount in Cart
        [HttpPut("EditCartProductQuntity")]
        public async Task<IActionResult> EditCartProductQuntity(int productId, int newQuantity)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                bool check = await shopingCartRepository.EditProductQuantity(userId, productId, newQuantity);

                if (check)
                {
                    return Ok();
                }
                else
                {
                    ProductToCartDto product = new ProductToCartDto()
                    {
                        ProductId = productId,
                        Quantity = newQuantity
                    };

                    bool checkAlternate = await shopingCartRepository.AddProductToCart(userId, product);

                    if (checkAlternate)
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                };
            }
            
            return Unauthorized();
        } 
        #endregion
    }
}
