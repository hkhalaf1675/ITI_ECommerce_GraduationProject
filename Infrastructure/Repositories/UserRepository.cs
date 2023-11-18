    using Core.DTOs.UserDtos;
using Core.DTOs.UserProfileDtos;
using Core.IRepositories;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> userManager;
        private readonly ECommerceDBContext context;

        public UserRepository(UserManager<User> _userManager,ECommerceDBContext _context)
        {
            userManager = _userManager;
            context = _context;
        }
        public async Task<ProfileDto> GetUserInfo(User user)
        {
            ProfileDto userProfile = new ProfileDto();

            if (user is null)
            {
                return userProfile;
            }

            userProfile.FullName = user.FullName;
            userProfile.Email = user.Email;

            return userProfile;
        }

        public async Task<bool> UpdateUser(User user, UserUpdateDto userUpdateDto)
        {

            if(user is null)
            {
                return false;
            }

            user.FullName = userUpdateDto.FullName;
            user.Email = userUpdateDto.Email;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteUser(User user,string password)
        {

            if (user is null)
                return true;

            var found = await userManager.CheckPasswordAsync(user, password);
            if (!found)
            {
                return false;
            }

            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
                return true;

            return false;
        }
        public async Task<bool> ChangePassword(User user, UserResetPasswordDto resetPassword)
        {

            if (user is null)
                return false;

            var found = await userManager.CheckPasswordAsync(user, resetPassword.OldPassword);

            if (!found)
                return false;

            var result = await userManager.ChangePasswordAsync(user,resetPassword.OldPassword,resetPassword.NewPassword);

            if (result.Succeeded)
                return true;

            return false;
        }

        public async Task<ICollection<UserAddressDto>> userAddresses(User user)
        {
            List<UserAddressDto> addressDtos = new List<UserAddressDto>();

            if (user is null)
                return addressDtos;

            var addresses = context.Address.Where(A => A.UserID == user.Id).ToList();

            foreach(var address in addresses)
            {
                addressDtos.Add(new UserAddressDto
                {
                    Id = address.Id,
                    City = address.City,
                    Street = address.Street,
                    State = address.State,
                    Country = address.Country,
                    PostalCode = address.PostalCode,
                    SpecialInstructions = address.SpecialInstructions
                });
            }

            return addressDtos;
        }

        public async Task<bool> AddAddress(User user, UserAddressDto address)
        {
            if(user is null)
                return false;

            user.Addresses.Add(new Address
            {
                Street = address.Street,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode,
                SpecialInstructions = address.SpecialInstructions
            });

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
                return true;

            return false;
        }

        public async Task<bool> DeleteAddress(User user, int addressId)
        {
            if(user is null) 
                return false;

            var address = user.Addresses.FirstOrDefault(A => A.Id == addressId);
            if (address is not null)
            {
                user.Addresses.Remove(address);

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return true;

                return false;
            }
            return true;
                
        }

        public async Task<ICollection<UserOrderDto>> userOrders(User user)
        {
            List<UserOrderDto> orderDtos = new List<UserOrderDto>();

            if(user == null)
                return orderDtos;

            var orders = user.Orders.ToList();
            foreach(var order in orders)
            {
                orderDtos.Add(new UserOrderDto
                {
                    OrderId = order.Id,
                    Status = order.Status,
                    Date = order.Date,
                });
            }

            return orderDtos;
        }

    }
}
