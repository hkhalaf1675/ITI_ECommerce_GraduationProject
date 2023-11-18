using Core.DTOs.ShopingCartDto;
using Core.DTOs.ShopingCartDtos;
using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Repositories
{
    public class ShopingCartRepository : IShopingCartRepository
    {
        #region Injection
        private readonly ECommerceDBContext context;

        // Added Newly
        IConfiguration _configuration;
        string baseUrl;

        public ShopingCartRepository(ECommerceDBContext _context, IConfiguration configuration)
        {
            context = _context;

            // Added Newly
            _configuration = configuration;
            baseUrl = _configuration["ApiBaseUrl"];
        } 
        #endregion

        public async Task<bool> AddProductToCart(int userId, ProductToCartDto toCartDto)
        {
            int? productStock = context.Products.FirstOrDefault(P => P.Id == toCartDto.ProductId)?.StockQuantity;
            if (productStock == null)
            {
                return false;
            }

            ShopingCart userCart = context.ShoppingCarts.FirstOrDefault(C => C.UserID == userId && C.ProductID == toCartDto.ProductId);

            if ( userCart != null)
            {
                if (userCart.Quantity + toCartDto.Quantity > Convert.ToInt32(productStock))
                {
                    return false;
                }
                userCart.Quantity += toCartDto.Quantity;
            }
            else
            {
                if (toCartDto.Quantity > Convert.ToInt32(productStock))
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

            // modified was returning single value 
            List<ShopingCart> shoppingcart = GetUserCart(userId); // modified to get the list from method

            List<CartProductsDto> cartProducts = new List<CartProductsDto>();

            if (shoppingcart == null || shoppingcart.Count == 0)
            {
                return cartProducts;
            }

            foreach (var item in shoppingcart)
            {

                Product product = context.Products.Include(p=> p.Images).FirstOrDefault(P => P.Id == item.ProductID);
                cartProducts.Add(new CartProductsDto
                {
                    ProductId = item.ProductID,
                    ProductName = item.Product.Name,
                    ProductPrice = item.Product.Price,
                    ProductQuantity = item.Quantity,
                    Discount = (item.Product.Discount == null) ? 0 : item.Product.Discount,
                    Images = item.Product.Images.Select(image => $"{baseUrl}/{image.ImageUrl}").ToList()
                });

            }

            return cartProducts;
        }

        public async Task<bool> RemoveProductFromCart(int userId, int productId)
        {
            ShopingCart userCart = context.ShoppingCarts.FirstOrDefault(C => C.UserID == userId && C.ProductID == productId);

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

        public async Task<bool> DeleteUserCartProducts(int userId)
        {
            var products = GetUserCart(userId);

            if(products == null || products.Count == 0)
            {
                return true;
            }

            context.ShoppingCarts.RemoveRange(products);

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

        // get list of products of user cart
        private List<ShopingCart> GetUserCart(int userId)
        {
            return context.ShoppingCarts.Where(C => C.UserID == userId).ToList();
        }
    }
}
