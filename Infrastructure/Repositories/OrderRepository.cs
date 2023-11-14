
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
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceDBContext context;
        private readonly IShopingCartRepository cartRepository;

        public OrderRepository(ECommerceDBContext _context,IShopingCartRepository _cartRepository)
        {
            context = _context;
            cartRepository = _cartRepository;
        }

        public async Task<bool> AddNewOrder(int userId, int addressId,string payMethod)
        {
            Order newOrder = new Order
            {
                Status = OrderStatus.Pending,
                AddressId = addressId,
                UserId = userId,
                Method = payMethod == "PayPal"?PayMethods.PayPal:PayMethods.Cash,
                Date = DateTime.Now,
                TotalPrice = 0
            };
            
            context.Orders.Add(newOrder);
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            Order? lastOrder = GetLastOrder(userId);
            decimal? orderTotalPrice = 0;

            var cartProducts = await cartRepository.GetUserCartProducts(userId);

            foreach(var product in cartProducts)
            {
                context.OrdersDetails.Add(new OrderDetails
                {
                    OrderID = lastOrder?.Id,
                    ProductID = product.ProductId,
                    Quantity = product.ProductQuantity,
                    TotalPrice = product.TotalPrice
                });

                orderTotalPrice += product.TotalPrice;
            }

            lastOrder.TotalPrice = Convert.ToDecimal(orderTotalPrice);

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

        private Order? GetLastOrder(int userId)
        {
            return context.Orders.OrderByDescending(O => O.UserId == userId)?
                .FirstOrDefault();
        }
    }
}
