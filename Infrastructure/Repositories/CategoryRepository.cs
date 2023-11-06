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
    public class CategoryRepository : ICategoryRepository
    {
        ECommerceDBContext context;
        public CategoryRepository(ECommerceDBContext _context)
        {
            context = _context;
        }
        public ICollection<Category> GetAll()
        {
            return context.Categories.ToList();
        }
        public Category GetById(int id)
        {
            return context.Categories.Find(id);
        }
        // to make function async ---->
        public async Task<Category> GetByIdAsync(int id)
        {
            return await context.Categories.FindAsync(id);
        }
        // function that take delegate (expression)
        public Category Find(Expression<Func<Category, bool>> predicate)
        {
            return context.Categories.SingleOrDefault(predicate);
        } 
        public ICollection<Category> GetByName(string categoryName)
        {
            return context.Categories.Where(C => C.Name == categoryName).ToList();
        }
        public bool AddNew(Category category)
        {
            try
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool Update(Category category)
        {
            try
            {
                context.Categories.Update(category);
                context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                context.Categories.Remove(GetById(id));
                context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}
