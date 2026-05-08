using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website_Selling_Phones.Models;

public enum PhoneCondition
{
    [Display(Name = "New")]
    New,
    [Display(Name = "Refurbished - Excellent")]
    RefurbishedExcellent,
    [Display(Name = "Refurbished - Good")]
    RefurbishedGood,
    [Display(Name = "Used - Like New")]
    UsedLikeNew,
    [Display(Name = "Used - Fair")]
    UsedFair
}

public class Phone
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Brand")]
    public string? Brand { get; set; }

    [StringLength(100)]
    [Display(Name = "Model Number")]
    public string? Model { get; set; }

    [Required]
    [Range(0, 100000)]
    [DataType(DataType.Currency)]
    [Display(Name = "Price")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Display(Name = "Original Price")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal? OriginalPrice { get; set; }

    [Required]
    [Display(Name = "Condition")]
    public PhoneCondition Condition { get; set; } = PhoneCondition.New;

    [Display(Name = "Discount")]
    [Range(0, 100)]
    public int DiscountPercentage { get; set; }

    [StringLength(1000)]
    [Display(Name = "Description")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [StringLength(2000)]
    [Display(Name = "Full Description")]
    public string? FullDescription { get; set; }

    [Display(Name = "Stock")]
    public int StockQuantity { get; set; }

    [StringLength(500)]
    [Display(Name = "Image URL")]
    [DataType(DataType.ImageUrl)]
    public string? ImageUrl { get; set; }

    [Display(Name = "Gallery Images")]
    public List<string> GalleryImages { get; set; } = new();

    [Display(Name = "Specifications")]
    [NotMapped]
    public PhoneSpecs? Specs { get; set; }

    [Display(Name = "Variants")]
    public List<ProductVariant> Variants { get; set; } = new();

    [Display(Name = "Rating")]
    [Range(0, 5)]
    public double Rating { get; set; }

    [Display(Name = "Review Count")]
    public int ReviewCount { get; set; }

    [Display(Name = "Total Sold")]
    public int TotalSold { get; set; }

    [Display(Name = "Featured")]
    public bool IsFeatured { get; set; }

    [Display(Name = "Category")]
    [StringLength(100)]
    public string? Category { get; set; }

    [Display(Name = "Date Added")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [NotMapped]
    public decimal DiscountedPrice => DiscountPercentage > 0
        ? Math.Round(Price - (Price * DiscountPercentage / 100), 2)
        : Price;

    [NotMapped]
    public string ConditionBadgeClass => Condition switch
    {
        PhoneCondition.New => "badge-new",
        PhoneCondition.RefurbishedExcellent => "badge-refurbished",
        PhoneCondition.RefurbishedGood => "badge-refurbished",
        PhoneCondition.UsedLikeNew => "badge-used",
        PhoneCondition.UsedFair => "badge-used",
        _ => "badge-new"
    };

    [NotMapped]
    public string ConditionDisplayName => Condition switch
    {
        PhoneCondition.New => "New",
        PhoneCondition.RefurbishedExcellent => "Refurbished",
        PhoneCondition.RefurbishedGood => "Refurbished",
        PhoneCondition.UsedLikeNew => "Used",
        PhoneCondition.UsedFair => "Used",
        _ => "New"
    };

    [NotMapped]
    public string StockStatus => StockQuantity switch
    {
        > 10 => "In Stock",
        > 0 => "Low Stock",
        _ => "Out of Stock"
    };

    [NotMapped]
    public string StockCssClass => StockQuantity switch
    {
        > 10 => "text-success",
        > 0 => "text-warning stock-urgent-pulse",
        _ => "text-danger"
    };
}