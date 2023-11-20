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
            user.UserName = userUpdateDto.UserName;
            user.PhoneNumber = userUpdateDto.PhoneNumber;
            user.Address = userUpdateDto.Address;

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

        #region User Phones

        public async Task<string?> AddPhone(int userId, string? phoneNumber)
        {
            var phone = 
                context.Phones.FirstOrDefault(Ph => Ph.UserID == userId && Ph.PhoneNumber == phoneNumber);

            if(phone != null)
            {
                return "That phone is already exists";
            }

            context.Phones.Add(new Phone
            {
                UserID = userId,
                PhoneNumber = phoneNumber
            });

            try
            {
                context.SaveChanges();

                return "The Phone was added successfully";
            }
            catch(Exception ex)
            {
                return $"There is an error : {ex.Message}";
            }

        }

        public async Task<string?> DeletePhone(int userId, string? phoneNumber)
        {
            var phone =
                context.Phones.FirstOrDefault(Ph => Ph.UserID == userId && Ph.PhoneNumber == phoneNumber);

            if (phone == null)
            {
                return "That phone is not found";
            }

            context.Phones.Remove(phone);

            try
            {
                context.SaveChanges();

                return "The Phone was deleted successfully";
            }
            catch (Exception ex)
            {
                return $"There is an error : {ex.Message}";
            }
        }

        public async Task<ICollection<UserPhoneDto>> GetUserPhones(int userId)
        {
            List<UserPhoneDto> phoneDtos = new List<UserPhoneDto>();

            var userPhones = context.Phones.Where(Ph => Ph.UserID == userId);
            foreach(var phone in userPhones)
            {
                phoneDtos.Add(new UserPhoneDto
                {
                    PhoneNumber = phone.PhoneNumber
                });
            }

            return phoneDtos;
        }
        #endregion
    }
}
