using Core.IRepositories;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }

        [HttpGet("All")]
        public IActionResult GetAll()
        {
            ICollection<Category> categories = categoryRepository.GetAll();
            if(categories?.Count == 0)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        [HttpGet("{id:int}",Name = "GetByID")]
        public IActionResult GetById(int id)
        {
            Category category = categoryRepository.GetById(id);
            if(category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            ICollection<Category> categories = categoryRepository.GetByName(name);
            if(categories?.Count == 0)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        // --------------------------------------------------------
        [HttpPost]
       [Authorize(Roles = "Admin")]
        public IActionResult PostNew(Category category)
        {
            if (ModelState.IsValid)
            {
                bool check = categoryRepository.AddNew(category);
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
            bool check = categoryRepository.Delete(id);
            if (check)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                bool check = categoryRepository.Update(category);
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
