
using Core.DTOs.ShopingCartDtos;
using Core.DTOs.UserDtos;
using Core.DTOs.UserProfileDtos;
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
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceDBContext context;
        private readonly IShopingCartRepository cartRepository;
        IConfiguration _configuration; // tasneem add it
        string baseUrl; //tasneem add it

        public OrderRepository(ECommerceDBContext _context,IShopingCartRepository _cartRepository, IConfiguration configuration)
        {
            context = _context;
            cartRepository = _cartRepository;
            _configuration = configuration; //tasneem add it
            baseUrl = _configuration["ApiBaseUrl"]; //tasneem add it 
        }

        public async Task<UserOrderDto?> GetById(int id)
        {
            var order = context.Orders.Include(O => O.User)
                .Include(O => O.Address)
                .Include(O => O.OrderDetails)
                .FirstOrDefault(O => O.Id == id);

            if (order is null)
                return null;

            // get order detail and map the products to dtos
            var products = GetOrderDetails(order);

            // map the order to dto
            UserOrderDto orderDto = MapOrderToDto(order, products);

            return orderDto;
        }

        public async Task<bool> AddNewOrder(int userId, int addressId,string payMethod, string phoneNumebr)
        {
            Order newOrder = new Order
            {
                Status = OrderStatus.Pending,
                AddressId = addressId,
                UserId = userId,
                Method = payMethod == "PayPal"?PayMethods.PayPal:PayMethods.Cash,
                Date = DateTime.Now,
                PhoneNumber = phoneNumebr,
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

            // delete the products from user cart
            var check = await cartRepository.DeleteUserCartProducts(userId);
            if (!check)
            {
                return false;
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
        
        //public async Task<int> GetOrdersCount()
        //{
        //    return context.Orders.Count();
        //}

        //public async Task<bool> AdminDeleteOrder(int orderId)
        //{
        //    Order order = context.Orders.FirstOrDefault(O => O.Id == orderId);
        //    if (order == null)
        //    {
        //        return false;
        //    }
        //    context.Orders.Remove(order);

        //    try
        //    {
        //        context.SaveChanges();
        //        return true;
        //    }
        //    catch(Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<IEnumerable<UserOrderDto>> GetAllOrders(int pageNumber)
        //{
        //    List<UserOrderDto> allOrders = new List<UserOrderDto>();

        //    var orders = context.Orders.Skip((pageNumber - 1) * 10).Take(10);

        //    foreach(var order in orders)
        //    {
        //        allOrders.Add(new UserOrderDto
        //        {
        //            OrderId = order.Id,
        //            Status = order.Status,
        //            Date = order.Date,
        //            UserId = order.UserId
        //        });
        //    }
        //}
        
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
                var products = GetOrderDetails(order);

                //map the order the dto
                allOrders.Add(MapOrderToDto(order,products));
            }

            return allOrders;
        }

        public async Task<decimal> TotalSell()
        {
            return context.Orders.Sum(O => O.TotalPrice);
        }

        private Order? GetLastOrder(int userId)
        {
            return context.Orders.Where(o=> o.UserId == userId).OrderByDescending(O => O.Id)?
                .FirstOrDefault();

        }


        // method to get order detials and map the products into dto
        private List<UserProductsDto> GetOrderDetails(Order order)
        {
            List<UserProductsDto> products = new List<UserProductsDto>();

            foreach (var product in order?.OrderDetails)
            {
                // get the order details => order products details
                var productDetail = context.Products.Include(P => P.Images)
                    .Include(P => P.Brand)
                    .FirstOrDefault(P => P.Id == product.ProductID); //tasneem add it

                // lsit ts save the url of the images of each product
                List<string> images = new List<string>();
                foreach (var image in productDetail.Images)
                {
                    images.Add($"{baseUrl}/{image.ImageUrl}"); // tasneem add it 
                }

                // map the product to the dto
                products.Add(new UserProductsDto
                {
                    Id = (int)product.ProductID, //tasneem add it
                    Name = productDetail.Name,
                    Model = productDetail.Model,
                    Price = productDetail.Price,
                    BrandName = productDetail.Brand.Name,
                    Images = images
                });
            }

            return products;
        }

        // method to map order to orderdto
        private UserOrderDto MapOrderToDto(Order order,List<UserProductsDto> products)
        {
            return new UserOrderDto
            {
                OrderId = order.Id,
                Status = order.Status,
                Date = order.Date,
                UserName = order.User.UserName,
                TotalPrice = order.TotalPrice,
                Address = order.Address.ToString(),
                Products = products
            };
        }
    }
}
