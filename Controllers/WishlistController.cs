using Microsoft.AspNetCore.Mvc;
using Website_Selling_Phones.Models;
using Website_Selling_Phones.Services;
using System.Text.Json;

namespace Website_Selling_Phones.Controllers;

public class WishlistController : Controller
{
    private readonly MockDataService _mockData;
    private const string WishlistSessionKey = "UserWishlist";

    public WishlistController(MockDataService mockData)
    {
        _mockData = mockData;
    }

    private HashSet<int> GetWishlist()
    {
        var json = HttpContext.Session.GetString(WishlistSessionKey);
        return string.IsNullOrEmpty(json)
            ? new HashSet<int>()
            : JsonSerializer.Deserialize<HashSet<int>>(json) ?? new HashSet<int>();
    }

    private void SaveWishlist(HashSet<int> wishlist)
    {
        HttpContext.Session.SetString(WishlistSessionKey, JsonSerializer.Serialize(wishlist));
    }

    public IActionResult Index()
    {
        var ids = GetWishlist();
        var phones = ids.Select(id => _mockData.GetPhoneById(id)).Where(p => p != null).Cast<Phone>().ToList();
        return View(phones);
    }

    [HttpPost]
    public IActionResult Toggle(int id)
    {
        var wishlist = GetWishlist();
        bool added;
        if (wishlist.Contains(id))
        {
            wishlist.Remove(id);
            added = false;
        }
        else
        {
            wishlist.Add(id);
            added = true;
        }
        SaveWishlist(wishlist);
        return Json(new { success = true, added, count = wishlist.Count });
    }

    public IActionResult GetCount()
    {
        var wishlist = GetWishlist();
        return Json(new { count = wishlist.Count });
    }

    public IActionResult IsInWishlist(int id)
    {
        var wishlist = GetWishlist();
        return Json(new { inWishlist = wishlist.Contains(id) });
    }
}
