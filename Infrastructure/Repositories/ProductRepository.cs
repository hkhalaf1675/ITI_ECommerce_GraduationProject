using Core.DTOs.Product;
using Core.IRepositories;
using Core.Models;
using Infrastructure.Prodfiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        // Modification :
        // -> add filter by color
        // -> add get top three rating

        ECommerceDBContext context;
        public ProductRepository(ECommerceDBContext _context)
        {
            context = _context;
        }

        public Product GetById(int id)
        {
            return context.Products
                    .Include(p => p.Reviews)
                    .Include(p => p.Images)
                    .Include(p => p.Warranties)
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .FirstOrDefault(p => p.Id == id);
        }

        // to make function async ---->
        public async Task<Product> GetByIdAsync(int id)
        {
            return await context.Products
                .Include(p => p.Reviews)
                    .Include(p => p.Images)
                    .Include(p => p.Warranties)
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .FirstOrDefaultAsync(p => p.Id == id);
        }


        public ICollection<Product> GetAll()
        {
            return context.Products
                .Include(p => p.Reviews)
                .Include(p => p.Images)
                .Include(p => p.Warranties)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .OrderBy(p => p.Name).ToList();
        }


        public ICollection<Product> GetAllWithSpec(QueryParametars parametars)
        {
            var products = GetAll();

            if (parametars.categoryid > 0)
            {
                products = products.Where(p => p.CategoryID == parametars.categoryid).ToList();
            }

            if (parametars.brandId > 0)
            {
                products = products.Where(p => p.BrandID == parametars.brandId).ToList();
            }

            if (parametars.condition?.ToLower() == "used" )
            {
                products = products.Where(p => p.Condition == ProductCondition.Used).ToList();
            }

            if (parametars.condition?.ToLower() == "new")
            {
                products = products.Where(p => p.Condition == ProductCondition.New).ToList();
            }

            if (parametars.Search != null && parametars.Search != "")
            {
                products = products.Where(p => p.Name.Contains(parametars.Search)).ToList();
            }

            if (parametars.MaxPrice > 0)
            {
                products = products.Where(p => p.Price - (p.Price * p.Discount / 100) <= parametars.MaxPrice).ToList();
            }
            if (parametars.MinPrice > 0)
            {
                products = products.Where(p => p.Price - (p.Price * p.Discount / 100) >= parametars.MinPrice).ToList();
            }

            if (parametars.Rating > 0)
            {
                products = products.Where(p => p.Reviews.Any()) // Filter products with at least one review
                                    .Select(p => new
                                    {
                                        Product = p,
                                        AvgRating = p.Reviews.Average(r => r.Rating)
                                    })
                                    .Where(p => p.AvgRating >= parametars.Rating)
                                    .Select(p => p.Product)
                                    .ToList();
            }

            if (parametars.sort == "priceAsc")
            {
                products = products.OrderBy(p => p.Price).ToList();
            }

            if (parametars.sort == "priceDesc")
            {
                products = products.OrderByDescending(p => p.Price).ToList();
            }

            // add filter by color
            if(parametars.Color != null)
            {
                products = products.Where(P => P.Color.ToLower() == parametars.Color.ToLower()).ToList();
            }

            return products.Skip((parametars.PageIndex - 1) * parametars.PageSize).Take(parametars.PageSize).ToList();
        }


        public ICollection<Product> GetAllProductsWithDiscount()
        {
            return context.Products
            .Include(p => p.Reviews)
            .Include(p => p.Images)
             .Include(p => p.Warranties)
                .Include(p => p.Category)
                .Include(p => p.Brand)
            .Where(p => p.Discount > 0)
            .OrderByDescending(p => p.Discount)
            .Take(6).ToList();
        }

        public ICollection<Product> GetRelatedProductsByBrandName(string brand)
        {
            return context.Products
           .Include(p => p.Brand)
           .Include(p => p.Reviews)
           .Include(p => p.Images)
           .Include(p => p.Warranties)
           .Include(p => p.Category)
           .Where(p => p.Brand.Name == brand)
           .Take(6).OrderBy(p => p.Name).ToList();
        }


        public ICollection<Product> GetNewProducts()
        {
            return context.Products
             .Include(p => p.Reviews)
             .Include(p => p.Images)
              .Include(p => p.Warranties)
                .Include(p => p.Category)
                .Include(p => p.Brand)
             .OrderByDescending(p => p.Id)
             .Take(6).OrderBy(p => p.Name).ToList();
        }

        public int GetCount()
        {
            return context.Products.Count();
        }

        // add get top three rating
        public async Task<ICollection<ProductToReturnDto>> GetTopThreeRate()
        {
            ICollection<ProductToReturnDto> productToReturnDtos = new List<ProductToReturnDto>();

            var products = context.Products
                .Include(p => p.Images)
                .Include(p => p.Warranties)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(P => P.Reviews)
                .OrderByDescending(P => P.Reviews.Sum(R => R.Rating)).Take(3).ToList();

            if(products == null || products?.Count == 0)
                return productToReturnDtos;

            productToReturnDtos = MapProductsToDto.Map(products);

            return productToReturnDtos;
        }

        #region ADMIN

        public bool AddNew(Product product)
        {
            try
            {
                context.Products.Add(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void UpdateWarranties(Product product, IEnumerable<WarrantiesDto> warrantyInput)
        {
            // Remove existing warranties not in the input
            var existingWarranties = product.Warranties.ToList();
            foreach (var existingWarranty in existingWarranties)
            {
                var matchingInputWarranty = warrantyInput.FirstOrDefault(w => w.partName == existingWarranty.PartName);
                if (matchingInputWarranty == null)
                {
                    product.Warranties.Remove(existingWarranty);
                }
            }

            // Add or update warranties from the input
            foreach (var warranty in warrantyInput)
            {
                var existingWarranty = product.Warranties.FirstOrDefault(w => w.PartName == warranty.partName);
                if (existingWarranty == null)
                {
                    // Create a new warranty
                    var newWarranty = new Warranty
                    {
                        PartName = warranty.partName,
                        Duration = warranty.duration
                    };
                    product.Warranties.Add(newWarranty);
                }
                else
                {
                    // Update existing warranty
                    existingWarranty.PartName = warranty.partName;
                    existingWarranty.Duration = warranty.duration;
                }
            }
        }

        public void UpdateImages(Product product, IEnumerable<Image> imageInput)
        {
            // Remove existing images not in the input
            var existingImages = product.Images.ToList();
            foreach (var existingImage in existingImages)
            {
                var matchingInputImage = imageInput.FirstOrDefault(i => i.ImageUrl == existingImage.ImageUrl);
                if (matchingInputImage == null)
                {
                    product.Images.Remove(existingImage);
                }
            }

            // Add or update images from the input
            foreach (var image in imageInput)
            {
                var existingImage = product.Images.FirstOrDefault(i => i.ImageUrl == image.ImageUrl);
                if (existingImage == null)
                {
                    // Create a new image
                    var newImage = new Image
                    {
                        ImageUrl = image.ImageUrl
                    };
                    product.Images.Add(newImage);
                }
                else
                {
                    // Update existing image
                    existingImage.ImageUrl = image.ImageUrl;
                }
            }
        }

        public bool Update(Product product)
        {
            try
            {
                context.Products.Update(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                context.Products.Remove(GetById(id));
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // get the count of all products
        public int GetProductsCount()
        {
            return context.Products.Count();
        }


        #endregion
    }
}
