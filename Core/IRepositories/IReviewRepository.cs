using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IReviewRepository
    {
        Review? GetById(int id);
        ICollection<Review> GetAll();
        ICollection<Review> GetAllAdmin(int? pageIndex);
        ICollection<Review> GetReviewsByProduct(int productId);
        //ICollection<Review> GetReviewsWithProductAndUser();
        Review GetByCompositeId(int ProductID, int userID);

        bool Add(Review entity);
        //void Update(Review entity);
        bool Delete(int id);
       
    }
}
