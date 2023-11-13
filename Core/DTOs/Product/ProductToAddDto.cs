using Core.Models;
using Microsoft.AspNetCore.Http;
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
        public string name { get; set; }
        public string description { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public decimal price { get; set; }
        [Required]
        [Range(0, 1)]
        public int condition { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int stockQuantity { get; set; }
        [Range(0, 100)]
        public int discount { get; set; }
        public string model { get; set; }
        public string color { get; set; }
        public int storage { get; set; }
        public int ram { get; set; }
        public string camera { get; set; }
        public string cpu { get; set; }
        public float screenSize { get; set; }
        public int batteryCapacity { get; set; }
        public string osVersion { get; set; }
        public int categoryID { get; set; }
        public int brandID { get; set; }
        public ICollection<WarrantiesDto> warranties { get; set; }
        public ICollection<IFormFile> images { get; set; }
    }
}
