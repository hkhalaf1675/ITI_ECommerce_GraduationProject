using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class User:IdentityUser<int>
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public virtual ICollection<Phone> Phones { get; set; } = new HashSet<Phone>();
        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<ShopingCart> Carts { get; set; } = new HashSet<ShopingCart>();
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new HashSet<Wishlist>();
        public virtual ICollection<Favourite> Favourites { get; set; } = new HashSet<Favourite>();
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

    }
}
