using Core.DTOs.Product;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Prodfiles
{
    public static class MapProductsToDto
    {
        public static ICollection<ProductToReturnDto> Map(ICollection<Product> products)
        {
            List<ProductToReturnDto> productToReturnDtos = new List<ProductToReturnDto>();

            foreach (Product product in products)
            {
                productToReturnDtos.Add(new ProductToReturnDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Discount = (int)product.Discount,
                    Condition = (int)product.Condition,
                    Model = product.Model,
                    Color = product.Color,
                    Storage = (int)product.Storage,
                    CPU = product.CPU,
                    Ram = (int)product.Ram,
                    Camera = product.Camera,
                    ScreenSize = (float)product.ScreenSize,
                    BatteryCapacity = (int)product.BatteryCapacity,
                    OSVersion = product.OSVersion,
                    CategoryID = (int)product.CategoryID,
                    CategoryName = product.Category.Name,
                    BrandID = (int)product.BrandID,
                    BrandName = product.Brand.Name,
                    Warranties = product.Warranties.ToDictionary(warranty => warranty.PartName, warranty => warranty.Duration),
                    Images = product.Images.Select(image => $"https://localhost:7003/images/products/{image.ImageUrl}").ToList(),
                    AvgRating = product.Reviews?.Any() == true ? (decimal)product.Reviews.Average(r => r.Rating) : 0
                
                });
            }

            return productToReturnDtos;
        }
    }
}
