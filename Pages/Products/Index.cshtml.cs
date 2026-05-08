using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Website_Selling_Phones.Data;
using Website_Selling_Phones.Models;

namespace Website_Selling_Phones.Pages.Products;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Product> Products { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task OnGetAsync()
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            query = query.Where(p => 
                p.Name.Contains(SearchTerm) || 
                p.Brand!.Contains(SearchTerm) ||
                p.Description!.Contains(SearchTerm));
        }

        Products = await query.OrderBy(p => p.Name).ToListAsync();
    }
}