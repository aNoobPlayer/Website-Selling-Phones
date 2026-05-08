namespace Website_Selling_Phones.Models;

public class ProductVariant
{
    public string Type { get; set; } = string.Empty; // "Storage", "Color"
    public string Value { get; set; } = string.Empty;
    public decimal PriceModifier { get; set; } // additional cost
}
