using Core.DTOs.UserProfileDtos;
using Core.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUserManagerController : ControllerBase
    {
        private readonly IAdminUserManager adminUserManager;

        public AdminUserManagerController(IAdminUserManager _adminUserManager)
        {
            adminUserManager = _adminUserManager;
        }

        [HttpGet("GetUsersCount")]
        public async Task<IActionResult> GetUsersCount()
        {
            return Ok(await adminUserManager.GetUsersCount());
        }

        [HttpGet("GetAllUsers/{pageNumber:int}")]
        public async Task<IActionResult> GetAllUsers(int pageNumber)
        {
            return Ok(await adminUserManager.GetAllUsers(pageNumber));
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddNewUser(AdminUserInfo userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool check = await adminUserManager.AdminAddUser(userInfo);
            if (!check)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            bool check = await adminUserManager.AdminDeleteUser(userName);
            if (!check)
                return BadRequest();

            return Ok();
        }
    }
}
