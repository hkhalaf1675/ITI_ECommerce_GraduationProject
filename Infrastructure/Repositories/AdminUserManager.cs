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
    public class AdminUserManager:IAdminUserManager
    {
        // modifiaction : 
        // -> injcet the roleManager
        // -> modify the user mapping
        // -> check if the role is exists or not
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public AdminUserManager(UserManager<User> _userManager,RoleManager<IdentityRole<int>> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }
        public async Task<bool> AdminDeleteUser(string userName)
        {
            User user = await userManager.FindByNameAsync(userName);
            if (user is null)
                return true;

            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
                return true;

            return false;
        }

        public async Task<bool> AdminAddUser(AdminUserInfo newUser)
        {
            if (newUser is null)
                return false;

            User _user = new User
            {
                UserName = newUser.UserName,
                FullName = newUser.FullName,
                Email = newUser.Email,
                Address = newUser.Address,
                PhoneNumber = newUser.PhoneNumber
            };

            var result = await userManager.CreateAsync(_user, newUser.Password);

            if (!result.Succeeded)
                return false;

            // check the role exists
            var check = await roleManager.RoleExistsAsync(newUser.Role);

            if (!check)
            {
                var role = new IdentityRole<int>(newUser.Role);

                var checkRoleCreation = await roleManager.CreateAsync(role);

                if (!checkRoleCreation.Succeeded)
                    return false;
            }

            try
            {
                await userManager.AddToRoleAsync(_user, newUser.Role);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<AdminUserInfo>> GetAllUsers(int pageNumber)
        {
            List<AdminUserInfo> AllUsers = new List<AdminUserInfo>();

            List<User> users = userManager.Users.Skip((pageNumber - 1) * 6).Take(6).ToList();

            foreach (User user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                AllUsers.Add(new AdminUserInfo
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault()
                });
            }

            return AllUsers;
        }

        public async Task<int> GetUsersCount()
        {
            return userManager.Users.Count();
        }
    }
}
