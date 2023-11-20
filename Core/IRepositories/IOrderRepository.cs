using Core.DTOs.UserProfileDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IOrderRepository
    {
        Task<UserOrderDto?> GetById(int id);
        Task<bool> AddNewOrder(int userId, int addressId, string payMethod, string phoneNumebr);
        Task<int> GetOrdersCount();
        Task<IEnumerable<UserOrderDto>> GetAllOrders(int pageNumber);
        Task<bool> AdminDeleteOrder(int orderId);
        Task<decimal> TotalSell();
    }
}
