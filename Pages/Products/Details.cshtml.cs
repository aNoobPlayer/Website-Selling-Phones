using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Website_Selling_Phones.Data;
using Website_Selling_Phones.Models;

namespace Website_Selling_Phones.Pages.Products;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Product? Product { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (Product == null)
        {
            return NotFound();
        }

        return Page();
    }
}