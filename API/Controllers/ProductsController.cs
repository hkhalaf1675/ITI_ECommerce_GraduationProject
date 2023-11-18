using Core.DTOs.Product;
using Core.IRepositories;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using Image = Core.Models.Image;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Intrinsics.Arm;
using System.Collections.Generic;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Modification :
        // -> add end api to get top three rating

        #region ctor
        IProductRepository productRepository;
        IConfiguration _configuration;
        string baseUrl;

        public ProductsController(IProductRepository _productRepository, IConfiguration configuration)
        {
            productRepository = _productRepository;
            _configuration = configuration;
            baseUrl = _configuration["ApiBaseUrl"];
        }


        #endregion

        #region GetById
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
            productToReturnDto.stockQuantity = product.StockQuantity;
            productToReturnDto.Model = product.Model;
            productToReturnDto.Color = product.Color;
            productToReturnDto.Storage = (int)product.Storage;
            productToReturnDto.CPU = product.CPU;
            productToReturnDto.Ram = (int)product.Ram;
            productToReturnDto.Camera = product.Camera;
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


            return Ok(productToReturnDto);
        }

        #endregion

        #region GetAll

        //[Authorize(Roles = "Admin")]
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
                    productToReturnDto.Camera = product.Camera;
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

        #endregion

        #region GetProductsWithDiscount

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
                    productToReturnDto.Camera = product.Camera;
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

        #endregion

        #region GetNewProducts

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
                    productToReturnDto.Camera = product.Camera;
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

        #endregion

        #region GetRelatedProductsByBrand

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
                    productToReturnDto.Camera = product.Camera;
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

        #endregion

        #region Get Top Three products with top rating
        [HttpGet("GetTopThreerating")]
        public async Task<IActionResult> GetTopThreeRating()
        {
            var products = await productRepository.GetTopThreeRate();

            return Ok(products);
        }
        #endregion


        #region -------------------------- ADMIN ------------------------------

        #region Add

        [HttpPost] //Post /api/Products
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostNew()
        {
            try
            {
                if (ModelState.IsValid)
                {

                    #region Declare product to add
                    var product = new Product
                    {

                        Name = Request.Form["name"],
                        Description = Request.Form["description"],
                        Price = decimal.Parse(Request.Form["price"]),
                        Condition = (ProductCondition)Enum.Parse(typeof(ProductCondition), Request.Form["condition"]),
                        StockQuantity = int.Parse(Request.Form["stockQuantity"]),
                        Discount = int.Parse(Request.Form["discount"]),
                        Model = Request.Form["model"],
                        Color = Request.Form["color"],
                        Storage = int.Parse(Request.Form["storage"]),
                        Ram = int.Parse(Request.Form["ram"]),
                        Camera = Request.Form["camera"],
                        CPU = Request.Form["cpu"],
                        ScreenSize = int.Parse(Request.Form["screenSize"]),
                        BatteryCapacity = int.Parse(Request.Form["batteryCapacity"]),
                        OSVersion = Request.Form["osVersion"],
                        CategoryID = int.Parse(Request.Form["categoryID"]),
                        BrandID = int.Parse(Request.Form["brandID"]),

                        Warranties = new List<Warranty>(),
                        Images = new List<Image>()
                    };
                    #endregion

                    #region ger warranty by Request.Form["warranties"]
                    // make it in a shape of Json Array 
                    var jsonString = "[" + Request.Form["warranties"].ToString() + "]";

                    // Deserialize it using Newtonsoft.json library $
                    var warranties = JsonConvert.DeserializeObject<List<WarrantiesDto>>(jsonString);

                    // That's it all! No magic here
                    #endregion

                    foreach (var item in warranties)
                    {
                        Warranty warranty = new Warranty()
                        {
                            PartName = item.partName,
                            Duration = item.duration
                        };

                        product.Warranties.Add(warranty);
                    }


                    #region Handle image uploads
                    if (Request.Form.Files.Count > 0)
                    {
                        foreach (var formFile in Request.Form.Files)
                        {
                            var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                            var imagePath = Path.Combine("wwwroot", "Images", "Products", imageFileName);

                            // Ensure the directory exists
                            var imageDirectory = Path.GetDirectoryName(imagePath);
                            if (!Directory.Exists(imageDirectory))
                            {
                                Directory.CreateDirectory(imageDirectory);
                            }

                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }

                            // Add the image information to the Images list
                            product.Images.Add(new Image { ImageUrl = imageFileName });

                        }
                    }
                    #endregion

                    bool check = productRepository.AddNew(product);
                    if (check)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        #endregion


        #region Delete
        [Authorize(Roles = "Admin")]
        [HttpDelete] //Delete /api/products
        public IActionResult Delete(int id)
        {
            bool check = productRepository.Delete(id);
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }
        #endregion


        #region Edit
        [HttpPut] //Put /api/product
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if the product with the given id exists
                    var existingProduct = productRepository.GetById(int.Parse(Request.Form["id"]));

                    if (existingProduct == null)
                    {
                        return NotFound(); // Product not found
                    }

                    #region  Update the existing product with the new data
                    existingProduct.Name = Request.Form["name"];
                    existingProduct.Description = Request.Form["description"];
                    existingProduct.Price = decimal.Parse(Request.Form["price"]);
                    existingProduct.Condition = (ProductCondition)Enum.Parse(typeof(ProductCondition), Request.Form["condition"]);
                    existingProduct.StockQuantity = int.Parse(Request.Form["stockQuantity"]);
                    existingProduct.Discount = int.Parse(Request.Form["discount"]);
                    existingProduct.Model = Request.Form["model"];
                    existingProduct.Color = Request.Form["color"];
                    existingProduct.Storage = int.Parse(Request.Form["storage"]);
                    existingProduct.Ram = int.Parse(Request.Form["ram"]);
                    existingProduct.Camera = Request.Form["camera"];
                    existingProduct.CPU = Request.Form["cpu"];
                    existingProduct.ScreenSize = int.Parse(Request.Form["screenSize"]);
                    existingProduct.BatteryCapacity = int.Parse(Request.Form["batteryCapacity"]);
                    existingProduct.OSVersion = Request.Form["osVersion"];
                    existingProduct.CategoryID = int.Parse(Request.Form["categoryID"]);
                    existingProduct.BrandID = int.Parse(Request.Form["brandID"]);
                    #endregion

                    #region Get Warranties
                    // make it in a shape of Json Array 
                    var jsonString = "[" + Request.Form["warranties"].ToString() + "]";

                    // Deserialize it using Newtonsoft.json library $
                    var warranties = JsonConvert.DeserializeObject<List<WarrantiesDto>>(jsonString);

                    // That's it all! No magic here
                    #endregion

                    #region Handle image uploads
                    var Images = new List<Image>();

                    if (Request.Form.Files.Count > 0)
                    {
                        foreach (var formFile in Request.Form.Files)
                        {
                            var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                            var imagePath = Path.Combine("wwwroot", "Images", "Products", imageFileName);

                            // Ensure the directory exists
                            var imageDirectory = Path.GetDirectoryName(imagePath);
                            if (!Directory.Exists(imageDirectory))
                            {
                                Directory.CreateDirectory(imageDirectory);
                            }

                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }

                            // Add the image information to the Images list
                            Images.Add(new Image { ImageUrl = imageFileName });
                        }
                    }
                    #endregion


                    // Update related entities (Warranties and Images)
                    productRepository.UpdateWarranties(existingProduct, warranties);
                    productRepository.UpdateImages(existingProduct, Images);

                    bool check = productRepository.Update(existingProduct);

                    if (check)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion


        //[Authorize(Roles = "Admin")]
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
        //        existingProduct.Camera = productInput.Camera;
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


        // get the count of all products


        #region Count
        [Authorize(Roles = "Admin")]
        [HttpGet("GetProductsCount")]
        public async Task<IActionResult> GetProductsCount()
        {
            return Ok(productRepository.GetProductsCount());
        } 
        #endregion


    }


    

}
#endregion






