using Website_Selling_Phones.Models;

namespace Website_Selling_Phones.Services;

public class DynamicPricingService
{
    private readonly Dictionary<string, decimal> _brandMultipliers = new()
    {
        ["Apple"] = 1.0m,
        ["Samsung"] = 1.0m,
        ["Google"] = 0.95m,
        ["OnePlus"] = 0.92m,
        ["Xiaomi"] = 0.90m,
        ["Nothing"] = 0.88m,
        ["Motorola"] = 0.85m
    };

    public decimal CalculatePrice(Phone phone, string? userTier = null)
    {
        var basePrice = phone.Price;
        var brandMultiplier = _brandMultipliers.GetValueOrDefault(phone.Brand ?? "", 1.0m);

        var tierDiscount = userTier?.ToLower() switch
        {
            "premium" => 0.05m,
            "vip" => 0.10m,
            _ => 0m
        };

        return Math.Round(basePrice * brandMultiplier * (1 - tierDiscount), 2);
    }

    public string GetPromoMessage(Phone phone)
    {
        if (phone.StockQuantity <= 5 && phone.StockQuantity > 0)
            return $"Only {phone.StockQuantity} left! Order soon.";

        if (phone.DiscountPercentage > 0)
            return $"Save {phone.DiscountPercentage}% today!";

        if (phone.Condition == PhoneCondition.New && phone.OriginalPrice == null)
            return "Best price guaranteed!";

        if (phone.Condition != PhoneCondition.New)
            return $"{phone.ConditionDisplayName} - Verified Quality";

        return "Limited time offer";
    }
}