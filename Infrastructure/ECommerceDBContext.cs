using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ECommerceDBContext:IdentityDbContext<User,IdentityRole<int>,int>
    {
        public ECommerceDBContext(DbContextOptions<ECommerceDBContext> options) : base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Compiste PK
            // Composite PK for Table ProductIdentity
            modelBuilder.Entity<ProductIdentity>()
                .HasKey(PID => new { PID.SerialNumber, PID.ProductID });

            // Composite PK for Phone
            modelBuilder.Entity<Phone>()
                .HasKey(Ph => new { Ph.PhoneNumber, Ph.UserID });
            #endregion


            #region User Entity To make Delete Behavior Cascade
            modelBuilder.Entity<User>()
                    .HasMany(U => U.Favourites)
                    .WithOne(F => F.User)
                    .HasForeignKey(F => F.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(U => U.Carts)
                .WithOne(C => C.User)
                .HasForeignKey(C => C.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(U => U.Wishlists)
                .WithOne(W => W.User)
                .HasForeignKey(W => W.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(U => U.Orders)
                .WithOne(O => O.User)
                .HasForeignKey(O => O.UserId)
                .OnDelete(DeleteBehavior.SetNull); // to make it do not delete the row from database

            modelBuilder.Entity<User>()
                .HasMany(U => U.Reviews)
                .WithOne(RV => RV.User)
                .HasForeignKey(RV => RV.UserID)
                .OnDelete(DeleteBehavior.SetNull); // to make it do not delete the row from database

            modelBuilder.Entity<User>()
                .HasMany(U => U.Addresses)
                .WithOne(A => A.User)
                .HasForeignKey(A => A.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(U => U.Phones)
                .WithOne(Ph => Ph.User)
                .HasForeignKey(Ph => Ph.UserID)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion


            #region Product Behavior
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Carts)
                .WithOne(sc => sc.Product)
                .HasForeignKey(sc => sc.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Wishlists)
                .WithOne(w => w.Product)
                .HasForeignKey(w => w.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Favourites)
                .WithOne(f => f.Product)
                .HasForeignKey(f => f.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(rv => rv.Product)
                .HasForeignKey(rv => rv.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Warranties)
                .WithOne(w => w.Product)
                .HasForeignKey(w => w.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrdersDetails { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductIdentity> ProductIdentities { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Shipment> Shipment { get; set; }
        public DbSet<ShopingCart> ShoppingCarts { get; set; }
        public DbSet<Warranty> Warrantys { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Favourite> Favourite { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
    }
}
