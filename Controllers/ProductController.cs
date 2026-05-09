using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
    private readonly ICompositeViewEngine _viewEngine;
    private const string ReviewsSessionKey = "ProductReviews";
    private const string CouponsSessionKey = "AppliedCoupons";

    public ProductController(MockDataService mockData, DynamicPricingService pricing, IHubContext<InventoryHub> hubContext, ICompositeViewEngine viewEngine)
    {
        _mockData = mockData;
        _pricing = pricing;
        _hubContext = hubContext;
        _viewEngine = viewEngine;
    }

    [ResponseCache(Duration = 60)]
    public IActionResult Index(string? brand, decimal? minPrice, decimal? maxPrice, string? category,
        string? condition, string? sort, string? search, string? view, int page = 1, int pageSize = 8)
    {
        var phones = _mockData.FilterPhones(brand, minPrice, maxPrice, category, condition);

        if (!string.IsNullOrWhiteSpace(search))
        {
            phones = phones.Where(p =>
                (p.Name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Brand?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
        }

        phones = sort?.ToLower() switch
        {
            "price_asc" => phones.OrderBy(p => p.DiscountedPrice).ToList(),
            "price_desc" => phones.OrderByDescending(p => p.DiscountedPrice).ToList(),
            "name_asc" => phones.OrderBy(p => p.Name).ToList(),
            "name_desc" => phones.OrderByDescending(p => p.Name).ToList(),
            "newest" => phones.OrderByDescending(p => p.CreatedAt).ToList(),
            "rating" => phones.OrderByDescending(p => p.Rating).ToList(),
            "popularity" => phones.OrderByDescending(p => p.TotalSold).ToList(),
            _ => phones.OrderByDescending(p => p.IsFeatured).ThenBy(p => p.Name).ToList()
        };

        var totalItems = phones.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        var pagedPhones = phones.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        ViewData["Brands"] = _mockData.GetBrands();
        ViewData["Categories"] = _mockData.GetCategories();
        ViewData["CurrentBrand"] = brand;
        ViewData["CurrentCategory"] = category;
        ViewData["MinPrice"] = minPrice ?? 0;
        ViewData["MaxPrice"] = maxPrice ?? 1500;
        ViewData["CurrentSort"] = sort ?? "featured";
        ViewData["CurrentCondition"] = condition;
        ViewData["Search"] = search;
        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        ViewData["TotalItems"] = totalItems;
        ViewData["PageSize"] = pageSize;
        ViewData["ViewMode"] = view ?? "grid";
        ViewData["ActiveFilterCount"] = (brand != null ? 1 : 0) + (category != null ? 1 : 0) + (condition != null ? 1 : 0) + (minPrice != null || maxPrice != null ? 1 : 0);

        return View(pagedPhones);
    }

    [ResponseCache(Duration = 30)]
    public IActionResult Details(int id)
    {
        var phone = _mockData.GetPhoneById(id);
        if (phone == null) return NotFound();

        var calculatedPrice = _pricing.CalculatePrice(phone);
        var promoMessage = _pricing.GetPromoMessage(phone);

        ViewData["CalculatedPrice"] = calculatedPrice;
        ViewData["PromoMessage"] = promoMessage;
        ViewData["RelatedPhones"] = _mockData.GetRelatedPhones(phone, 4);

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
        if (rating < 1 || rating > 5 || string.IsNullOrWhiteSpace(comment) || comment.Trim().Length < 10)
        {
            TempData["ReviewError"] = "Please provide a rating (1-5 stars) and a review comment (at least 10 characters).";
            return RedirectToAction("Details", new { id = phoneId });
        }

        if (comment.Length > 500)
            comment = comment.Substring(0, 500);

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
            Comment = comment.Trim(),
            CreatedAt = DateTime.Now
        };

        reviews.Add(review);
        HttpContext.Session.SetString(ReviewsSessionKey, JsonSerializer.Serialize(reviews));

        TempData["ReviewSuccess"] = "Your review has been submitted! Thank you for your feedback.";
        return RedirectToAction("Details", new { id = phoneId });
    }

    [HttpGet]
    public IActionResult SearchSuggestions(string q)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
            return Json(Array.Empty<object>());

        var results = _mockData.SearchPhones(q).Select(p => new {
            id = p.Id,
            name = p.Name,
            brand = p.Brand,
            price = p.Price,
            image = p.ImageUrl
        });

        return Json(results);
    }

    [HttpGet]
    public IActionResult QuickView(int id)
    {
        var phone = _mockData.GetPhoneById(id);
        if (phone == null) return NotFound();
        return PartialView("_QuickView", phone);
    }

    [HttpPost]
    public async Task<IActionResult> FilterAjax(string? brand, decimal? minPrice, decimal? maxPrice, string? category,
        string? condition, string? sort, string? search, string? view, int page = 1, int pageSize = 8)
    {
        var phones = _mockData.FilterPhones(brand, minPrice, maxPrice, category, condition);

        if (!string.IsNullOrWhiteSpace(search))
        {
            phones = phones.Where(p =>
                (p.Name?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Brand?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (p.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
        }

        phones = sort?.ToLower() switch
        {
            "price_asc" => phones.OrderBy(p => p.DiscountedPrice).ToList(),
            "price_desc" => phones.OrderByDescending(p => p.DiscountedPrice).ToList(),
            "name_asc" => phones.OrderBy(p => p.Name).ToList(),
            "name_desc" => phones.OrderByDescending(p => p.Name).ToList(),
            "newest" => phones.OrderByDescending(p => p.CreatedAt).ToList(),
            "rating" => phones.OrderByDescending(p => p.Rating).ToList(),
            "popularity" => phones.OrderByDescending(p => p.TotalSold).ToList(),
            _ => phones.OrderByDescending(p => p.IsFeatured).ThenBy(p => p.Name).ToList()
        };

        var totalItems = phones.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        var pagedPhones = phones.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var viewMode = view ?? "grid";

        ViewData["TotalPages"] = totalPages;
        ViewData["CurrentPage"] = page;
        ViewData["TotalItems"] = totalItems;
        ViewData["CurrentBrand"] = brand;
        ViewData["CurrentCategory"] = category;
        ViewData["CurrentCondition"] = condition;
        ViewData["CurrentSort"] = sort;
        ViewData["Search"] = search;
        ViewData["ViewMode"] = viewMode;
        ViewData["MinPrice"] = minPrice;
        ViewData["MaxPrice"] = maxPrice;

        var gridHtml = await RenderViewToStringAsync("_ProductGrid", (pagedPhones, viewMode));
        var paginationHtml = await RenderViewToStringAsync("_Pagination", new {});

        var activeFilterCount = (brand != null ? 1 : 0) + (category != null ? 1 : 0) + (condition != null ? 1 : 0) + (minPrice != null || maxPrice != null ? 1 : 0);

        return Json(new {
            html = gridHtml,
            paginationHtml,
            totalItems,
            totalPages,
            activeFilterCount,
            currentPage = page
        });
    }

    [HttpGet]
    public IActionResult VariantInfo(int phoneId, string variantType, string variantValue)
    {
        var phone = _mockData.GetPhoneById(phoneId);
        if (phone == null) return NotFound();

        var variant = phone.Variants.FirstOrDefault(v =>
            string.Equals(v.Type, variantType, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(v.Value, variantValue, StringComparison.OrdinalIgnoreCase));

        var priceModifier = variant?.PriceModifier ?? 0;
        var basePrice = _pricing.CalculatePrice(phone);
        var adjustedPrice = basePrice + priceModifier;
        var discountedPrice = Math.Round(adjustedPrice - (adjustedPrice * phone.DiscountPercentage / 100), 2);

        string? imageUrl = null;
        if (string.Equals(variantType, "Color", StringComparison.OrdinalIgnoreCase))
        {
            imageUrl = phone.GalleryImages
                .FirstOrDefault(img => img.Contains(variantValue.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                ?? phone.GalleryImages.FirstOrDefault()
                ?? phone.ImageUrl;
        }

        return Json(new {
            price = Math.Round(adjustedPrice, 2),
            priceModifier,
            discountedPrice,
            discountPercent = phone.DiscountPercentage,
            imageUrl,
            basePrice = phone.Price
        });
    }

    private async Task<string> RenderViewToStringAsync(string viewName, object? model)
    {
        var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
        if (!viewResult.Success) return string.Empty;

        var viewData = new ViewDataDictionary(ViewData);
        if (model != null) viewData.Model = model;
        await using var sw = new System.IO.StringWriter();
        var viewContext = new ViewContext(ControllerContext, viewResult.View, viewData, TempData, sw, new HtmlHelperOptions());
        await viewResult.View.RenderAsync(viewContext);
        return sw.ToString();
    }
}
