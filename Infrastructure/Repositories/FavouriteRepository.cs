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
    public class FavouriteRepository: IFavouriteRepository
    {
        private readonly ECommerceDBContext context;
        IConfiguration _configuration; // tasneem add it
        string baseUrl; //tasneem add it

        public FavouriteRepository(ECommerceDBContext _context, IConfiguration configuration)
        {
            context = _context;
            _configuration = configuration; //tasneem add it
            baseUrl = _configuration["ApiBaseUrl"]; //tasneem add it 
        }

        // checks if the product id in the user's favorite list
        public Favourite? GetByUserIdAndProductId(int userId, int productId)
        {
            return context.Favourite.FirstOrDefault(F => F.UserID == userId && F.ProductID == productId);
        }
        public bool CheckExist(int userId, int productId)
        {
            if (GetByUserIdAndProductId(userId, productId) == null)
            {
                return false;
            }
            return true;
        }
        public bool AddNew(int userId, int productId)
        {
            if (!CheckExist(userId, productId))
            {
                context.Favourite.Add(new Favourite()
                {
                    UserID = userId,
                    ProductID = productId
                });
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
            return true;
        }

        public bool Delete(int userId, int productId)
        {
            // modified remove the '!' from the condition 
            if (CheckExist(userId, productId))
            {
                context.Favourite.Remove(GetByUserIdAndProductId(userId, productId));
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
            return true;
        }

        public ICollection<UserProductsDto> UserProducts(int userId)
        {
            List<UserProductsDto> products = new List<UserProductsDto>();
            List<Favourite> favourites = context.Favourite.Where(F => F.UserID == userId).ToList();
            foreach (Favourite item in favourites)
            {
                List<string> images = new List<string>();
                Product? product = context.Products.Include(P => P.Brand).Include(P => P.Images).FirstOrDefault(P => P.Id == item.ProductID);
                foreach (var image in product?.Images)
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
