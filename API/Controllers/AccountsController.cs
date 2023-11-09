using Core.Models;
using ECommerceGP.Bl.Dtos.UserDtos;
using ECommerceGP.Bl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Repositories;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly IWishlistRepository wishlistRepository;
        private readonly IFavouriteRepository favouriteRepository;

        public AccountsController(UserManager<User> _userManager, IConfiguration _configuration, IWishlistRepository _wishlistRepository, IFavouriteRepository _favouriteRepository)
        {
            this.userManager = _userManager;
            this.configuration = _configuration;
            wishlistRepository = _wishlistRepository;
            favouriteRepository = _favouriteRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> register(UserRegisterDTO input)
        {
            User NewUser = new User
            {
                FullName = input.FullName,
                UserName = input.UserName,
                Email = input.Email,
                Address = input.Address,
                PhoneNumber = input.PhoneNumber
            };

            var creationResult = await userManager.CreateAsync(NewUser, input.password);

            if (!creationResult.Succeeded)
            {
                return BadRequest(creationResult.Errors);

            }
            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier,NewUser.Id.ToString()),
               new Claim(ClaimTypes.Role,"Client")
            };

            var claimsResult = await userManager.AddClaimsAsync(NewUser, claims);
            if (!claimsResult.Succeeded)
            {
                return BadRequest(claimsResult.Errors);
            }
            return Ok();
        }

        [HttpPost]
        [Route("Login")]

        public async Task<ActionResult<TokenDTO>> Login(LoginCredentialsDTO loginInput)
        {
            var user = await userManager.FindByNameAsync(loginInput.username);

            if (user == null)
            {
                return BadRequest(new { message = "user not found" });

            }

            var Islocked = await userManager.IsLockedOutAsync(user);
            if (Islocked)
            {
                return BadRequest(new { message = "You  are Locked Out" });
            }

            if (!await userManager.CheckPasswordAsync(user, loginInput.password))
            {
                await userManager.AccessFailedAsync(user);
                return Unauthorized();
            }


            // get the claims of the user where the input value (user) contain all the values of the logged in user
            var userclaims = await userManager.GetClaimsAsync(user);

            // add Custom claims from the user data 
            userclaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            userclaims.Add(new Claim(ClaimTypes.GivenName, user.FullName));
            userclaims.Add(new Claim(ClaimTypes.Email, user.Email));
            userclaims.Add(new Claim(ClaimTypes.StreetAddress, user.Address));
            userclaims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            userclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userclaims.Remove(userclaims[0]);
            // getting the role of the user 
            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                // add to the userclaims list 
                userclaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var KeyString = configuration.GetValue<string>("SecretKey");
            var KeyInBytes = Encoding.ASCII.GetBytes(KeyString);
            var Key = new SymmetricSecurityKey(KeyInBytes);

            var signingCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);


            var jwt = new JwtSecurityToken(
                claims: userclaims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddMinutes(30),
                notBefore: DateTime.Now

                );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(jwt);
            return Ok(new TokenDTO
            {
                Token = tokenString
            });
        }


        //[HttpGet]
        //[Authorize]
        //[Route("CurrentUser")]
        //public async Task<ActionResult> GetCurrentUser()
        //{
        //    var CurrentUser = await userManager.GetUserAsync(User);
        //    return Ok(
        //        new
        //        {
        //            Id = CurrentUser.Id,
        //            UserName = CurrentUser.UserName
        //        }
        //        );
        //}


        #region WishList Of User
        // get the wishlist product of the user
        [HttpGet("wishlist")]
        public async Task<IActionResult> GetWishlist()
        {
            if(int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value,out int userId))
            {
                var productList = wishlistRepository.UserProducts(userId);
                if (productList?.Count == 0)
                    return NotFound();
                return Ok(productList);
            }
            return BadRequest();
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
            return BadRequest();
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
            return BadRequest();
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
                if (productList?.Count == 0)
                    return NotFound();
                return Ok(productList);
            }
            return BadRequest();
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
            return BadRequest();
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
            return BadRequest();
        }
        #endregion
    }
}
