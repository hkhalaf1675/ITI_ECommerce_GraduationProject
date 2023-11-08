using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IWishlistRepository:IWishlistFavouriteRepository
    {
        Wishlist? GetByUserIdAndProductId(int userId, int productId);
    }
}
