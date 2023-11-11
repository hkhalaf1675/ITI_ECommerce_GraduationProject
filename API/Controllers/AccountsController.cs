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
using Core.IServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        #region Injection
        private readonly UserManager<User> userManager;
        private readonly IAccountManagerServices accountManager;
        private readonly IConfiguration configuration;
        private readonly IWishlistRepository wishlistRepository;
        private readonly IFavouriteRepository favouriteRepository;

        public AccountsController(UserManager<User> _userManager, IAccountManagerServices _accountManager, IConfiguration _configuration, IWishlistRepository _wishlistRepository, IFavouriteRepository _favouriteRepository)
        {
            this.userManager = _userManager;
            accountManager = _accountManager;
            this.configuration = _configuration;
            wishlistRepository = _wishlistRepository;
            favouriteRepository = _favouriteRepository;
        }
        #endregion

        #region Register New Version
        //[HttpPost("register")]
        //public async Task<IActionResult> Register(UserRegisterDTO registerDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var result = await accountManager.RegisterAsync(registerDto);

        //    if (!result.IsAuthenticated)
        //        return BadRequest(result.Message);

        //    return Ok(result);
        //}
        #endregion

        #region login New version
        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginCredentialsDTO loginDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var result = await accountManager.LoginAsync(loginDto);

        //    if (!result.IsAuthenticated)
        //        return BadRequest(result.Message);

        //    return Ok(result);
        //}
        #endregion



        #region Register 
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
        #endregion

        #region Login
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginCredentialsDTO loginInput)
        {

            #region User name vertification
            var user = await userManager.FindByNameAsync(loginInput.username);

            if (user == null)
            {
                return BadRequest(new { message = "user not found" });
            }
            #endregion

            #region User lockout check
            var Islocked = await userManager.IsLockedOutAsync(user);
            if (Islocked)
            {
                return BadRequest(new { message = "You  are Locked Out" });
            }
            #endregion

            #region User Password Vertification
            if (!await userManager.CheckPasswordAsync(user, loginInput.password))
            {
                await userManager.AccessFailedAsync(user);
                return Unauthorized();
            }
            #endregion

            #region Get user Claims and Add more custom claims to the list
            var userclaims = await userManager.GetClaimsAsync(user);
            userclaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            userclaims.Add(new Claim(ClaimTypes.GivenName, user.FullName));
            userclaims.Add(new Claim(ClaimTypes.Email, user.Email));
            userclaims.Add(new Claim(ClaimTypes.StreetAddress, user.Address));
            userclaims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
            userclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            #endregion

            #region Get the role of the user and add it to the userclaims
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                // add to the userclaims list 
                userclaims.Add(new Claim(ClaimTypes.Role, role));
            }
            #endregion

            #region Getting the secret key from Appsettings file
            var KeyString = configuration.GetValue<string>("SecretKey");
            var KeyInBytes = Encoding.ASCII.GetBytes(KeyString);
            var Key = new SymmetricSecurityKey(KeyInBytes);
            #endregion

            #region Token Signing
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
            #endregion

        }
        #endregion

        // hashed?
        #region Get Current User
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
        #endregion

        #region Current User Wishlist

        #region Get All wishlist
        [HttpGet("wishlist")]
        public async Task<IActionResult> GetWishlist()
        {
            if (int.TryParse(User.Claims.FirstOrDefault(C => C.Type == ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                var productList = wishlistRepository.UserProducts(userId);
                if (productList?.Count == 0)
                    return NotFound();
                return Ok(productList);
            }
            return BadRequest();
        }
        #endregion

        #region Add new Wishlist
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
        #endregion

        #region Delete Wishlist
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

        #endregion

        #region Current User Favorites

        #region Get All Favorites
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
        #endregion

        #region Add Favorite
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
        #endregion

        #region Delete Favorite 

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

        #endregion


    }

}