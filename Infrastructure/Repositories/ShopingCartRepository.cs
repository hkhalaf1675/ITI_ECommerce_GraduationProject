using Core.DTOs.ShopingCartDto;
using Core.DTOs.ShopingCartDtos;
using Core.IRepositories;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ShopingCartRepository : IShopingCartRepository
    {
        private readonly ECommerceDBContext context;

        public ShopingCartRepository(ECommerceDBContext _context)
        {
            context = _context;
        }
        public async Task<bool> AddProductToCart(int userId, ProductToCartDto toCartDto)
        {
            int? productStock = context.Products.FirstOrDefault(P => P.Id == toCartDto.ProductId)?.StockQuantity;
            if (productStock == null)
            {
                return false;
            }

            ShopingCart userCart 
                = context.ShoppingCarts.FirstOrDefault(C => C.UserID == userId && C.ProductID == toCartDto.ProductId);
            if ( userCart != null)
            {
                if(userCart.Quantity + toCartDto.Quantity > Convert.ToInt32(productStock))
                {
                    return false;
                }
                userCart.Quantity += toCartDto.Quantity; 
            }
            else
            {
                if(toCartDto.Quantity > Convert.ToInt32(productStock))
                {
                    return false;
                }
                context.ShoppingCarts.Add(new ShopingCart
                {
                    UserID = userId,
                    ProductID = toCartDto.ProductId,
                    Quantity = toCartDto.Quantity,
                });
            }
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ICollection<CartProductsDto>> GetUserCartProducts(int userId)
        {
            List<CartProductsDto> cartProducts = new List<CartProductsDto>();

            var userCarts = context.ShoppingCarts.Where(C => C.UserID == userId);
            if(userCarts?.Count() == 0)
            {
                return cartProducts;
            }

            foreach(var userCart in userCarts)
            {
                Product product = context.Products.FirstOrDefault(P => P.Id == userCart.ProductID);
                cartProducts.Add(new CartProductsDto
                {
                    ProductId = userCart.ProductID,
                    ProductName = product?.Name,
                    ProductPrice = product?.Price,
                    ProductQuantity = userCart.Quantity,
                    Discount = (product?.Discount == null) ? 0 : product.Discount
                });
            }

            return cartProducts;
        }

        public async Task<bool> RemoveProductFromCart(int userId, int productId)
        {
            ShopingCart userCart
                = context.ShoppingCarts.FirstOrDefault(C => C.UserID == userId && C.ProductID == productId);

            if (userCart == null)
            {
                return false;
            }

            try
            {
                context.ShoppingCarts.Remove(userCart);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> EditProductQuantity(int userId, int productId, int newQuntity)
        {
            ShopingCart userCart
                = context.ShoppingCarts.FirstOrDefault(C => C.UserID == userId && C.ProductID == productId);

            if (userCart == null)
            {
                return false;
            }

            int? productStock = context.Products.FirstOrDefault(P => P.Id == productId)?.StockQuantity;
            if (productStock == null)
            {
                return false;
            }

            if(newQuntity > Convert.ToInt32(productStock))
            {
                return false;
            }

            userCart.Quantity = newQuntity;
            try
            {
                context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
