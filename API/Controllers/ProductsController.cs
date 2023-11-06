using API.Dto;
using Core.IRepositories;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductRepository productRepository;
        public ProductsController(IProductRepository _productRepository)
        {
            productRepository = _productRepository;
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
            productToReturnDto.Discount = product.Discount;
            productToReturnDto.Condition = (int)product.Condition;
            productToReturnDto.Model = product.Model;
            productToReturnDto.Color = product.Color;
            productToReturnDto.Storage = product.Storage;
            productToReturnDto.CPU = product.CPU;
            productToReturnDto.Ram = product.Ram;
            productToReturnDto.Carmera = product.Carmera;
            productToReturnDto.ScreenSize = product.ScreenSize;
            productToReturnDto.BatteryCapacity = product.BatteryCapacity;
            productToReturnDto.OSVersion = product.OSVersion;
            productToReturnDto.CategoryID = product.CategoryID;
            //productToReturnDto.CategoryName = product.Category.Name ;  null ref
            productToReturnDto.BrandID = product.BrandID;
            //productToReturnDto.BrandName = product.Brand.Name;
            //productToReturnDto.Warranties = product.Warranties;
            //productToReturnDto.Images = product.Images;
            //productToReturnDto.AvgRating = product.Reviews; 
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
                    productToReturnDto.Discount = product.Discount;
                    productToReturnDto.Condition = (int)product.Condition;
                    productToReturnDto.Model = product.Model;
                    productToReturnDto.Color = product.Color;
                    productToReturnDto.Storage = product.Storage;
                    productToReturnDto.CPU = product.CPU;
                    productToReturnDto.Ram = product.Ram;
                    productToReturnDto.Carmera = product.Carmera;
                    productToReturnDto.ScreenSize = product.ScreenSize;
                    productToReturnDto.BatteryCapacity = product.BatteryCapacity;
                    productToReturnDto.OSVersion = product.OSVersion;
                    productToReturnDto.CategoryID = product.CategoryID;
                    //productToReturnDto.CategoryName = product.Category.Name ;  null ref
                    productToReturnDto.BrandID = product.BrandID;
                    //productToReturnDto.BrandName = product.Brand.Name;
                    //productToReturnDto.Warranties = product.Warranties;
                    //productToReturnDto.Images = product.Images;
                    //productToReturnDto.AvgRating = product.Reviews; 
                    #endregion

                    productsDto.Add(productToReturnDto);
                }
            }
            return Ok(productsDto);
        }


        [HttpGet("discount")] //GET /api/products/discount
        public IActionResult GetProductsWithDiscount ()
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
                    productToReturnDto.Discount = product.Discount;
                    productToReturnDto.Condition = (int)product.Condition;
                    productToReturnDto.Model = product.Model;
                    productToReturnDto.Color = product.Color;
                    productToReturnDto.Storage = product.Storage;
                    productToReturnDto.CPU = product.CPU;
                    productToReturnDto.Ram = product.Ram;
                    productToReturnDto.Carmera = product.Carmera;
                    productToReturnDto.ScreenSize = product.ScreenSize;
                    productToReturnDto.BatteryCapacity = product.BatteryCapacity;
                    productToReturnDto.OSVersion = product.OSVersion;
                    productToReturnDto.CategoryID = product.CategoryID;
                    //productToReturnDto.CategoryName = product.Category.Name ;  null ref
                    productToReturnDto.BrandID = product.BrandID;
                    //productToReturnDto.BrandName = product.Brand.Name;
                    //productToReturnDto.Warranties = product.Warranties;
                    //productToReturnDto.Images = product.Images;
                    //productToReturnDto.AvgRating = product.Reviews; 
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
                    productToReturnDto.Discount = product.Discount;
                    productToReturnDto.Condition = (int)product.Condition;
                    productToReturnDto.Model = product.Model;
                    productToReturnDto.Color = product.Color;
                    productToReturnDto.Storage = product.Storage;
                    productToReturnDto.CPU = product.CPU;
                    productToReturnDto.Ram = product.Ram;
                    productToReturnDto.Carmera = product.Carmera;
                    productToReturnDto.ScreenSize = product.ScreenSize;
                    productToReturnDto.BatteryCapacity = product.BatteryCapacity;
                    productToReturnDto.OSVersion = product.OSVersion;
                    productToReturnDto.CategoryID = product.CategoryID;
                    //productToReturnDto.CategoryName = product.Category.Name ;  null ref
                    productToReturnDto.BrandID = product.BrandID;
                    //productToReturnDto.BrandName = product.Brand.Name;
                    //productToReturnDto.Warranties = product.Warranties;
                    //productToReturnDto.Images = product.Images;
                    //productToReturnDto.AvgRating = product.Reviews; 
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
                    productToReturnDto.Discount = product.Discount;
                    productToReturnDto.Condition = (int)product.Condition;
                    productToReturnDto.Model = product.Model;
                    productToReturnDto.Color = product.Color;
                    productToReturnDto.Storage = product.Storage;
                    productToReturnDto.CPU = product.CPU;
                    productToReturnDto.Ram = product.Ram;
                    productToReturnDto.Carmera = product.Carmera;
                    productToReturnDto.ScreenSize = product.ScreenSize;
                    productToReturnDto.BatteryCapacity = product.BatteryCapacity;
                    productToReturnDto.OSVersion = product.OSVersion;
                    productToReturnDto.CategoryID = product.CategoryID;
                    //productToReturnDto.CategoryName = product.Category.Name ;  null ref
                    productToReturnDto.BrandID = product.BrandID;
                    //productToReturnDto.BrandName = product.Brand.Name;
                    //productToReturnDto.Warranties = product.Warranties;
                    //productToReturnDto.Images = product.Images;
                    //productToReturnDto.AvgRating = product.Reviews; 
                    #endregion

                    productsDto.Add(productToReturnDto);
                }
            }
            return Ok(productsDto);
        }


        #region -------------------------- ADMIN ------------------------------

        [HttpPost] //Post /api/products
        public IActionResult PostNew(Product Product)
        {
            if (ModelState.IsValid)
            {
                bool check = productRepository.AddNew(Product);
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
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut] //Put /api/product
        public IActionResult Update(Product Product)
        {
            if (ModelState.IsValid)
            {
                bool check = productRepository.Update(Product);
                if (check)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        } 
        #endregion
    }
}
