using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Product
{
    public class ProductToAddDto
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        [Range(0, 1)]
        public int Condition { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        [Range(0, 100)]
        public int Discount { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int Storage { get; set; }
        public int Ram { get; set; }
        public string Camera { get; set; }
        public string CPU { get; set; }
        public float ScreenSize { get; set; }
        public int BatteryCapacity { get; set; }
        public string OSVersion { get; set; }
        public int CategoryID { get; set; }
        public int BrandID { get; set; }
        public ICollection<WarrantiesDto> Warranties { get; set; }
        public ICollection<ImagesInputDto> Images { get; set; }
    }
}
