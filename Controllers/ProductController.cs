using Microsoft.AspNetCore.Mvc;
using Website_Selling_Phones.Services;
using Website_Selling_Phones.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Website_Selling_Phones.Controllers;

public class ProductController : Controller
{
    private readonly MockDataService _mockData;
    private readonly DynamicPricingService _pricing;
    private readonly IHubContext<InventoryHub> _hubContext;

    public ProductController(MockDataService mockData, DynamicPricingService pricing, IHubContext<InventoryHub> hubContext)
    {
        _mockData = mockData;
        _pricing = pricing;
        _hubContext = hubContext;
    }

    public IActionResult Index(string? brand, decimal? minPrice, decimal? maxPrice, string? category, string? condition, string? sort)
    {
        var phones = _mockData.FilterPhones(brand, minPrice, maxPrice, category, condition);

        // Sorting
        phones = sort?.ToLower() switch
        {
            "price_asc" => phones.OrderBy(p => p.DiscountedPrice).ToList(),
            "price_desc" => phones.OrderByDescending(p => p.DiscountedPrice).ToList(),
            "name_asc" => phones.OrderBy(p => p.Name).ToList(),
            "name_desc" => phones.OrderByDescending(p => p.Name).ToList(),
            "newest" => phones.OrderByDescending(p => p.CreatedAt).ToList(),
            _ => phones.OrderByDescending(p => p.IsFeatured).ThenBy(p => p.Name).ToList()
        };

        ViewData["Brands"] = _mockData.GetBrands();
        ViewData["Categories"] = _mockData.GetCategories();
        ViewData["CurrentBrand"] = brand;
        ViewData["CurrentCategory"] = category;
        ViewData["MinPrice"] = minPrice;
        ViewData["MaxPrice"] = maxPrice;
        ViewData["CurrentSort"] = sort ?? "featured";
        ViewData["CurrentCondition"] = condition;

        // Check for low stock items
        var lowStock = phones.Where(p => p.StockQuantity <= 5 && p.StockQuantity > 0).ToList();
        ViewData["LowStockAlerts"] = lowStock;

        return View(phones);
    }

    public IActionResult Details(int id)
    {
        var phone = _mockData.GetPhoneById(id);
        if (phone == null)
            return NotFound();

        var calculatedPrice = _pricing.CalculatePrice(phone);
        var promoMessage = _pricing.GetPromoMessage(phone);

        ViewData["CalculatedPrice"] = calculatedPrice;
        ViewData["PromoMessage"] = promoMessage;

        return View(phone);
    }
}