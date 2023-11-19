using Core.DTOs.UserDtos;
using Core.DTOs.UserProfileDtos;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IUserRepository
    {
        Task<ProfileDto> GetUserInfo(User user);
        Task<bool> DeleteUser(User user, string password);
        Task<bool> UpdateUser(User user,UserUpdateDto userUpdateDto);
        Task<bool> AddAddress(User user, UserAddressDto address);
        Task<bool> DeleteAddress(User user, int addressId);
        Task<ICollection<UserAddressDto>> userAddresses(User user);
        Task<ICollection<UserOrderDto>> userOrders(User user);
        Task<bool> ChangePassword(User user,UserResetPasswordDto resetPassword);
        Task<string?> AddPhone(int userId, string? phoneNumber);
        Task<string?> DeletePhone(int userId , string? phoneNumber);
        Task<ICollection<UserPhoneDto>> GetUserPhones(int userId);
    }
}
