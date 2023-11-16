using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        public ProductCondition? Condition { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        [Range(0, 100)]
        public int? Discount { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public int? Storage { get; set; }
        public int? Ram { get; set; }
        public string? Camera { get; set; }
        public string? CPU { get; set; }
        public float? ScreenSize { get; set; }
        public int? BatteryCapacity { get; set; }
        public string? OSVersion { get; set; }
        [ForeignKey("Category")]
        public int? CategoryID { get; set; }
        public virtual Category? Category { get; set; }
        [ForeignKey("Brand")]
        public int? BrandID { get; set; }
        public virtual Brand? Brand { get; set; }
        public virtual ICollection<ProductIdentity> Identities { get; set; } = new HashSet<ProductIdentity>();
        public virtual ICollection<Warranty> Warranties { get; set; } = new HashSet<Warranty>();
        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
        public virtual ICollection<Review> Reviews { get; set; }=new HashSet<Review>();
        public virtual ICollection<ShopingCart> Carts { get; set; }=new HashSet<ShopingCart>();
        public virtual ICollection<Wishlist> Wishlists { get; set; }=new HashSet<Wishlist>();
        public virtual ICollection<Favourite> Favourites { get; set; }=new HashSet<Favourite>();
        public virtual ICollection<OrderDetails> OrderDetails { get; set; } =new HashSet<OrderDetails>();


    }
    public enum ProductCondition
    {
        New, //0
        Used //1
    }
}
