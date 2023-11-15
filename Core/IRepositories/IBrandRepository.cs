using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IBrandRepository
    {
        ICollection<Brand> GetAll();
        Brand GetById(int id);
        Task<Brand> GetByIdAsync(int id);
        Brand Find(Expression<Func<Brand, bool>> predicate);
        ICollection<Brand> GetByName(string name);
        bool AddNew(Brand brand);
        bool Update(Brand brand);
        bool Delete(int id);
    }
}
