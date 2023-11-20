using Core.DTOs.UserProfileDtos;
using Core.IRepositories;
using Core.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {

        #region Repository Injection
        private readonly IUserRepository userRepository;
        private readonly UserManager<User> userManager;
        private readonly IWishlistRepository wishlistRepository;
        private readonly IFavouriteRepository favouriteRepository;

        public UserProfileController(IUserRepository _userRepository, UserManager<User> _userManager, IWishlistRepository _wishlistRepository, IFavouriteRepository _favouriteRepository)
        {
            userRepository = _userRepository;
            userManager = _userManager;
            wishlistRepository = _wishlistRepository;
            favouriteRepository = _favouriteRepository;
        } 
        #endregion

        #region User Info , Update ,Delete User , Change Password
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            return Ok(userRepository.GetUserInfo(currentUser));
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser(string password)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }
            bool check = await userRepository.DeleteUser(currentUser, password);
            if (check)
                return Ok();

            return BadRequest();
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto updateDto)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return BadRequest();
            }
            bool check = await userRepository.UpdateUser(currentUser, updateDto);

            if (check)
                return Ok();

            return BadRequest();
        }
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(UserResetPasswordDto resetPasswordDto)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return BadRequest();
            }
            bool check = await userRepository.ChangePassword(currentUser, resetPasswordDto);

            if (check)
                return Ok();

            return BadRequest();
        }
        #endregion

        #region User Addresses , Get , Add , Delete
        [HttpPost("Address")]
        public async Task<IActionResult> AddAddress(UserAddressDto addressDto)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            bool check = await userRepository.AddAddress(currentUser, addressDto);
            if (check)
                return Ok();

            return BadRequest();
        }
        [HttpDelete("Address")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            bool check = await userRepository.DeleteAddress(currentUser, addressId);
            if (check)
                return Ok();

            return BadRequest();
        }
        [HttpGet("Addresses")]
        public async Task<IActionResult> GetAddresses()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var addreses = await userRepository.userAddresses(currentUser);
            if (addreses == null)
            {
                List<UserAddressDto> Address = new List<UserAddressDto>();

                return Ok(Address);
            }
            return Ok(addreses.ToList());
        }
        #endregion

        #region User Orders
        [HttpGet("Orders")]
        public async Task<IActionResult> GetOrders()
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var orders = await userRepository.userOrders(currentUser);


            return Ok(orders?.ToList());

        }
        #endregion



        #region WishList Of User
        // get the wishlist product of the user
        [HttpGet("wishlist")]
        public async Task<IActionResult> GetWishlist()
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var productList =  wishlistRepository.UserProducts(userId);

                return Ok(productList);
            }
            return Unauthorized();
        }

        [HttpPost("wishlist/{productId:int}")]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                bool check = wishlistRepository.AddNew(userId, productId);
                if (check)
                    return Ok();
            }
            return Unauthorized();
        }
        [HttpDelete("wishlist/{productId:int}")]
        public async Task<IActionResult> DeleteFromWishlist(int productId)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                bool check = wishlistRepository.Delete(userId, productId);
                if (check)
                    return Ok();
            }
            return Unauthorized();
        }
        #endregion

        #region Favourite Of User
        // get the wishlist product of the user
        [HttpGet("favourite")]
        public async Task<IActionResult> GetFavourite()
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var productList = favouriteRepository.UserProducts(userId);

                return Ok(productList);
            }
            return Unauthorized();
        }
        [HttpPost("favourite/{productId:int}")]
        public async Task<IActionResult> AddToFavourite(int productId)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                bool check = favouriteRepository.AddNew(userId, productId);
                if (check)
                    return Ok();
            }
            return Unauthorized();
        }
        [HttpDelete("favourite/{productId:int}")]
        public async Task<IActionResult> DeleteFromFavourite(int productId)
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                bool check = favouriteRepository.Delete(userId, productId);
                if (check)
                    return Ok();
            }
            return Unauthorized();
        }
        #endregion

        #region User Phones : Add , Delete , Get User Phones

        [HttpPost("add-phone")]
        public async Task<IActionResult> AddPhone()
        {   
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {



                var result = await userRepository.AddPhone(userId, Request.Form["phone"]);

                return Ok();
            }
            return Unauthorized();
        }

        [HttpDelete("delete-phone")]
        public async Task<IActionResult> DeletePhone(string phone)
        {


            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var result = await userRepository.DeletePhone(userId, phone);

                return Ok();
            }
            return Unauthorized();
        }

        [HttpGet("get-phones")]
        public async Task<IActionResult> GetUserPhones()
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var result = await userRepository.GetUserPhones(userId);

                return Ok(result);
            }
            return Unauthorized();
        }
        #endregion
    }
}
