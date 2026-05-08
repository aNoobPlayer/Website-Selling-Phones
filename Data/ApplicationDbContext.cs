using Microsoft.EntityFrameworkCore;
using Website_Selling_Phones.Models;

namespace Website_Selling_Phones.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed some sample phone data
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "iPhone 15 Pro",
                Description = "Latest iPhone with A17 Pro chip, titanium design, and advanced camera system",
                Price = 999.99m,
                Brand = "Apple",
                Model = "iPhone 15 Pro",
                StockQuantity = 50,
                ImageUrl = "https://via.placeholder.com/300x400/000000/FFFFFF?text=iPhone+15+Pro",
                IsFeatured = true
            },
            new Product
            {
                Id = 2,
                Name = "Samsung Galaxy S24 Ultra",
                Description = "Powerful Android phone with S Pen, AI features, and 200MP camera",
                Price = 1299.99m,
                Brand = "Samsung",
                Model = "Galaxy S24 Ultra",
                StockQuantity = 40,
                ImageUrl = "https://via.placeholder.com/300x400/1a1a2e/FFFFFF?text=Galaxy+S24+Ultra",
                IsFeatured = true
            },
            new Product
            {
                Id = 3,
                Name = "Google Pixel 8 Pro",
                Description = "Google's flagship with best-in-class AI photography and pure Android",
                Price = 899.99m,
                Brand = "Google",
                Model = "Pixel 8 Pro",
                StockQuantity = 30,
                ImageUrl = "https://via.placeholder.com/300x400/4285f4/FFFFFF?text=Pixel+8+Pro",
                IsFeatured = true
            },
            new Product
            {
                Id = 4,
                Name = "OnePlus 12",
                Description = "Fast and smooth performance with Hasselblad cameras",
                Price = 799.99m,
                Brand = "OnePlus",
                Model = "OnePlus 12",
                StockQuantity = 25,
                ImageUrl = "https://via.placeholder.com/300x400/f50514/FFFFFF?text=OnePlus+12",
                IsFeatured = false
            },
            new Product
            {
                Id = 5,
                Name = "Xiaomi 14 Ultra",
                Description = "Leica optics, Snapdragon 8 Gen 3, and premium design",
                Price = 1099.99m,
                Brand = "Xiaomi",
                Model = "14 Ultra",
                StockQuantity = 20,
                ImageUrl = "https://via.placeholder.com/300x400/ff6900/FFFFFF?text=Xiaomi+14+Ultra",
                IsFeatured = false
            },
            new Product
            {
                Id = 6,
                Name = "iPhone 15",
                Description = "Great iPhone experience with Dynamic Island and USB-C",
                Price = 799.99m,
                Brand = "Apple",
                Model = "iPhone 15",
                StockQuantity = 60,
                ImageUrl = "https://via.placeholder.com/300x400/000000/FFFFFF?text=iPhone+15",
                IsFeatured = false
            }
        );
    }
}