using TechTest.Data.Models;

namespace TechTest.Data;

using Microsoft.EntityFrameworkCore;

public class TechTestDbContext(DbContextOptions<TechTestDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var productEntity = modelBuilder.Entity<Product>();
        productEntity
            .HasKey(e => e.ProductId);
        productEntity
            .HasOne<Category>(e => e.Category)
            .WithMany(e => e.Products);
        productEntity
            .Property(e => e.Price)
            .HasColumnType("decimal(18, 2)");

        var categoryEntity = modelBuilder.Entity<Category>();
        categoryEntity
            .HasKey(e => e.CategoryId);
    }
}
