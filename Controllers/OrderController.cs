using Microsoft.AspNetCore.Authorization;
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

    public OrderController(MockDataService mockData)
    {
        _mockData = mockData;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Checkout()
    {
        var cartJson = HttpContext.Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(cartJson))
            return RedirectToAction("Index", "Cart");

        var cart = JsonSerializer.Deserialize<ShoppingCart>(cartJson) ?? new ShoppingCart();

        foreach (var item in cart.Items)
            item.Phone = _mockData.GetPhoneById(item.PhoneId);

        if (!cart.Items.Any())
            return RedirectToAction("Index", "Cart");

        return View(cart);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder(string shippingAddress, string paymentMethod)
    {
        var cartJson = HttpContext.Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(cartJson))
            return RedirectToAction("Index", "Cart");

        var cart = JsonSerializer.Deserialize<ShoppingCart>(cartJson) ?? new ShoppingCart();

        foreach (var item in cart.Items)
            item.Phone = _mockData.GetPhoneById(item.PhoneId);

        var order = new Order
        {
            UserId = User.Identity?.Name ?? "anonymous",
            CustomerName = User.Identity?.Name ?? "Guest",
            CustomerEmail = User.Identity?.Name ?? "guest@email.com",
            ShippingAddress = shippingAddress,
            PaymentMethod = paymentMethod,
            TotalAmount = cart.TotalPrice,
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

        // Store order in session (would be DB in production)
        var ordersJson = HttpContext.Session.GetString(OrdersSessionKey);
        var orders = string.IsNullOrEmpty(ordersJson)
            ? new List<Order>()
            : JsonSerializer.Deserialize<List<Order>>(ordersJson) ?? new List<Order>();

        order.Id = orders.Count + 1;
        orders.Insert(0, order);
        HttpContext.Session.SetString(OrdersSessionKey, JsonSerializer.Serialize(orders));

        // Clear cart
        HttpContext.Session.Remove(CartSessionKey);

        TempData["OrderSuccess"] = "Order placed successfully!";
        return RedirectToAction("Confirmation", new { id = order.Id });
    }

    [Authorize]
    [HttpGet]
    public IActionResult Confirmation(int id)
    {
        var ordersJson = HttpContext.Session.GetString(OrdersSessionKey);
        var orders = string.IsNullOrEmpty(ordersJson)
            ? new List<Order>()
            : JsonSerializer.Deserialize<List<Order>>(ordersJson) ?? new List<Order>();

        var order = orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            return RedirectToAction("History");

        return View(order);
    }

    [Authorize]
    [HttpGet]
    public IActionResult History()
    {
        var ordersJson = HttpContext.Session.GetString(OrdersSessionKey);
        var orders = string.IsNullOrEmpty(ordersJson)
            ? new List<Order>()
            : JsonSerializer.Deserialize<List<Order>>(ordersJson) ?? new List<Order>();

        return View(orders);
    }
}