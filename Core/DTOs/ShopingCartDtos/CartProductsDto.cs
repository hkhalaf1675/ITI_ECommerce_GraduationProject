using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ShopingCartDtos
{
    public class CartProductsDto
    {
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; } = 0;
        public int? Discount { get; set; } = 0;
        public int? ProductQuantity { get; set; } = 0;
        public decimal? TotalPrice => (ProductPrice - ((Discount / 100) * ProductPrice)) * ProductQuantity;

        // newely Added By Abdulrahman 
        public ICollection<string> Images { get; set; }

    }
}
