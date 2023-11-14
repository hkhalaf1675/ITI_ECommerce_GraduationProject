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
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int condition { get; set; }
        public int stockQuantity { get; set; }
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
        public ICollection<IFormFile>? images { get; set; }
    }
}
