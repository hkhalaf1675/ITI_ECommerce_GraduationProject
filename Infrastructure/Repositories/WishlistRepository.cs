using Core.DTOs.UserDtos;
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
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ECommerceDBContext context;
        IConfiguration _configuration; // tasneem add it
        string baseUrl; //tasneem add it

        public WishlistRepository(ECommerceDBContext _context, IConfiguration configuration)
        {
            context = _context;
            _configuration = configuration; //tasneem add it
            baseUrl = _configuration["ApiBaseUrl"]; //tasneem add it 
        }
        public Wishlist? GetByUserIdAndProductId(int userId, int productId)
        {
            return context.Wishlists.FirstOrDefault(W => W.UserID == userId && W.ProductID == productId);
        }
        public bool CheckExist(int userId, int productId)
        {
            if(GetByUserIdAndProductId(userId,productId) == null)
            {
                return false;
            }
            return true;
        }
        public bool AddNew(int userId, int productId)
        {
            if(!CheckExist(userId,productId))
            {
                context.Wishlists.Add(new Wishlist()
                {
                    UserID = userId,
                    ProductID = productId
                });
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
            return true; ;
        }

        public bool Delete(int userId, int productId)
        {
            // modified remove the '!' from the condition
            if (CheckExist(userId, productId))
            {
                context.Wishlists.Remove(GetByUserIdAndProductId(userId, productId));
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
            return true;
        }

        public ICollection<UserProductsDto> UserProducts(int userId)
        {
            List<UserProductsDto> products = new List<UserProductsDto>();
            List<Wishlist> wishlists = context.Wishlists.Where(W => W.UserID == userId).ToList();
            foreach(Wishlist item in wishlists)
            {
                List<string> images = new List<string>();
                Product? product = context.Products.Include(P => P.Brand).Include(P => P.Images).FirstOrDefault(P => P.Id == item.ProductID);
                foreach(var image in product?.Images)
                {

                    images.Add($"{baseUrl}/{image.ImageUrl}"); // tasneem add it 
                }
                products.Add(new UserProductsDto()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Model = product.Model,
                    BrandName = product?.Brand?.Name,
                    Images = images
                });
            }

            return products;
        }
    }
}
