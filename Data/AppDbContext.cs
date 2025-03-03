//using e_commerce.models;
//using Microsoft.EntityFrameworkCore;

//namespace e_commerce.Data
//{
//    public class AppDbContext : DbContext
//    {
//        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//        {
//        }

//        public DbSet<Cart> Cart { get; set; }
//        public DbSet<Categories> Categories { get; set; }
//        public DbSet<Order> Order { get; set; }
//        public DbSet<Payments> Payments { get; set; }
//        public DbSet<Products> Products { get; set; }
//        public DbSet<User> User { get; set; }

//        public DbSet<Feedback> Feedback { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.Entity<Order>()
//                .Property(o => o.TotalAmount)
//                .HasColumnType("decimal(18, 2)");

//            modelBuilder.Entity<Payments>()
//                .Property(p => p.Amount)
//                .HasColumnType("decimal(18, 2)");

//            modelBuilder.Entity<Products>()
//                .Property(p => p.Price)
//                .HasColumnType("decimal(18, 2)");

//            modelBuilder.Entity<Order>()
//                .HasOne(o => o.User)
//                .WithMany()
//                .HasForeignKey(o => o.UserId);

//            modelBuilder.Entity<Payments>()
//                .HasOne(p => p.Order)
//                .WithMany()
//                .HasForeignKey(p => p.OrderId);

//            modelBuilder.Entity<Products>()
//                .HasOne(p => p.Category)
//                .WithMany()
//                .HasForeignKey(p => p.CategoryId);

//            modelBuilder.Entity<CartItems>()
//               .HasOne(ci => ci.Cart)
//               .WithMany(c => c.CartItems)
//               .HasForeignKey(ci => ci.CartId);

//            modelBuilder.Entity<CartItems>()
//                .HasOne(ci => ci.Product)
//                .WithMany()
//                .HasForeignKey(ci => ci.ProductId);

//            modelBuilder.Entity<Products>()
//                .HasOne(p => p.Category)
//                .WithMany()
//                .HasForeignKey(p => p.CategoryId);

//            modelBuilder.Entity<CartItems>()
//                .HasOne(ci => ci.Product)
//                .WithMany()
//                .HasForeignKey(ci => ci.ProductId);

//            modelBuilder.Entity<CartItems>()
//                .HasOne(ci => ci.Order)
//                .WithMany()
//                .HasForeignKey(ci => ci.OrderId);
//        }
//    }
//}

using e_commerce.models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Payments>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Products>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Payments>()
                .HasOne(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<Products>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);

            //modelBuilder.Entity<CartItems>()
            //    .HasOne(ci => ci.Cart)
            //                    //.WithMany(c => c.CartItems)
            //    .WithMany(ci => ci.CartItems)

            //    .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItems>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Products)
                .WithMany(p => p.Feedback)
                .HasForeignKey(f => f.ProductId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);
        }
    }
}