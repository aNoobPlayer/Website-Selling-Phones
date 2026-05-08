using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website_Selling_Phones.Models;

public class CartItem
{
    public int Id { get; set; }
    public int PhoneId { get; set; }
    public int Quantity { get; set; } = 1;

    [ForeignKey("PhoneId")]
    public Phone? Phone { get; set; }
}

public class ShoppingCart
{
    public List<CartItem> Items { get; set; } = new();

    public decimal TotalPrice => Items.Sum(i => (i.Phone?.DiscountedPrice ?? 0) * i.Quantity);
    public decimal Subtotal => TotalPrice;
    public int TotalItems => Items.Sum(i => i.Quantity);

    public void AddItem(Phone phone, int quantity = 1)
    {
        var existing = Items.FirstOrDefault(i => i.PhoneId == phone.Id);
        if (existing != null)
            existing.Quantity += quantity;
        else
            Items.Add(new CartItem { PhoneId = phone.Id, Quantity = quantity, Phone = phone });
    }

    public void RemoveItem(int phoneId)
    {
        Items.RemoveAll(i => i.PhoneId == phoneId);
    }

    public void UpdateQuantity(int phoneId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.PhoneId == phoneId);
        if (item != null)
            item.Quantity = quantity;
    }

    public void Clear() => Items.Clear();
}