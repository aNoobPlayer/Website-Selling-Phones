using Microsoft.AspNetCore.SignalR;

namespace Website_Selling_Phones.Hubs;

public class InventoryHub : Hub
{
    public async Task SendLowStockAlert(string productName, int stockQuantity)
    {
        await Clients.All.SendAsync("ReceiveLowStockAlert", new
        {
            ProductName = productName,
            StockQuantity = stockQuantity,
            Timestamp = DateTime.Now,
            Message = $"⚠ Low stock alert: {productName} has only {stockQuantity} units remaining!"
        });
    }

    public async Task SendPriceUpdate(string productName, decimal newPrice)
    {
        await Clients.All.SendAsync("ReceivePriceUpdate", new
        {
            ProductName = productName,
            NewPrice = newPrice,
            Timestamp = DateTime.Now,
            Message = $"💰 Price update: {productName} is now ${newPrice:F2}!"
        });
    }

    public async Task SendNewArrival(string productName, string brand)
    {
        await Clients.All.SendAsync("ReceiveNewArrival", new
        {
            ProductName = productName,
            Brand = brand,
            Timestamp = DateTime.Now,
            Message = $"🆕 New arrival: {productName} by {brand}!"
        });
    }
}