using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserProfileDtos
{
    public class AdminUserInfo
    {
        public int? Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public string? Password { get; set; }

        // add the address and phone
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
    }
}
