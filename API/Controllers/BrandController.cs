using Core.IRepositories;
using Core.Models;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly ECommerceDBContext con;

        //Brand Controller

        public BrandController(ECommerceDBContext con)
        {
            this.con = con;
        }

        [HttpGet("All")]
        public IActionResult GetAll()
        {
            ICollection<Brand> brands = con.Brands.ToList();
            if (brands?.Count == 0)
            {
                return NotFound();
            }
            return Ok(brands);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                Brand brd = con.Brands.Find(id);
                con.Brands.Remove(brd);
                con.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}
