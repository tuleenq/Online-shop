using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;

namespace MyShop.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<User> Usesrs { get; set; } 
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Ctegory> category { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Call the base class's OnModelCreating method
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
               .HasOne(p => p.Ctegory)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CtegoryId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductReview>()
            .HasKey(pr => pr.Id);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.Reviews) // Update Product class to include `public ICollection<ProductReview> Reviews`
                .HasForeignKey(pr => pr.ProductId);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.User)
                .WithMany() // If you want a navigation property in User, add `public ICollection<ProductReview> Reviews` in User class
                .HasForeignKey(pr => pr.UserId);
        }
       
    }
}
