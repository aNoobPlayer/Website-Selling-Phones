using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Website_Selling_Phones.Models;
using Website_Selling_Phones.Services;

namespace Website_Selling_Phones.Pages.Admin;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly MockDataService _mockData;

    public IndexModel(MockDataService mockData)
    {
        _mockData = mockData;
    }

    public List<Phone> Products { get; set; } = new();
    public int TotalProducts { get; set; }
    public int InStockProducts { get; set; }
    public int FeaturedProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int OutOfStockProducts { get; set; }
    public decimal TotalValue { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }

    [BindProperty]
    public Phone Product { get; set; } = new();

    public void OnGet()
    {
        Products = _mockData.GetAllPhones();
        TotalProducts = Products.Count;
        InStockProducts = Products.Count(p => p.StockQuantity > 5);
        LowStockProducts = Products.Count(p => p.StockQuantity > 0 && p.StockQuantity <= 5);
        OutOfStockProducts = Products.Count(p => p.StockQuantity == 0);
        FeaturedProducts = Products.Count(p => p.IsFeatured);
        TotalValue = Products.Sum(p => p.Price * p.StockQuantity);
        TotalRevenue = Products.Sum(p => p.TotalSold * p.Price) * 0.8m; // estimated revenue
        TotalOrders = Products.Sum(p => p.TotalSold);

        ViewData["TotalProducts"] = TotalProducts;
        ViewData["FeaturedProducts"] = FeaturedProducts;
    }

    public IActionResult OnPostDelete(int id)
    {
        // In production, this would remove from DB
        Products = _mockData.GetAllPhones();
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            Products.Remove(product);
        }
        return RedirectToPage();
    }

    public IActionResult OnPostBulkAction(string action, int[] selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any()) return RedirectToPage();
        Products = _mockData.GetAllPhones();
        switch (action)
        {
            case "delete":
                Products.RemoveAll(p => selectedIds.Contains(p.Id));
                break;
            case "feature":
                Products.Where(p => selectedIds.Contains(p.Id)).ToList().ForEach(p => p.IsFeatured = true);
                break;
            case "unfeature":
                Products.Where(p => selectedIds.Contains(p.Id)).ToList().ForEach(p => p.IsFeatured = false);
                break;
        }
        return RedirectToPage();
    }
}
