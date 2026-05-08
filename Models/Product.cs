using System.ComponentModel.DataAnnotations;

namespace Website_Selling_Phones.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(0, 100000)]
    public decimal Price { get; set; }

    [StringLength(100)]
    public string? Brand { get; set; }

    [StringLength(100)]
    public string? Model { get; set; }

    public int StockQuantity { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsFeatured { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}