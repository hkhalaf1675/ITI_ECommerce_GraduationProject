using Core.IRepositories;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BrandRepository:IBrandRepository
    {
        ECommerceDBContext context;
        public BrandRepository(ECommerceDBContext _context)
        {
            context = _context;
        }
        public ICollection<Brand> GetAll()
        {
            return context.Brands.ToList();
        }
        public Brand GetById(int id)
        {
            return context.Brands.Find(id);
        }
        // to make function async ---->
        public async Task<Brand> GetByIdAsync(int id)
        {
            return await context.Brands.FindAsync(id);
        }
        // function that take delegate (expression)
        public Brand Find(Expression<Func<Brand, bool>> predicate)
        {
            return context.Brands.SingleOrDefault(predicate);
        }
        public ICollection<Brand> GetByName(string name)
        {
            return context.Brands.Where(C => C.Name == name).ToList();
        }
        public bool AddNew(Brand brand)
        {
            try
            {
                context.Brands.Add(brand);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Update(Brand brand)
        {
            try
            {
                context.Brands.Update(brand);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                context.Brands.Remove(GetById(id));
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
