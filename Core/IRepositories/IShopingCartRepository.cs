using Core.DTOs.ShopingCartDto;
using Core.DTOs.ShopingCartDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IShopingCartRepository
    {
        Task<bool> AddProductToCart(int userId, ProductToCartDto toCartDto);
        Task<bool> RemoveProductFromCart(int userId, int productId);
        Task<bool> EditProductQuantity(int userId, int productId, int newQuntity);
        Task<ICollection<CartProductsDto>> GetUserCartProducts(int userId);
        Task<bool> DeleteUserCartProducts(int userId);

    }
}
