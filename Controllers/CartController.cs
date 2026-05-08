using Microsoft.AspNetCore.Mvc;
using Website_Selling_Phones.Models;
using Website_Selling_Phones.Services;
using System.Text.Json;

namespace Website_Selling_Phones.Controllers;

public class CartController : Controller
{
    private readonly MockDataService _mockData;
    private const string CartSessionKey = "ShoppingCart";

    public CartController(MockDataService mockData)
    {
        _mockData = mockData;
    }

    private ShoppingCart GetCart()
    {
        var cartJson = HttpContext.Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(cartJson))
            return new ShoppingCart();

        var cart = JsonSerializer.Deserialize<ShoppingCart>(cartJson) ?? new ShoppingCart();

        // Re-hydrate Phone references from mock data
        foreach (var item in cart.Items)
            item.Phone = _mockData.GetPhoneById(item.PhoneId);

        return cart;
    }

    private void SaveCart(ShoppingCart cart)
    {
        // Strip Phone references before serializing
        var saveCart = new ShoppingCart { Items = cart.Items.Select(i => new CartItem { PhoneId = i.PhoneId, Quantity = i.Quantity }).ToList() };
        HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(saveCart));
    }

    public IActionResult Index()
    {
        var cart = GetCart();
        return View(cart);
    }

    [HttpPost]
    public IActionResult Add(int id, int quantity = 1)
    {
        var phone = _mockData.GetPhoneById(id);
        if (phone == null) return NotFound();

        var cart = GetCart();
        cart.AddItem(phone, quantity);
        SaveCart(cart);

        TempData["CartMessage"] = $"{phone.Name} added to cart!";
        return RedirectToAction("Index", "Product");
    }

    [HttpPost]
    public IActionResult AddAjax(int id, int quantity = 1)
    {
        var phone = _mockData.GetPhoneById(id);
        if (phone == null) return NotFound();

        var cart = GetCart();
        cart.AddItem(phone, quantity);
        SaveCart(cart);

        return Json(new { success = true, totalItems = cart.TotalItems, totalPrice = cart.TotalPrice.ToString("F2") });
    }

    [HttpPost]
    public IActionResult Update(int id, int quantity)
    {
        var cart = GetCart();
        cart.UpdateQuantity(id, quantity);
        SaveCart(cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remove(int id)
    {
        var cart = GetCart();
        cart.RemoveItem(id);
        SaveCart(cart);
        return RedirectToAction("Index");
    }

    public IActionResult GetCount()
    {
        var cart = GetCart();
        return Json(new { count = cart.TotalItems });
    }
}