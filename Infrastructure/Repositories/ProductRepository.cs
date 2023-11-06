using Azure;
using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        ECommerceDBContext context;
        public ProductRepository(ECommerceDBContext _context)
        {
            context = _context;
        }

        public Product GetById(int id)
        {
            return context.Product
                    .Include(p => p.Reviews)
                    .Include(p => p.Images)
                    .Include(p => p.Warranties)
                    .FirstOrDefault(p => p.Id == id);
        }

        // to make function async ---->
        public async Task<Product> GetByIdAsync(int id)
        {
            return await context.Product
                .Include(p => p.Reviews)
                    .Include(p => p.Images)
                    .Include(p => p.Warranties)
                    .FirstOrDefaultAsync(p => p.Id == id);
        }


        public ICollection<Product> GetAll()
        {
            return context.Product.Include(p => p.Reviews).Include(p => p.Images).OrderBy(p => p.Name).ToList().ToList();
        }


        public ICollection<Product> GetAllWithSpec(QueryParametars parametars)
        {
            var products = GetAll();
       
            if (parametars.categoryid > 0)
            {
                products = products.Where(p => p.CategoryID == parametars.categoryid).ToList();
            }

            if (parametars.brandId > 0)
            {
                products = products.Where(p => p.BrandID == parametars.brandId).ToList();
            }

            //if (parametars.Search != null || parametars.Search != "")
            //{
            //    products = products.Where(p => p.Name.Contains(parametars.Search)).ToList();
            //}

            if (parametars.MaxPrice > 0)
            {
                products = products.Where(p => p.Price - (p.Price * p.Discount / 100) <= parametars.MaxPrice).ToList();
            }
            if (parametars.MinPrice > 0)
            {
                products = products.Where(p => p.Price - (p.Price * p.Discount / 100) >= parametars.MinPrice).ToList();
            }

            if (parametars.Rating > 0)
            {
                products = products.Where(p => p.Reviews.Average(r => r.Rating) >= parametars.Rating).ToList();
            }

            if(parametars.sort == "priceAsc")
            {
                products = products.OrderBy(p => p.Price).ToList();
            }

            if (parametars.sort == "priceDesc")
            {
                products = products.OrderByDescending(p => p.Price).ToList();
            }

            return products.Skip((parametars.PageIndex - 1) * parametars.PageSize).Take(parametars.PageSize).ToList() ;
        }

        public ICollection<Product> GetAllProductsWithDiscount()
        {
            return context.Product
            .Include(p => p.Reviews)
            .Include(p => p.Images)
            .Where(p => p.Discount > 0)
            .OrderByDescending(p => p.Discount)
            .Take(6).ToList();
        }

        public ICollection<Product> GetRelatedProductsByBrandName(string brand)
        {
            return context.Product
            .Include(p => p.Brand)
           .Include(p => p.Reviews)
           .Include(p => p.Images)
           .Where(p => p.Brand.Name == brand)
           .Take(6).ToList();
        }


        public ICollection<Product> GetNewProducts()
        {
            return context.Product
             .Include(p => p.Reviews)
             .Include(p => p.Images)
             .OrderByDescending(p => p.Id)
             .Take(6).ToList();
        }

        public int GetCount()
        {
            return context.Product.Count();
        }



        #region ADMIN

        public bool AddNew(Product product)
        {
            try
            {
                context.Product.Add(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Update(Product product)
        {
            try
            {
                context.Product.Update(product);
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
                context.Product.Remove(GetById(id));
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        


        #endregion

    }
}
