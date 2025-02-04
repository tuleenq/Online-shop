using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopRazor.models;

namespace ShopRazor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<Poducts> Products { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Product entity
            modelBuilder.Entity<Poducts>(entity =>
            {
                entity.Property(e => e.Price)
                      .HasPrecision(18, 2);
            });

            // Call the base class's OnModelCreating method
            base.OnModelCreating(modelBuilder);
        }
    }
}
