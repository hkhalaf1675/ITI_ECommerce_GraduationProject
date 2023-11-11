using Core.DTOs;
using ECommerceGP.Bl.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IAccountManagerServices
    {
        Task<AuthModel> RegisterAsync(UserRegisterDTO registerDto);
        Task<AuthModel> LoginAsync(LoginCredentialsDTO loginDto);
    }
}
