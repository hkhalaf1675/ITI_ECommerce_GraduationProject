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
    }
}
