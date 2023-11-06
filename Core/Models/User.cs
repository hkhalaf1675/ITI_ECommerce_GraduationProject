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
        public string? ImageUrl { get; set; }
        public ICollection<Phone> Phones { get; set; } = new HashSet<Phone>();
        public ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public ICollection<ShopingCart> Carts { get; set; } = new HashSet<ShopingCart>();
        public ICollection<Wishlist> Wishlists { get; set; } = new HashSet<Wishlist>();
        public ICollection<Favourite> Favourites { get; set; } = new HashSet<Favourite>();
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

    }
}
