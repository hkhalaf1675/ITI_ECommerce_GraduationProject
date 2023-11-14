using Core.DTOs.Product;
using Core.IRepositories;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using Image = Core.Models.Image;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductRepository productRepository;
        IConfiguration _configuration;
        string baseUrl;

        public ProductsController(IProductRepository _productRepository, IConfiguration configuration)
        {
            productRepository = _productRepository;
            _configuration = configuration;
            baseUrl = _configuration["ApiBaseUrl"];
        }



        [HttpGet("{id:int}", Name = "GetProductByID")] //Get /api/products/GetProductByID/1
        public IActionResult GetById(int id)
        {
            Product product = productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            ProductToReturnDto productToReturnDto = new ProductToReturnDto();

            #region Mapping
            productToReturnDto.Id = product.Id;
            productToReturnDto.Name = product.Name;
            productToReturnDto.Description = product.Description;
            productToReturnDto.Price = product.Price;
            productToReturnDto.Discount = (int)product.Discount;
            productToReturnDto.Condition = (int)product.Condition;
            productToReturnDto.Model = product.Model;
            productToReturnDto.Color = product.Color;
            productToReturnDto.Storage = (int)product.Storage;
            productToReturnDto.CPU = product.CPU;
            productToReturnDto.Ram = (int)product.Ram;
            productToReturnDto.Carmera = product.Carmera;
            productToReturnDto.ScreenSize = (float)product.ScreenSize;
            productToReturnDto.BatteryCapacity = (int)product.BatteryCapacity;
            productToReturnDto.OSVersion = product.OSVersion;
            productToReturnDto.CategoryID = (int)product.CategoryID;
            productToReturnDto.CategoryName = product.Category.Name ;  
            productToReturnDto.BrandID = (int)product.BrandID;
            productToReturnDto.BrandName = product.Brand.Name;
            productToReturnDto.Warranties = product.Warranties.ToDictionary(warranty => warranty.PartName, warranty => warranty.Duration);
            productToReturnDto.Images = product.Images.Select(image => $"{baseUrl}/{image.ImageUrl}").ToList();
            productToReturnDto.AvgRating = product.Reviews?.Any() == true ? (decimal)product.Reviews.Average(r => r.Rating) : 0;

            #endregion


            return Ok(productToReturnDto);
        }



        [HttpGet("All")] //GET /api/products/all
        public IActionResult GetAll([FromQuery] QueryParametars parametars)
        {
            ICollection<Product> products = productRepository.GetAllWithSpec(parametars);

            List<ProductToReturnDto> productsDto = new List<ProductToReturnDto>();
            if (products?.Count > 0)
            {
                foreach (Product product in products)
                {
                    ProductToReturnDto productToReturnDto = new ProductToReturnDto();

                    #region Mapping
                    productToReturnDto.Id = product.Id;
                    productToReturnDto.Name = product.Name;
                    productToReturnDto.Description = product.Description;
                    productToReturnDto.Price = product.Price;
                    productToReturnDto.Discount = (int)product.Discount;
                    productToReturnDto.Condition = (int)product.Condition;
                    productToReturnDto.Model = product.Model;
                    productToReturnDto.Color = product.Color;
                    productToReturnDto.Storage = (int)product.Storage;
                    productToReturnDto.CPU = product.CPU;
                    productToReturnDto.Ram = (int)product.Ram;
                    productToReturnDto.Carmera = product.Carmera;
                    productToReturnDto.ScreenSize = (float)product.ScreenSize;
                    productToReturnDto.BatteryCapacity = (int)product.BatteryCapacity;
                    productToReturnDto.OSVersion = product.OSVersion;
                    productToReturnDto.CategoryID = (int)product.CategoryID;
                    productToReturnDto.CategoryName = product.Category.Name;
                    productToReturnDto.BrandID = (int)product.BrandID;
                    productToReturnDto.BrandName = product.Brand.Name;
                    productToReturnDto.Warranties = product.Warranties.ToDictionary(warranty => warranty.PartName, warranty => warranty.Duration);
                    productToReturnDto.Images = product.Images.Select(image => $"{baseUrl}/{image.ImageUrl}").ToList();
                    productToReturnDto.AvgRating = product.Reviews?.Any() == true ? (decimal)product.Reviews.Average(r => r.Rating) : 0;

                    #endregion

                    productsDto.Add(productToReturnDto);
                }
            }
            return Ok(productsDto);
        }


        [HttpGet("discount")] //GET /api/products/discount
        public IActionResult GetProductsWithDiscount()
        {
            ICollection<Product> products = productRepository.GetAllProductsWithDiscount();
            List<ProductToReturnDto> productsDto = new List<ProductToReturnDto>();
            if (products?.Count > 0)
            {
                foreach (Product product in products)
                {
                    ProductToReturnDto productToReturnDto = new ProductToReturnDto();

                    #region Mapping
                    productToReturnDto.Id = product.Id;
                    productToReturnDto.Name = product.Name;
                    productToReturnDto.Description = product.Description;
                    productToReturnDto.Price = product.Price;
                    productToReturnDto.Discount = (int)product.Discount;
                    productToReturnDto.Condition = (int)product.Condition;
                    productToReturnDto.Model = product.Model;
                    productToReturnDto.Color = product.Color;
                    productToReturnDto.Storage = (int)product.Storage;
                    productToReturnDto.CPU = product.CPU;
                    productToReturnDto.Ram = (int)product.Ram;
                    productToReturnDto.Carmera = product.Carmera;
                    productToReturnDto.ScreenSize = (float)product.ScreenSize;
                    productToReturnDto.BatteryCapacity = (int)product.BatteryCapacity;
                    productToReturnDto.OSVersion = product.OSVersion;
                    productToReturnDto.CategoryID = (int)product.CategoryID;
                    productToReturnDto.CategoryName = product.Category.Name;
                    productToReturnDto.BrandID = (int)product.BrandID;
                    productToReturnDto.BrandName = product.Brand.Name;
                    productToReturnDto.Warranties = product.Warranties.ToDictionary(warranty => warranty.PartName, warranty => warranty.Duration);
                    productToReturnDto.Images = product.Images.Select(image => $"{baseUrl}/{image.ImageUrl}").ToList();
                    productToReturnDto.AvgRating = product.Reviews?.Any() == true ? (decimal)product.Reviews.Average(r => r.Rating) : 0;

                    #endregion

                    productsDto.Add(productToReturnDto);
                }
            }
            return Ok(productsDto);
        }


        [HttpGet("latest")] //GET /api/products/latest
        public IActionResult GetNewProducts()
        {
            ICollection<Product> products = productRepository.GetNewProducts();
            List<ProductToReturnDto> productsDto = new List<ProductToReturnDto>();
            if (products?.Count > 0)
            {
                foreach (Product product in products)
                {
                    ProductToReturnDto productToReturnDto = new ProductToReturnDto();

                    #region Mapping
                    productToReturnDto.Id = product.Id;
                    productToReturnDto.Name = product.Name;
                    productToReturnDto.Description = product.Description;
                    productToReturnDto.Price = product.Price;
                    productToReturnDto.Discount = (int)product.Discount;
                    productToReturnDto.Condition = (int)product.Condition;
                    productToReturnDto.Model = product.Model;
                    productToReturnDto.Color = product.Color;
                    productToReturnDto.Storage = (int)product.Storage;
                    productToReturnDto.CPU = product.CPU;
                    productToReturnDto.Ram = (int)product.Ram;
                    productToReturnDto.Carmera = product.Carmera;
                    productToReturnDto.ScreenSize = (float)product.ScreenSize;
                    productToReturnDto.BatteryCapacity = (int)product.BatteryCapacity;
                    productToReturnDto.OSVersion = product.OSVersion;
                    productToReturnDto.CategoryID = (int)product.CategoryID;
                    productToReturnDto.CategoryName = product.Category.Name;
                    productToReturnDto.BrandID = (int)product.BrandID;
                    productToReturnDto.BrandName = product.Brand.Name;
                    productToReturnDto.Warranties = product.Warranties.ToDictionary(warranty => warranty.PartName, warranty => warranty.Duration);
                    productToReturnDto.Images = product.Images.Select(image => $"{baseUrl}/{image.ImageUrl}").ToList();
                    productToReturnDto.AvgRating = product.Reviews?.Any() == true ? (decimal)product.Reviews.Average(r => r.Rating) : 0;

                    #endregion

                    productsDto.Add(productToReturnDto);
                }
            }
            return Ok(productsDto);
        }


        [HttpGet("{brand:alpha}", Name = "related")] //GET /api/products/related/dell
        public IActionResult GetRelatedProductsByBrand(string brand)
        {
            ICollection<Product> products = productRepository.GetRelatedProductsByBrandName(brand);
            List<ProductToReturnDto> productsDto = new List<ProductToReturnDto>();
            if (products?.Count > 0)
            {
                foreach (Product product in products)
                {
                    ProductToReturnDto productToReturnDto = new ProductToReturnDto();

                    #region Mapping
                    productToReturnDto.Id = product.Id;
                    productToReturnDto.Name = product.Name;
                    productToReturnDto.Description = product.Description;
                    productToReturnDto.Price = product.Price;
                    productToReturnDto.Discount = (int)product.Discount;
                    productToReturnDto.Condition = (int)product.Condition;
                    productToReturnDto.Model = product.Model;
                    productToReturnDto.Color = product.Color;
                    productToReturnDto.Storage = (int)product.Storage;
                    productToReturnDto.CPU = product.CPU;
                    productToReturnDto.Ram = (int)product.Ram;
                    productToReturnDto.Carmera = product.Carmera;
                    productToReturnDto.ScreenSize = (float)product.ScreenSize;
                    productToReturnDto.BatteryCapacity = (int)product.BatteryCapacity;
                    productToReturnDto.OSVersion = product.OSVersion;
                    productToReturnDto.CategoryID = (int)product.CategoryID;
                    productToReturnDto.CategoryName = product.Category.Name;
                    productToReturnDto.BrandID = (int)product.BrandID;
                    productToReturnDto.BrandName = product.Brand.Name;
                    productToReturnDto.Warranties = product.Warranties.ToDictionary(warranty => warranty.PartName, warranty => warranty.Duration);
                    productToReturnDto.Images = product.Images.Select(image => $"{baseUrl}/{image.ImageUrl}").ToList();
                    productToReturnDto.AvgRating = product.Reviews?.Any() == true ? (decimal)product.Reviews.Average(r => r.Rating) : 0;

                    #endregion

                    productsDto.Add(productToReturnDto);
                }
            }
            return Ok(productsDto);
        }


        #region -------------------------- ADMIN ------------------------------

        [HttpPost] //Post /api/Products
        public async Task<IActionResult> PostNew([FromForm]ProductToAddDto productInput)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = productInput.name,
                    Description = productInput.description,
                    Price = productInput.price,
                    Condition = (ProductCondition)productInput.condition,
                    StockQuantity = productInput.stockQuantity,
                    Discount = productInput.discount,
                    Model = productInput.model,
                    Color = productInput.color,
                    Storage = productInput.storage,
                    Ram = productInput.ram,
                    Carmera = productInput.camera,
                    CPU = productInput.cpu,
                    ScreenSize = productInput.screenSize,
                    BatteryCapacity = productInput.batteryCapacity,
                    OSVersion = productInput.osVersion,
                    CategoryID = productInput.categoryID,
                    BrandID = productInput.brandID,

                    // Create related entities
                    Warranties = productInput.warranties?.Select(w => new Warranty
                    {
                        PartName = w.partName,
                        Duration = w.duration
                    }).ToList(),

                    Images = new List<Image>()
                };

                // Handle image uploads
                if (productInput.images != null && productInput.images.Any())
                {
                    foreach (var imageInput in productInput.images)
                    {
                        // Save the image to the "images" folder
                        var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageInput.FileName);
                        var imagePath = Path.Combine("wwwroot","Images", "Products", imageFileName);

                        // Ensure the directory exists
                        var imageDirectory = Path.GetDirectoryName(imagePath);
                        if (!Directory.Exists(imageDirectory))
                        {
                            Directory.CreateDirectory(imageDirectory);
                        }

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await imageInput.CopyToAsync(stream);
                        }

                        // Add the image information to the Images list
                        product.Images.Add(new Image { ImageUrl = imageFileName });

                    }
                }

                bool check = productRepository.AddNew(product);
                if (check)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete] //Delete /api/products
        public IActionResult Delete(int id)
        {
            bool check = productRepository.Delete(id);
            if (check)
            {
                return Ok("deleted Succsesfully");
            }
            return BadRequest();
        }

        //[HttpPut] //Put /api/product
        //public IActionResult Update(int id, [FromBody] ProductToAddDto productInput)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Check if the product with the given id exists
        //        var existingProduct = productRepository.GetById(id);

        //        if (existingProduct == null)
        //        {
        //            return NotFound(); // Product not found
        //        }

        //        // Update the existing product with the new data
        //        existingProduct.Name = productInput.Name;
        //        existingProduct.Description = productInput.Description;
        //        existingProduct.Price = productInput.Price;
        //        existingProduct.Condition = (ProductCondition)productInput.Condition;
        //        existingProduct.StockQuantity = productInput.StockQuantity;
        //        existingProduct.Discount = productInput.Discount;
        //        existingProduct.Model = productInput.Model;
        //        existingProduct.Color = productInput.Color;
        //        existingProduct.Storage = productInput.Storage;
        //        existingProduct.Ram = productInput.Ram;
        //        existingProduct.Carmera = productInput.Camera;
        //        existingProduct.CPU = productInput.CPU;
        //        existingProduct.ScreenSize = productInput.ScreenSize;
        //        existingProduct.BatteryCapacity = productInput.BatteryCapacity;
        //        existingProduct.OSVersion = productInput.OSVersion;
        //        existingProduct.CategoryID = productInput.CategoryID;
        //        existingProduct.BrandID = productInput.BrandID;

        //        // Update related entities (Warranties and Images)
        //        productRepository.UpdateWarranties(existingProduct, productInput.Warranties);
        //        productRepository.UpdateImages(existingProduct, productInput.Images);

        //        bool check = productRepository.Update(existingProduct);

        //        if (check)
        //        {
        //            return Ok();
        //        }
        //        return BadRequest();
        //    }
        //    return BadRequest(ModelState);
        //}



        #endregion

        
    }
}
