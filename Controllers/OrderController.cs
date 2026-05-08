using Microsoft.AspNetCore.Mvc;
using Website_Selling_Phones.Models;
using Website_Selling_Phones.Services;
using System.Text.Json;

namespace Website_Selling_Phones.Controllers;

public class OrderController : Controller
{
    private readonly MockDataService _mockData;
    private const string CartSessionKey = "ShoppingCart";
    private const string OrdersSessionKey = "Orders";
    private const string CouponSessionKey = "AppliedCoupon";

    private static readonly List<Coupon> _coupons = new()
    {
        new() { Code = "SAVE10", DiscountPercent = 10 },
        new() { Code = "PHONE20", DiscountPercent = 20 },
        new() { Code = "WELCOME5", DiscountPercent = 5 },
        new() { Code = "FLASH15", DiscountPercent = 15, ValidUntil = DateTime.Now.AddDays(7) }
    };

    public OrderController(MockDataService mockData)
    {
        _mockData = mockData;
    }

    [HttpGet]
    public IActionResult Checkout(int step = 1)
    {
        var cart = GetCart();
        if (!cart.Items.Any())
            return RedirectToAction("Index", "Cart");

        var coupon = GetAppliedCoupon();
        var couponDiscount = coupon != null ? cart.Subtotal * coupon.DiscountPercent / 100 : 0;

        ViewData["Step"] = step;
        ViewData["Coupon"] = coupon;
        ViewData["CouponDiscount"] = couponDiscount;
        ViewData["Shipping"] = cart.Subtotal >= 500 ? 0 : 29.99m;
        ViewData["ShippingOptions"] = new List<ShippingOption>
        {
            new() { Name = "Standard (5-7 days)", Price = cart.Subtotal >= 500 ? 0 : 29.99m },
            new() { Name = "Express (2-3 days)", Price = 49.99m },
            new() { Name = "Overnight (Next day)", Price = 79.99m }
        };
        ViewData["Tax"] = Math.Round(cart.Subtotal * 0.08m, 2);

        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder(string shippingAddress, string paymentMethod, string shippingMethod, string customerName, string customerEmail)
    {
        var cart = GetCart();
        if (!cart.Items.Any())
            return RedirectToAction("Index", "Cart");

        var name = string.IsNullOrWhiteSpace(customerName) ? (User.Identity?.Name ?? "Guest") : customerName;
        var email = string.IsNullOrWhiteSpace(customerEmail) ? (User.Identity?.Name ?? "guest@email.com") : customerEmail;

        var coupon = GetAppliedCoupon();
        var subtotal = cart.TotalPrice;
        var couponDiscount = coupon != null ? subtotal * coupon.DiscountPercent / 100 : 0;
        var shipping = ParseShippingCost(shippingMethod);
        var tax = Math.Round((subtotal - couponDiscount) * 0.08m, 2);
        var total = subtotal - couponDiscount + shipping + tax;

        var order = new Order
        {
            UserId = User.Identity?.Name ?? "anonymous",
            CustomerName = name,
            CustomerEmail = email,
            ShippingAddress = shippingAddress,
            PaymentMethod = paymentMethod,
            TotalAmount = total,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Confirmed,
            OrderItems = cart.Items.Select(i => new OrderItem
            {
                PhoneId = i.PhoneId,
                PhoneName = i.Phone?.Name ?? "Unknown",
                Price = i.Phone?.DiscountedPrice ?? 0,
                Quantity = i.Quantity
            }).ToList()
        };

        var ordersJson = HttpContext.Session.GetString(OrdersSessionKey);
        var orders = string.IsNullOrEmpty(ordersJson)
            ? new List<Order>()
            : JsonSerializer.Deserialize<List<Order>>(ordersJson) ?? new List<Order>();

        order.Id = orders.Count + 1;
        orders.Insert(0, order);
        HttpContext.Session.SetString(OrdersSessionKey, JsonSerializer.Serialize(orders));

        HttpContext.Session.Remove(CartSessionKey);
        HttpContext.Session.Remove(CouponSessionKey);

        TempData["OrderSuccess"] = "Order placed successfully!";
        return RedirectToAction("Confirmation", new { id = order.Id });
    }

    [HttpGet]
    public IActionResult Confirmation(int id)
    {
        var ordersJson = HttpContext.Session.GetString(OrdersSessionKey);
        var orders = string.IsNullOrEmpty(ordersJson)
            ? new List<Order>()
            : JsonSerializer.Deserialize<List<Order>>(ordersJson) ?? new List<Order>();

        var order = orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return RedirectToAction("History");

        return View(order);
    }

    [HttpGet]
    public IActionResult History()
    {
        var ordersJson = HttpContext.Session.GetString(OrdersSessionKey);
        var orders = string.IsNullOrEmpty(ordersJson)
            ? new List<Order>()
            : JsonSerializer.Deserialize<List<Order>>(ordersJson) ?? new List<Order>();

        return View(orders);
    }

    private ShoppingCart GetCart()
    {
        var cartJson = HttpContext.Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(cartJson)) return new ShoppingCart();
        var cart = JsonSerializer.Deserialize<ShoppingCart>(cartJson) ?? new ShoppingCart();
        foreach (var item in cart.Items)
            item.Phone = _mockData.GetPhoneById(item.PhoneId);
        return cart;
    }

    private Coupon? GetAppliedCoupon()
    {
        var code = HttpContext.Session.GetString(CouponSessionKey);
        if (string.IsNullOrEmpty(code)) return null;
        return _coupons.FirstOrDefault(c => c.Code == code && c.IsValid);
    }

    private static decimal ParseShippingCost(string? method) => method switch
    {
        "Express" => 49.99m,
        "Overnight" => 79.99m,
        _ => 0m
    };
}
