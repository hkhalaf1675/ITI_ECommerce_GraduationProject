using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetAll();
        Category GetById(int id);
        ICollection<Category> GetByName(string categoryName);
        bool AddNew(Category category);
        bool Update(Category category);
        bool Delete(int id);
    }
}
