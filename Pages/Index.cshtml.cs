using Microsoft.AspNetCore.Mvc.RazorPages;
using Website_Selling_Phones.Models;
using Website_Selling_Phones.Services;

namespace Website_Selling_Phones.Pages;

public class IndexModel : PageModel
{
    private readonly MockDataService _mockData;

    public IndexModel(MockDataService mockData)
    {
        _mockData = mockData;
    }

    public List<Phone> FeaturedPhones { get; set; } = new();
    public List<Phone> PopularPhones { get; set; } = new();
    public List<BrandInfo> TopBrands { get; set; } = new();

    public void OnGet()
    {
        FeaturedPhones = _mockData.GetAllPhones()
            .Where(p => p.IsFeatured)
            .OrderByDescending(p => p.Rating)
            .Take(6)
            .ToList();

        PopularPhones = _mockData.GetPopularPhones(6);

        TopBrands = _mockData.GetBrands()
            .Select(b => new BrandInfo { Name = b, Slug = b, Icon = BrandIconFor(b) })
            .ToList();
    }

    private static string BrandIconFor(string name) => name.ToLower() switch
    {
        "apple" => "bi-apple",
        "samsung" => "bi-phone-flip",
        "google" => "bi-google",
        "oneplus" => "bi-circle",
        "xiaomi" => "bi-grid",
        "nothing" => "bi-lightning-charge",
        "motorola" => "bi-phone",
        _ => "bi-phone"
    };
}

public class BrandInfo
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Icon { get; set; } = "bi-phone";
}
