using Core.DTOs.UserDtos;
using Core.DTOs.UserProfileDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IWishlistFavouriteRepository
    {
        bool CheckExist(int userId, int productId);
        bool AddNew(int userId, int productId);
        bool Delete(int userId, int productId);
        ICollection<UserProductsDto> UserProducts(int userId);
    }
}
