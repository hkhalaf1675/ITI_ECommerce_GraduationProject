using Core.DTOs.Product;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepositories
{
    public interface IProductRepository
    {
        // Modifcation :
        // -> add get top three review products

        Product GetById(int id);
        Task<Product> GetByIdAsync(int id);

        ICollection<Product> GetAll();

        ICollection<Product> GetAllWithSpec(QueryParametars parametars);

        public ICollection<Product> GetAllProductsWithDiscount();
        ICollection<Product> GetRelatedProductsByBrandName(string brand);
        public ICollection<Product> GetNewProducts();

        int GetCount();

        // add get top three products rate
        Task<ICollection<ProductToReturnDto>> GetTopThreeRate();


        #region ADMIN

        bool AddNew(Product product);
        bool Update(Product product);
        bool Delete(int id);

        public void UpdateWarranties(Product product, IEnumerable<WarrantiesDto> warrantyInput);

        public void UpdateImages(Product product, IEnumerable<Image> imageInput);

        int GetProductsCount();


        #endregion
    }
}
