using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserDtos
{
    public class UserProductsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Model { get; set; }
        public string? BrandName { get; set; }
        public ICollection<string> Images { get; set; } = new List<string>();
    }
}
