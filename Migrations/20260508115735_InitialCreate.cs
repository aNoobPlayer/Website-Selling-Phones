using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Website_Selling_Phones.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CreatedAt", "Description", "ImageUrl", "IsFeatured", "Model", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, "Apple", new DateTime(2026, 5, 8, 18, 57, 35, 479, DateTimeKind.Local).AddTicks(6860), "Latest iPhone with A17 Pro chip, titanium design, and advanced camera system", "https://via.placeholder.com/300x400/000000/FFFFFF?text=iPhone+15+Pro", true, "iPhone 15 Pro", "iPhone 15 Pro", 999.99m, 50 },
                    { 2, "Samsung", new DateTime(2026, 5, 8, 18, 57, 35, 479, DateTimeKind.Local).AddTicks(6887), "Powerful Android phone with S Pen, AI features, and 200MP camera", "https://via.placeholder.com/300x400/1a1a2e/FFFFFF?text=Galaxy+S24+Ultra", true, "Galaxy S24 Ultra", "Samsung Galaxy S24 Ultra", 1299.99m, 40 },
                    { 3, "Google", new DateTime(2026, 5, 8, 18, 57, 35, 479, DateTimeKind.Local).AddTicks(6889), "Google's flagship with best-in-class AI photography and pure Android", "https://via.placeholder.com/300x400/4285f4/FFFFFF?text=Pixel+8+Pro", true, "Pixel 8 Pro", "Google Pixel 8 Pro", 899.99m, 30 },
                    { 4, "OnePlus", new DateTime(2026, 5, 8, 18, 57, 35, 479, DateTimeKind.Local).AddTicks(6891), "Fast and smooth performance with Hasselblad cameras", "https://via.placeholder.com/300x400/f50514/FFFFFF?text=OnePlus+12", false, "OnePlus 12", "OnePlus 12", 799.99m, 25 },
                    { 5, "Xiaomi", new DateTime(2026, 5, 8, 18, 57, 35, 479, DateTimeKind.Local).AddTicks(6893), "Leica optics, Snapdragon 8 Gen 3, and premium design", "https://via.placeholder.com/300x400/ff6900/FFFFFF?text=Xiaomi+14+Ultra", false, "14 Ultra", "Xiaomi 14 Ultra", 1099.99m, 20 },
                    { 6, "Apple", new DateTime(2026, 5, 8, 18, 57, 35, 479, DateTimeKind.Local).AddTicks(6895), "Great iPhone experience with Dynamic Island and USB-C", "https://via.placeholder.com/300x400/000000/FFFFFF?text=iPhone+15", false, "iPhone 15", "iPhone 15", 799.99m, 60 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
