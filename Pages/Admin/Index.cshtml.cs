using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Website_Selling_Phones.Data;
using Website_Selling_Phones.Models;

namespace Website_Selling_Phones.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Product> Products { get; set; } = new();
    public int TotalProducts { get; set; }
    public int InStockProducts { get; set; }
    public int FeaturedProducts { get; set; }
    public decimal TotalValue { get; set; }

    public async Task OnGetAsync()
    {
        Products = await _context.Products.OrderBy(p => p.Id).ToListAsync();
        TotalProducts = Products.Count;
        InStockProducts = Products.Count(p => p.StockQuantity > 0);
        FeaturedProducts = Products.Count(p => p.IsFeatured);
        TotalValue = Products.Sum(p => p.Price * p.StockQuantity);

        ViewData["TotalProducts"] = TotalProducts;
        ViewData["FeaturedProducts"] = FeaturedProducts;
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}