using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Website_Selling_Phones.Data;
using Website_Selling_Phones.Models;

namespace Website_Selling_Phones.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Product> FeaturedProducts { get; set; } = new();

    public async Task OnGetAsync()
    {
        FeaturedProducts = await _context.Products
            .Where(p => p.IsFeatured)
            .Take(6)
            .ToListAsync();
    }
}