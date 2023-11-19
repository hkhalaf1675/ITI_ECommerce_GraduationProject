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

        public ICollection<Review> GetAll()
        {
            var reviews = _context.Reviews
                        .Include(r => r.Product)
                        .Include(r => r.User)
            .ToList();
            return reviews; 
        }

        public ICollection<Review> GetAllAdmin(int? pageIndex)
        {
            var reviews = _context.Reviews
                        .Include(r => r.Product)
                        .Include(r => r.User)
            .ToList();
            return reviews.Skip((int)((pageIndex - 1) * 6)).Take(6).ToList();
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
            // update it to check if the review is exists or not before adding it m 
            var userReview = 
                _context.Reviews.FirstOrDefault(R => R.UserID == Convert.ToInt32(review.UserID) && R.ProductID == Convert.ToInt32(review.ProductID));

            if (userReview != null)
            {
                userReview.Rating = review.Rating;
                userReview.Text = review.Text;
                userReview.Date = review.Date;
            }
            else
            {
                _context.Reviews.Add(review);
            }

            try
            {
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
