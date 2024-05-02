using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DoAn_TranTheAnh_2020607026.Models
{
    public partial class Fashion : DbContext
    {
        public Fashion()
            : base("name=Fashion")
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>()
                .Property(e => e.BrandName)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.OrderStatus)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Address_Delivery)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.PhoneCustomer)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.OrderCode)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Images)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.CartItems)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rate>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Size>()
                .Property(e => e.NameSize)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsUnicode(false);
        }
    }
}
