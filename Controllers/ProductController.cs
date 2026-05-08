using Microsoft.AspNetCore.Mvc;
using Website_Selling_Phones.Services;
using Website_Selling_Phones.Models;
using Website_Selling_Phones.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Website_Selling_Phones.Controllers;

public class ProductController : Controller
{
    private readonly MockDataService _mockData;
    private readonly DynamicPricingService _pricing;
    private readonly IHubContext<InventoryHub> _hubContext;
    private const string ReviewsSessionKey = "ProductReviews";

    public ProductController(MockDataService mockData, DynamicPricingService pricing, IHubContext<InventoryHub> hubContext)
    {
        _mockData = mockData;
        _pricing = pricing;
        _hubContext = hubContext;
    }

    public IActionResult Index(string? brand, decimal? minPrice, decimal? maxPrice, string? category,
        string? condition, string? sort, string? search, int page = 1, int pageSize = 8)
    {
        var phones = _mockData.FilterPhones(brand, minPrice, maxPrice, category, condition);

        // Search
        if (!string.IsNullOrWhiteSpace(search))
        {
            phones = phones.Where(p =>
                (p.Name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Brand?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
        }

        // Sorting
        phones = sort?.ToLower() switch
        {
            "price_asc" => phones.OrderBy(p => p.DiscountedPrice).ToList(),
            "price_desc" => phones.OrderByDescending(p => p.DiscountedPrice).ToList(),
            "name_asc" => phones.OrderBy(p => p.Name).ToList(),
            "name_desc" => phones.OrderByDescending(p => p.Name).ToList(),
            "newest" => phones.OrderByDescending(p => p.CreatedAt).ToList(),
            "rating" => new Random().Next(0, 2) == 1 ? phones.OrderBy(p => p.Id).ToList() : phones.OrderByDescending(p => p.Id).ToList(),
            _ => phones.OrderByDescending(p => p.IsFeatured).ThenBy(p => p.Name).ToList()
        };

        // Pagination
        var totalItems = phones.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        var pagedPhones = phones.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        ViewData["Brands"] = _mockData.GetBrands();
        ViewData["Categories"] = _mockData.GetCategories();
        ViewData["CurrentBrand"] = brand;
        ViewData["CurrentCategory"] = category;
        ViewData["MinPrice"] = minPrice;
        ViewData["MaxPrice"] = maxPrice;
        ViewData["CurrentSort"] = sort ?? "featured";
        ViewData["CurrentCondition"] = condition;
        ViewData["Search"] = search;
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        ViewData["TotalItems"] = totalItems;
        ViewData["PageSize"] = pageSize;

        var lowStock = phones.Where(p => p.StockQuantity <= 5 && p.StockQuantity > 0).ToList();
        ViewData["LowStockAlerts"] = lowStock;

        return View(pagedPhones);
    }

    public IActionResult Details(int id)
    {
        var phone = _mockData.GetPhoneById(id);
        if (phone == null) return NotFound();

        var calculatedPrice = _pricing.CalculatePrice(phone);
        var promoMessage = _pricing.GetPromoMessage(phone);

        ViewData["CalculatedPrice"] = calculatedPrice;
        ViewData["PromoMessage"] = promoMessage;

        // Get reviews
        var reviewsJson = HttpContext.Session.GetString(ReviewsSessionKey);
        var allReviews = string.IsNullOrEmpty(reviewsJson)
            ? new List<Review>()
            : JsonSerializer.Deserialize<List<Review>>(reviewsJson) ?? new List<Review>();

        var productReviews = allReviews.Where(r => r.PhoneId == id).OrderByDescending(r => r.CreatedAt).ToList();
        ViewData["Reviews"] = productReviews;

        return View(phone);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitReview(int phoneId, int rating, string comment)
    {
        var reviewsJson = HttpContext.Session.GetString(ReviewsSessionKey);
        var reviews = string.IsNullOrEmpty(reviewsJson)
            ? new List<Review>()
            : JsonSerializer.Deserialize<List<Review>>(reviewsJson) ?? new List<Review>();

        var review = new Review
        {
            Id = reviews.Count + 1,
            PhoneId = phoneId,
            UserId = User.Identity?.Name ?? "anonymous",
            UserName = User.Identity?.Name ?? "Guest User",
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.Now
        };

        reviews.Add(review);
        HttpContext.Session.SetString(ReviewsSessionKey, JsonSerializer.Serialize(reviews));

        TempData["ReviewSuccess"] = "Your review has been submitted!";
        return RedirectToAction("Details", new { id = phoneId });
    }
}