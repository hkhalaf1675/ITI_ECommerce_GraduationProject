using Core.DTOs.ContactUs;
using Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ContactUsRepository : IContactUsRepository
    {
        private readonly ECommerceDBContext context;

        public ContactUsRepository(ECommerceDBContext _context)
        {
            context = _context;
        }
        public async Task<bool> AddMessage(ContactUsDto contactUsDto)
        {
            context.ContactUs.Add(new Core.Models.ContactUs
            {
                FullName = contactUsDto.FullName,
                Email = contactUsDto.Email,
                PhoneNumber = contactUsDto.PhoneNumber,
                Message = contactUsDto.Message
            });

            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ContactUsDto>> GetAll(int pageNumber)
        {
            List<ContactUsDto> contactUsDtos = new List<ContactUsDto>();

            var list = context.ContactUs.Skip((pageNumber - 1) * 6).Take(6);

            foreach(var item in list)
            {
                contactUsDtos.Add(new ContactUsDto
                {
                    FullName = item.FullName,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    Message = item.Message
                });
            }

            return contactUsDtos;
        }
    }
}
