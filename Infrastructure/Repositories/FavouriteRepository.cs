using Core.DTOs.UserDtos;
using Core.Models;
using Microsoft.EntityFrameworkCore;
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

        public FavouriteRepository(ECommerceDBContext _context)
        {
            context = _context;
        }
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
            if (!CheckExist(userId, productId))
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

                    images.Add(image?.ImageUrl);
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
