using Core.DTOs.UserProfileDtos;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IAdminUserManager
    {
        Task<bool> AdminDeleteUser(string userName);
        Task<int> GetUsersCount();
        Task<IEnumerable<AdminUserInfo>> GetAllUsers(int pageNumber);
        Task<bool> AdminAddUser(AdminUserInfo newUser);
    }
}
