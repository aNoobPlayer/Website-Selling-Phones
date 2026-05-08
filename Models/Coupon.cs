using System.ComponentModel.DataAnnotations;

namespace Website_Selling_Phones.Models;

public class Coupon
{
    [Key]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    public decimal DiscountPercent { get; set; }

    public int MaxUses { get; set; } = 100;
    public int UsedCount { get; set; }

    public DateTime ValidFrom { get; set; } = DateTime.Now;
    public DateTime ValidUntil { get; set; } = DateTime.Now.AddMonths(1);

    public bool IsValid => UsedCount < MaxUses && DateTime.Now >= ValidFrom && DateTime.Now <= ValidUntil;
}
