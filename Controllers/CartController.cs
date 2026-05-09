using Microsoft.AspNetCore.Mvc;
using Website_Selling_Phones.Models;
using Website_Selling_Phones.Services;
using System.Text.Json;

namespace Website_Selling_Phones.Controllers;

public class CartController : Controller
{
    private readonly MockDataService _mockData;
    private const string CartSessionKey = "ShoppingCart";
    private const string CouponSessionKey = "AppliedCoupon";

    private static readonly List<Coupon> _coupons = new()
    {
        new() { Code = "SAVE10", DiscountPercent = 10, MaxUses = 1000 },
        new() { Code = "PHONE20", DiscountPercent = 20, MaxUses = 500, ValidUntil = DateTime.Now.AddMonths(3) },
        new() { Code = "WELCOME5", DiscountPercent = 5, MaxUses = 2000 },
        new() { Code = "FLASH15", DiscountPercent = 15, MaxUses = 100, ValidUntil = DateTime.Now.AddDays(7) }
    };

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
        foreach (var item in cart.Items)
            item.Phone = _mockData.GetPhoneById(item.PhoneId);

        return cart;
    }

    private void SaveCart(ShoppingCart cart)
    {
        var saveCart = new ShoppingCart { Items = cart.Items.Select(i => new CartItem { PhoneId = i.PhoneId, Quantity = i.Quantity }).ToList() };
        HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(saveCart));
    }

    private Coupon? GetAppliedCoupon()
    {
        var code = HttpContext.Session.GetString(CouponSessionKey);
        if (string.IsNullOrEmpty(code)) return null;
        return _coupons.FirstOrDefault(c => c.Code == code && c.IsValid);
    }

    public IActionResult Index()
    {
        var cart = GetCart();
        var coupon = GetAppliedCoupon();
        ViewData["Coupon"] = coupon;
        ViewData["CouponDiscount"] = coupon != null ? cart.Subtotal * coupon.DiscountPercent / 100 : 0;
        ViewData["Shipping"] = cart.Subtotal >= 500 ? 0 : 29.99m;
        ViewData["Tax"] = Math.Round(cart.Subtotal * 0.08m, 2);
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
    public IActionResult AddAndCheckout(int id, int quantity = 1, string? color = null, string? storage = null)
    {
        var phone = _mockData.GetPhoneById(id);
        if (phone == null) return NotFound();

        var cart = GetCart();
        var existing = cart.Items.FirstOrDefault(i => i.PhoneId == id);
        if (existing != null) existing.Quantity = quantity;
        else cart.AddItem(phone, quantity);

        SaveCart(cart);
        return Json(new { success = true });
    }

    [HttpPost]
    public IActionResult AddAjax(int id, int quantity = 1)
    {
        var phone = _mockData.GetPhoneById(id);
        if (phone == null) return NotFound();

        var cart = GetCart();
        cart.AddItem(phone, quantity);
        SaveCart(cart);

        return Json(new { success = true, totalItems = cart.TotalItems, totalPrice = cart.TotalPrice.ToString("F2"), itemName = phone.Name });
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
        TempData["CartMessage"] = "Item removed from cart.";
        return RedirectToAction("Index");
    }

    public IActionResult GetCount()
    {
        var cart = GetCart();
        return Json(new { count = cart.TotalItems });
    }

    [HttpPost]
    public IActionResult ApplyCoupon(string code)
    {
        var coupon = _coupons.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && c.IsValid);
        if (coupon == null)
        {
            TempData["CouponError"] = "Invalid or expired coupon code.";
            return RedirectToAction("Index");
        }

        HttpContext.Session.SetString(CouponSessionKey, coupon.Code);
        TempData["CouponSuccess"] = $"Coupon {coupon.Code} applied! {coupon.DiscountPercent}% off.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult RemoveCoupon()
    {
        HttpContext.Session.Remove(CouponSessionKey);
        TempData["CartMessage"] = "Coupon removed.";
        return RedirectToAction("Index");
    }

    public IActionResult ValidateCoupon(string code)
    {
        var coupon = _coupons.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && c.IsValid);
        return Json(coupon != null ? new { valid = true, discount = coupon.DiscountPercent } : new { valid = false });
    }
}
