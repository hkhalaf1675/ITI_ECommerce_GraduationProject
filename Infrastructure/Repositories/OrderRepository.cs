
using Core.DTOs.ShopingCartDtos;
using Core.DTOs.UserDtos;
using Core.DTOs.UserProfileDtos;
using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;
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
        
        public async Task<int> GetOrdersCount()
        {
            return context.Orders.Count();
        }

        public async Task<bool> AdminDeleteOrder(int orderId)
        {
            Order order = context.Orders.FirstOrDefault(O => O.Id == orderId);
            if (order == null)
            {
                return false;
            }
            context.Orders.Remove(order);

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

        public async Task<IEnumerable<UserOrderDto>> GetAllOrders(int pageNumber)
        {
            List<UserOrderDto> allOrders = new List<UserOrderDto>();

            // get the orders
            var orders = context.Orders.Include(O => O.User)
                .Include(O => O.Address)
                .Include(O => O.OrderDetails)
                .Skip((pageNumber - 1) * 10).Take(10).ToList();

            // loop to map for each order
            foreach(var order in orders)
            {
                List<UserProductsDto> products = new List<UserProductsDto>();

                foreach(var product in order.OrderDetails)
                {
                    // get the order details => order products details
                    var productDetail = context.Products.Include(P => P.Images)
                        .Include(P => P.Brand)
                        .FirstOrDefault(P => P.Id == product.Id);

                    // lsit ts save the url of the images of each product
                    List<string> images = new List<string>();
                    foreach(var image in productDetail.Images)
                    {
                        images.Add(image.ImageUrl);
                    }

                    // map the product to the dto
                    products.Add(new UserProductsDto
                    {
                        Id = product.Id,
                        Name = productDetail.Name,
                        Model = productDetail.Model,
                        Price = productDetail.Price,
                        BrandName = productDetail.Brand.Name,
                        Images = images
                    });
                }

                //map the order the dto
                allOrders.Add(new UserOrderDto
                {
                    OrderId = order.Id,
                    Status = order.Status,
                    Date = order.Date,
                    UserName = order.User.UserName,
                    TotalPrice = order.TotalPrice,
                    Address = order.Address.ToString(),
                    Products = products
                });
            }

            return allOrders;
        }

        public async Task<decimal> TotalSell()
        {
            return context.Orders.Sum(O => O.TotalPrice);
        }

        private Order? GetLastOrder(int userId)
        {
            return context.Orders.OrderByDescending(O => O.UserId == userId)?
                .FirstOrDefault();
        }

    }
}
