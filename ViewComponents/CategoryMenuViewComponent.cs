using Microsoft.AspNetCore.Mvc;
using Website_Selling_Phones.Services;
using Website_Selling_Phones.Models;

namespace Website_Selling_Phones.ViewComponents;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly MockDataService _mockData;

    public CategoryMenuViewComponent(MockDataService mockData)
    {
        _mockData = mockData;
    }

    public IViewComponentResult Invoke(string? currentCategory = null)
    {
        ViewData["CurrentCategory"] = currentCategory;
        var categories = _mockData.GetCategories();
        var brands = _mockData.GetBrands();
        var allPhones = _mockData.GetAllPhones();

        var model = new CategoryMenuViewModel
        {
            Categories = categories,
            Brands = brands,
            TotalProducts = allPhones.Count,
            NewProducts = allPhones.Count(p => p.Condition == PhoneCondition.New),
            RefurbishedProducts = allPhones.Count(p => p.ConditionDisplayName == "Refurbished"),
            UsedProducts = allPhones.Count(p => p.ConditionDisplayName == "Used"),
            OnSaleCount = allPhones.Count(p => p.DiscountPercentage > 0)
        };

        return View(model);
    }
}

public class CategoryMenuViewModel
{
    public List<string> Categories { get; set; } = new();
    public List<string> Brands { get; set; } = new();
    public int TotalProducts { get; set; }
    public int NewProducts { get; set; }
    public int RefurbishedProducts { get; set; }
    public int UsedProducts { get; set; }
    public int OnSaleCount { get; set; }
}