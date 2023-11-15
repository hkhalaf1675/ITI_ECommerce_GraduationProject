using Core.IRepositories;
using Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ECommerceDBContext _context;

        public ReviewRepository(ECommerceDBContext context) 
        {
            _context = context;
        }

        public Review? GetById(int id)
        {
            return _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefault(r => r.Id == id);
        }

        public ICollection<Review> GetAll(int? pageIndex)
        {
            var reviews = _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .ToList();


            // At first we have to check that the page index has a value in the first place before proceed on 
            if (pageIndex.HasValue && pageIndex > 0)
            {
                int startIndex = (pageIndex.Value - 1) * 6;

                // Here we make sure to not going out of bound!
                if (startIndex >= 0 && startIndex < reviews.Count)
                {
                    return reviews.Skip(startIndex).Take(6).ToList();
                }
                else
                {
                    // In case it went out of bound we send back an empty list     
                    return new List<Review>();
                }
            }
            else
            {
                // In case the page index is null here, we send back an empty list 
                return new List<Review>();
            }
        }


        public ICollection<Review> GetReviewsByProduct(int productId)
        {
            return _context.Reviews
                 .Where(r => r.ProductID == productId)
                .Include(r => r.Product)
                .Include(r => r.User)
               .ToList();
        }

        public Review GetByCompositeId(int ProductId, int userId)
        {
            return _context.Reviews
            .Where(r => r.ProductID == ProductId && r.UserID == userId)
            .Include(r => r.Product)
            .Include(r => r.User)
            .FirstOrDefault();
        }
        
        public bool Add(Review review)
        {
            try
            {
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public void Update(Review entity)
        //{
        //    throw new NotImplementedException();
        //}

        public bool Delete(int id)
        {
            try
            {
                _context.Reviews.Remove(GetById(id));
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }

}
