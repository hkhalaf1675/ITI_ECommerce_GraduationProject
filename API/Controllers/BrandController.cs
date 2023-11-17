using Core.IRepositories;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository brandRepository;

        public BrandController(IBrandRepository _brandRepository)
        {
            brandRepository = _brandRepository;
        }

        [HttpGet("All")]
        public IActionResult GetAll()
        {
            ICollection<Brand> brands = brandRepository.GetAll();
            if (brands?.Count == 0)
            {
                return NotFound();
            }
            return Ok(brands);
        }

        [HttpGet("{id:int}", Name = "BrandGetByID")]
        public IActionResult GetById(int id)
        {
            Brand brand = brandRepository.GetById(id);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand);
        }

        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            ICollection<Brand> brands = brandRepository.GetByName(name);
            if (brands?.Count == 0)
            {
                return NotFound();
            }
            return Ok(brands);
        }

        // --------------------------------------------------------
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult PostNew(Brand brand)
        {
            if (ModelState.IsValid)
            {
                bool check = brandRepository.AddNew(brand);
                if (check)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            bool check = brandRepository.Delete(id);
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Brand brand)
        {
            if (ModelState.IsValid)
            {
                bool check = brandRepository.Update(brand);
                if (check)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        }
    }
}
