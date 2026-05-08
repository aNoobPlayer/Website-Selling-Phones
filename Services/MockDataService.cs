using Website_Selling_Phones.Models;

namespace Website_Selling_Phones.Services;

public class MockDataService
{
    private readonly List<Phone> _phones;

    public MockDataService()
    {
        _phones = new List<Phone>
        {
            new Phone
            {
                Id = 1, Name = "iPhone 15 Pro Max", Brand = "Apple", Model = "A2896",
                Price = 1199.99m, OriginalPrice = 1299.99m,
                Condition = PhoneCondition.New, DiscountPercentage = 8,
                Description = "The ultimate iPhone with A17 Pro chip, titanium design, 48MP camera, and USB-C connectivity.",
                StockQuantity = 15, ImageUrl = "https://via.placeholder.com/400x500/0f172a/ffffff?text=iPhone+15+Pro+Max",
                IsFeatured = true, Category = "Flagship", CreatedAt = DateTime.Now.AddDays(-5)
            },
            new Phone
            {
                Id = 2, Name = "Samsung Galaxy S24 Ultra", Brand = "Samsung", Model = "SM-S928B",
                Price = 1299.99m, Condition = PhoneCondition.New,
                Description = "Galaxy AI is here. 200MP camera, S Pen, and titanium frame for the ultimate Galaxy experience.",
                StockQuantity = 8, ImageUrl = "https://via.placeholder.com/400x500/1e3a5f/ffffff?text=Galaxy+S24+Ultra",
                IsFeatured = true, Category = "Flagship", CreatedAt = DateTime.Now.AddDays(-3)
            },
            new Phone
            {
                Id = 3, Name = "Google Pixel 8 Pro", Brand = "Google", Model = "GC3VE",
                Price = 899.99m, OriginalPrice = 999.99m,
                Condition = PhoneCondition.New, DiscountPercentage = 10,
                Description = "Google's most powerful Pixel with Tensor G3, best-in-class AI camera features, and 7 years of updates.",
                StockQuantity = 3, ImageUrl = "https://via.placeholder.com/400x500/ea4335/ffffff?text=Pixel+8+Pro",
                IsFeatured = true, Category = "Flagship", CreatedAt = DateTime.Now.AddDays(-10)
            },
            new Phone
            {
                Id = 4, Name = "iPhone 14 Pro", Brand = "Apple", Model = "A2890",
                Price = 749.99m, OriginalPrice = 999.99m,
                Condition = PhoneCondition.RefurbishedExcellent, DiscountPercentage = 25,
                Description = "Apple-certified refurbished. Dynamic Island, A16 Bionic, 48MP camera. Like new condition with full warranty.",
                StockQuantity = 5, ImageUrl = "https://via.placeholder.com/400x500/4a4a6a/ffffff?text=iPhone+14+Pro",
                IsFeatured = false, Category = "Refurbished", CreatedAt = DateTime.Now.AddDays(-15)
            },
            new Phone
            {
                Id = 5, Name = "Samsung Galaxy S23+", Brand = "Samsung", Model = "SM-S916B",
                Price = 549.99m, OriginalPrice = 849.99m,
                Condition = PhoneCondition.RefurbishedGood, DiscountPercentage = 35,
                Description = "Quality refurbished Galaxy S23+ with Snapdragon 8 Gen 2. Minor cosmetic wear, fully functional.",
                StockQuantity = 12, ImageUrl = "https://via.placeholder.com/400x500/2d4a2d/ffffff?text=Galaxy+S23%2B",
                IsFeatured = false, Category = "Refurbished", CreatedAt = DateTime.Now.AddDays(-20)
            },
            new Phone
            {
                Id = 6, Name = "OnePlus 12", Brand = "OnePlus", Model = "CPH2583",
                Price = 799.99m, Condition = PhoneCondition.New,
                Description = "Fast and smooth with Snapdragon 8 Gen 3, Hasselblad 4th Gen cameras, and 100W SUPERVOOC charging.",
                StockQuantity = 20, ImageUrl = "https://via.placeholder.com/400x500/f50514/ffffff?text=OnePlus+12",
                IsFeatured = false, Category = "Flagship", CreatedAt = DateTime.Now.AddDays(-7)
            },
            new Phone
            {
                Id = 7, Name = "Xiaomi 14 Ultra", Brand = "Xiaomi", Model = "24030PN60G",
                Price = 999.99m, OriginalPrice = 1099.99m,
                Condition = PhoneCondition.New, DiscountPercentage = 9,
                Description = "Leica professional optics, Snapdragon 8 Gen 3, 1-inch sensor for unmatched mobile photography.",
                StockQuantity = 2, ImageUrl = "https://via.placeholder.com/400x500/ff6900/ffffff?text=Xiaomi+14+Ultra",
                IsFeatured = false, Category = "Flagship", CreatedAt = DateTime.Now.AddDays(-2)
            },
            new Phone
            {
                Id = 8, Name = "iPhone SE (2022)", Brand = "Apple", Model = "A2783",
                Price = 299.99m, OriginalPrice = 429.99m,
                Condition = PhoneCondition.UsedLikeNew, DiscountPercentage = 30,
                Description = "Pre-owned iPhone SE with A15 Bionic chip. Like new condition with minimal signs of use.",
                StockQuantity = 18, ImageUrl = "https://via.placeholder.com/400x500/666666/ffffff?text=iPhone+SE",
                IsFeatured = false, Category = "Budget", CreatedAt = DateTime.Now.AddDays(-30)
            },
            new Phone
            {
                Id = 9, Name = "Nothing Phone (2)", Brand = "Nothing", Model = "A065",
                Price = 499.99m, OriginalPrice = 649.99m,
                Condition = PhoneCondition.New, DiscountPercentage = 23,
                Description = "Iconic Glyph Interface, Snapdragon 8+ Gen 1, 50MP dual camera. Design meets innovation.",
                StockQuantity = 25, ImageUrl = "https://via.placeholder.com/400x500/1a1a1a/ffffff?text=Nothing+Phone+2",
                IsFeatured = true, Category = "Mid-Range", CreatedAt = DateTime.Now.AddDays(-1)
            },
            new Phone
            {
                Id = 10, Name = "Motorola Edge 40", Brand = "Motorola", Model = "XT2303",
                Price = 349.99m, OriginalPrice = 499.99m,
                Condition = PhoneCondition.UsedFair, DiscountPercentage = 30,
                Description = "Used Motorola Edge 40. Fair condition with visible signs of wear. Fully functional, 6 months warranty.",
                StockQuantity = 7, ImageUrl = "https://via.placeholder.com/400x500/5a5a8a/ffffff?text=Motorola+Edge+40",
                IsFeatured = false, Category = "Budget", CreatedAt = DateTime.Now.AddDays(-45)
            }
        };
    }

    public List<Phone> GetAllPhones() => _phones;

    public Phone? GetPhoneById(int id) => _phones.FirstOrDefault(p => p.Id == id);

    public List<Phone> FilterPhones(string? brand, decimal? minPrice, decimal? maxPrice, string? category, string? condition)
    {
        var query = _phones.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(p => p.Brand != null && p.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category != null && p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(condition))
            query = query.Where(p => p.ConditionDisplayName.Equals(condition, StringComparison.OrdinalIgnoreCase));

        return query.ToList();
    }

    public List<string> GetCategories() => _phones
        .Select(p => p.Category)
        .Where(c => c != null)
        .Distinct()
        .Cast<string>()
        .OrderBy(c => c)
        .ToList();

    public List<string> GetBrands() => _phones
        .Select(p => p.Brand)
        .Where(b => b != null)
        .Distinct()
        .Cast<string>()
        .OrderBy(b => b)
        .ToList();
}