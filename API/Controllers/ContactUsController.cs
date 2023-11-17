using Core.DTOs.ContactUs;
using Core.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsRepository contactUsRepository;

        public ContactUsController(IContactUsRepository _contactUsRepository)
        {
            contactUsRepository = _contactUsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(ContactUsDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            bool check = await contactUsRepository.AddMessage(dto);

            if(check)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMessages(int pageNumber)
        {
            var messages = await contactUsRepository.GetAll(pageNumber);

            if (messages?.Count() > 0)
                return Ok(messages);

            return NotFound();
        }
    }
}
