using Core.DTOs.ContactUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IContactUsRepository
    {
        Task<bool> AddMessage(ContactUsDto contactUsDto);
        Task<IEnumerable<ContactUsDto>> GetAll(int pageNumber);
    }
}
