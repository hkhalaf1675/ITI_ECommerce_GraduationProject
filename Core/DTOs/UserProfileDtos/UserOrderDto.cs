using Core.DTOs.UserDtos;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserProfileDtos
{
    public class UserOrderDto
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public decimal? TotalPrice { get; set; }
        public List<UserProductsDto?> Products { get; set; }
    }
}
