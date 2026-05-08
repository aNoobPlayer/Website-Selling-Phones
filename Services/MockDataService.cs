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
                FullDescription = "iPhone 15 Pro Max is the most powerful iPhone ever. With the A17 Pro chip — the industry's first 3-nanometer chip — you get groundbreaking performance and efficiency. The titanium design is both strong and lightweight. The 48MP Main camera captures super-high-resolution photos with incredible detail. And with USB-C, you can charge your iPhone, Mac, and iPad with the same cable.",
                StockQuantity = 15,
                ImageUrl = "https://via.placeholder.com/600x750/0f172a/ffffff?text=iPhone+15+Pro+Max",
                GalleryImages = new List<string> {
                    "https://via.placeholder.com/600x750/0f172a/ffffff?text=iPhone+15+Pro+Max",
                    "https://via.placeholder.com/600x750/1a2744/ffffff?text=Side+View",
                    "https://via.placeholder.com/600x750/253553/ffffff?text=Back+View",
                    "https://via.placeholder.com/600x750/2d4268/ffffff?text=Camera+Closeup"
                },
                Specs = new PhoneSpecs { Display = "6.7\" Super Retina XDR OLED", Processor = "A17 Pro (3nm)", Ram = "8GB", Storage = "256GB", Camera = "48MP Main + 12MP UW + 12MP 5x Tele", Battery = "4441mAh", Os = "iOS 17", Weight = "221g", Connectivity = "5G, Wi-Fi 6E, USB-C" },
                Variants = new List<ProductVariant> { new() { Type = "Color", Value = "Natural Titanium" }, new() { Type = "Color", Value = "Blue Titanium" }, new() { Type = "Color", Value = "White Titanium" }, new() { Type = "Color", Value = "Black Titanium" }, new() { Type = "Storage", Value = "256GB" }, new() { Type = "Storage", Value = "512GB", PriceModifier = 200 }, new() { Type = "Storage", Value = "1TB", PriceModifier = 400 } },
                Rating = 4.8, ReviewCount = 234,
                TotalSold = 1523, IsFeatured = true, Category = "Flagship", CreatedAt = DateTime.Now.AddDays(-5)
            },
            new Phone
            {
                Id = 2, Name = "Samsung Galaxy S24 Ultra", Brand = "Samsung", Model = "SM-S928B",
                Price = 1299.99m, Condition = PhoneCondition.New,
                Description = "Galaxy AI is here. 200MP camera, S Pen, and titanium frame for the ultimate Galaxy experience.",
                FullDescription = "Meet Galaxy S24 Ultra, the ultimate Galaxy experience. With the power of Galaxy AI, you can edit photos, translate calls, and search like never before. The 200MP camera captures stunning detail, while the built-in S Pen gives you precision control. The titanium frame is both durable and lightweight, and the 6.8\" Dynamic AMOLED 2X display brings everything to life with vibrant colors and smooth 120Hz refresh rate.",
                StockQuantity = 8,
                ImageUrl = "https://via.placeholder.com/600x750/1e3a5f/ffffff?text=Galaxy+S24+Ultra",
                GalleryImages = new List<string> {
                    "https://via.placeholder.com/600x750/1e3a5f/ffffff?text=Galaxy+S24+Ultra",
                    "https://via.placeholder.com/600x750/274b7c/ffffff?text=Side+View",
                    "https://via.placeholder.com/600x750/305d9a/ffffff?text=Back+View",
                    "https://via.placeholder.com/600x750/3a6fb8/ffffff?text=SPen+View"
                },
                Specs = new PhoneSpecs { Display = "6.8\" Dynamic AMOLED 2X 120Hz", Processor = "Snapdragon 8 Gen 3", Ram = "12GB", Storage = "256GB", Camera = "200MP Main + 12MP UW + 50MP 5x + 10MP 3x", Battery = "5000mAh", Os = "Android 14, One UI 6.1", Weight = "232g", Connectivity = "5G, Wi-Fi 7, Bluetooth 5.3" },
                Variants = new List<ProductVariant> { new() { Type = "Color", Value = "Titanium Gray" }, new() { Type = "Color", Value = "Titanium Black" }, new() { Type = "Color", Value = "Titanium Violet" }, new() { Type = "Storage", Value = "256GB" }, new() { Type = "Storage", Value = "512GB", PriceModifier = 120 }, new() { Type = "Storage", Value = "1TB", PriceModifier = 360 } },
                Rating = 4.7, ReviewCount = 189,
                TotalSold = 1247, IsFeatured = true, Category = "Flagship", CreatedAt = DateTime.Now.AddDays(-3)
            },
            new Phone
            {
                Id = 3, Name = "Google Pixel 8 Pro", Brand = "Google", Model = "GC3VE",
                Price = 899.99m, OriginalPrice = 999.99m,
                Condition = PhoneCondition.New, DiscountPercentage = 10,
                Description = "Google's most powerful Pixel with Tensor G3, best-in-class AI camera features, and 7 years of updates.",
                FullDescription = "Pixel 8 Pro is the most helpful and personal phone. Powered by Tensor G3, it's built with Google AI for cutting-edge photo and video features. The 6.7\" Super Actua display is Pixel's brightest yet. With 50MP main, 48MP telephoto, and 48MP ultrawide cameras, every shot is stunning. And with 7 years of OS, security, and Feature Drop updates, it gets better over time.",
                StockQuantity = 3,
                ImageUrl = "https://via.placeholder.com/600x750/ea4335/ffffff?text=Pixel+8+Pro",
                GalleryImages = new List<string> {
                    "https://via.placeholder.com/600x750/ea4335/ffffff?text=Pixel+8+Pro",
                    "https://via.placeholder.com/600x750/d33828/ffffff?text=Side+View",
                    "https://via.placeholder.com/600x750/bd2e1c/ffffff?text=Back+View"
                },
                Specs = new PhoneSpecs { Display = "6.7\" Super Actua LTPO OLED 120Hz", Processor = "Google Tensor G3", Ram = "12GB", Storage = "128GB", Camera = "50MP Main + 48MP UW + 48MP 5x Tele", Battery = "5050mAh", Os = "Android 14", Weight = "213g", Connectivity = "5G, Wi-Fi 7, Bluetooth 5.3" },
                Variants = new List<ProductVariant> { new() { Type = "Color", Value = "Obsidian" }, new() { Type = "Color", Value = "Porcelain" }, new() { Type = "Color", Value = "Bay" }, new() { Type = "Storage", Value = "128GB" }, new() { Type = "Storage", Value = "256GB", PriceModifier = 60 } },
                Rating = 4.6, ReviewCount = 156,
                TotalSold = 892, IsFeatured = true, Category = "Flagship", CreatedAt = DateTime.Now.AddDays(-10)
            },
            new Phone
            {
                Id = 4, Name = "iPhone 14 Pro", Brand = "Apple", Model = "A2890",
                Price = 749.99m, OriginalPrice = 999.99m,
                Condition = PhoneCondition.RefurbishedExcellent, DiscountPercentage = 25,
                Description = "Apple-certified refurbished. Dynamic Island, A16 Bionic, 48MP camera. Like new condition with full warranty.",
                FullDescription = "This Apple-certified refurbished iPhone 14 Pro has undergone rigorous testing and comes with a new battery and outer shell. The Dynamic Island bubbles up alerts and live activities. The A16 Bionic chip powers advanced camera features and smooth performance. With the 48MP Main camera, you can capture incredible detail. Includes Apple's standard 1-year warranty.",
                StockQuantity = 5,
                ImageUrl = "https://via.placeholder.com/600x750/4a4a6a/ffffff?text=iPhone+14+Pro",
                GalleryImages = new List<string> {
                    "https://via.placeholder.com/600x750/4a4a6a/ffffff?text=iPhone+14+Pro",
                    "https://via.placeholder.com/600x750/5a5a7d/ffffff?text=Side+View",
                    "https://via.placeholder.com/600x750/6a6a90/ffffff?text=Back+View"
                },
                Specs = new PhoneSpecs { Display = "6.1\" Super Retina XDR OLED", Processor = "A16 Bionic (4nm)", Ram = "6GB", Storage = "128GB", Camera = "48MP Main + 12MP UW + 12MP 3x Tele", Battery = "3200mAh", Os = "iOS 17", Weight = "206g", Connectivity = "5G, Wi-Fi 6, Lightning" },
                Variants = new List<ProductVariant> { new() { Type = "Color", Value = "Deep Purple" }, new() { Type = "Color", Value = "Space Black" }, new() { Type = "Storage", Value = "128GB" }, new() { Type = "Storage", Value = "256GB", PriceModifier = 50 } },
                Rating = 4.5, ReviewCount = 312,
                TotalSold = 2156, IsFeatured = false, Category = "Refurbished", CreatedAt = DateTime.Now.AddDays(-15)
            },
            new Phone
            {
                Id = 5, Name = "Samsung Galaxy S23+", Brand = "Samsung", Model = "SM-S916B",
                Price = 549.99m, OriginalPrice = 849.99m,
                Condition = PhoneCondition.RefurbishedGood, DiscountPercentage = 35,
                Description = "Quality refurbished Galaxy S23+ with Snapdragon 8 Gen 2. Minor cosmetic wear, fully functional.",
                StockQuantity = 12,
                ImageUrl = "https://via.placeholder.com/600x750/2d4a2d/ffffff?text=Galaxy+S23%2B",
                GalleryImages = new List<string> {
                    "https://via.placeholder.com/600x750/2d4a2d/ffffff?text=Galaxy+S23%2B",
                    "https://via.placeholder.com/600x750/3a5a3a/ffffff?text=Side+View"
                },
                Specs = new PhoneSpecs { Display = "6.6\" Dynamic AMOLED 2X 120Hz", Processor = "Snapdragon 8 Gen 2", Ram = "8GB", Storage = "256GB", Camera = "50MP Main + 12MP UW + 10MP 3x Tele", Battery = "4700mAh", Os = "Android 14, One UI 6.1", Weight = "195g", Connectivity = "5G, Wi-Fi 6E, Bluetooth 5.3" },
                Variants = new List<ProductVariant> { new() { Type = "Color", Value = "Phantom Black" }, new() { Type = "Color", Value = "Lavender" } },
                Rating = 4.4, ReviewCount = 198,
                TotalSold = 1876, IsFeatured = false, Category = "Refurbished", CreatedAt = DateTime.Now.AddDays(-20)
            },
            new Phone
            {
                Id = 6, Name = "OnePlus 12", Brand = "OnePlus", Model = "CPH2583",
                Price = 799.99m, Condition = PhoneCondition.New,
                Description = "Fast and smooth with Snapdragon 8 Gen 3, Hasselblad 4th Gen cameras, and 100W SUPERVOOC charging.",
                FullDescription = "The OnePlus 12 combines raw power with refined elegance. Powered by the Snapdragon 8 Gen 3 and up to 16GB RAM, every app and game runs buttery smooth. The 4th Gen Hasselblad Camera System delivers true-to-life colors and professional-grade portraits. With 100W SUPERVOOC charging, you get a day's power in just 26 minutes. The 6.82\" 2K ProXDR display with 120Hz refresh rate makes everything look stunning.",
                StockQuantity = 20,
                ImageUrl = "https://via.placeholder.com/600x750/f50514/ffffff?text=OnePlus+12",
                GalleryImages = new List<string> {
                    "https://via.placeholder.com/600x750/f50514/ffffff?text=OnePlus+12",
                    "https://via.placeholder.com/600x750/d40410/ffffff?text=Side+View",
                    "https://via.placeholder.com/600x750/b3030c/ffffff?text=Back+View"
                },
                Specs = new PhoneSpecs { Display = "6.82\" 2K ProXDR AMOLED 120Hz", Processor = "Snapdragon 8 Gen 3", Ram = "16GB", Storage = "256GB", Camera = "50MP Main + 48MP UW + 64MP 3x Tele (Hasselblad)", Battery = "5400mAh, 100W wired", Os = "Android 14, OxygenOS 14", Weight = "220g", Connectivity = "5G, Wi-Fi 7, Bluetooth 5.4" },
                Variants = new List<ProductVariant> { new() { Type = "Color", Value = "Flowy Emerald" }, new() { Type = "Color", Value = "Silky Black" } },
                Rating = 4.5, ReviewCount = 87,
                TotalSold = 624, IsFeatured = false, Category = "Flagship", CreatedAt = DateTime.Now.AddDays(-7)
            },
            new Phone
            {
                Id = 7, Name = "Xiaomi 14 Ultra", Brand = "Xiaomi", Model = "24030PN60G",
                Price = 999.99m, OriginalPrice = 1099.99m,
                Condition = PhoneCondition.New, DiscountPercentage = 9,
                Description = "Leica professional optics, Snapdragon 8 Gen 3, 1-inch sensor for unmatched mobile photography.",
                StockQuantity = 2,
                ImageUrl = "https://via.placeholder.com/600x750/ff6900/ffffff?text=Xiaomi+14+Ultra",
                GalleryImages = new List<string> {
                    "https://via.placeholder.com/600x750/ff6900/ffffff?text=Xiaomi+14+Ultra",
                    "https://via.placeholder.com/600x750/e55a00/ffffff?text=Side+View"
                },
                Specs = new PhoneSpecs { Display = "6.73\" LTPO AMOLED 120Hz", Processor = "Snapdragon 8 Gen 3", Ram = "16GB", Storage = "512GB", Camera = "50MP 1-inch Main + 50MP UW + 50MP 3.2x + 50MP 5x (Leica)", Battery = "5300mAh, 90W wired", Os = "Android 14, HyperOS", Weight = "219g", Connectivity = "5G, Wi-Fi 7, Bluetooth 5.4" },
                Variants = new List<ProductVariant> { new() { Type = "Color", Value = "Black" }, new() { Type = "Color", Value = "White" } },
                Rating = 4.6, ReviewCount = 43,
                TotalSold = 312, IsFeatured = false, Category = "Flagship", CreatedAt = DateTime.Now.AddDays(-2)
            },
            new Phone
            {
                Id = 8, Name = "iPhone SE (2022)", Brand = "Apple", Model = "A2783",
                Price = 299.99m, OriginalPrice = 429.99m,
                Condition = PhoneCondition.UsedLikeNew, DiscountPercentage = 30,
                Description = "Pre-owned iPhone SE with A15 Bionic chip. Like new condition with minimal signs of use.",
                StockQuantity = 18,
                ImageUrl = "https://via.placeholder.com/600x750/666666/ffffff?text=iPhone+SE",
                GalleryImages = new List<string> {
                    "https://via.placeholder.com/600x750/666666/ffffff?text=iPhone+SE",
                    "https://via.placeholder.com/600x750/777777/ffffff?text=Back+View"
                },
                Specs = new PhoneSpecs { Display = "4.7\" Retina HD LCD", Processor = "A15 Bionic (5nm)", Ram = "4GB", Storage = "64GB", Camera = "12MP Main", Battery = "2018mAh", Os = "iOS 17", Weight = "144g", Connectivity = "5G, Wi-Fi 6, Lightning" },
                Variants = new List<ProductVariant> { new() { Type = "Color", Value = "Midnight" }, new() { Type = "Color", Value = "Starlight" }, new() { Type = "Color", Value = "Red" }, new() { Type = "Storage", Value = "64GB" }, new() { Type = "Storage", Value = "128GB", PriceModifier = 50 } },
                Rating = 4.3, ReviewCount = 521,
                TotalSold = 3421, IsFeatured = false, Category = "Budget", CreatedAt = DateTime.Now.AddDays(-30)
            },
            new Phone
            {
                Id = 9, Name = "Nothing Phone (2)", Brand = "Nothing", Model = "A065",
                Price = 499.99m, OriginalPrice = 649.99m,
                Condition = PhoneCondition.New, DiscountPercentage = 23,
                Description = "Iconic Glyph Interface, Snapdragon 8+ Gen 1, 50MP dual camera. Design meets innovation.",
                StockQuantity = 25,
                ImageUrl = "https://via.placeholder.com/600x750/1a1a1a/ffffff?text=Nothing+Phone+2",
                GalleryImages = new List<string> {
                    "https://via.placeholder.com/600x750/1a1a1a/ffffff?text=Nothing+Phone+2",
                    "https://via.placeholder.com/600x750/2a2a2a/ffffff?text=Side+View",
                    "https://via.placeholder.com/600x750/3a3a3a/ffffff?text=Glyph+View"
                },
                Specs = new PhoneSpecs { Display = "6.7\" LTPO OLED 120Hz", Processor = "Snapdragon 8+ Gen 1", Ram = "12GB", Storage = "256GB", Camera = "50MP Main + 50MP UW", Battery = "4700mAh, 45W wired", Os = "Android 14, Nothing OS 2.5", Weight = "201g", Connectivity = "5G, Wi-Fi 6, Bluetooth 5.3" },
                Variants = new List<ProductVariant> { new() { Type = "Color", Value = "Dark Gray" }, new() { Type = "Color", Value = "White" } },
                Rating = 4.4, ReviewCount = 178,
                TotalSold = 892, IsFeatured = true, Category = "Mid-Range", CreatedAt = DateTime.Now.AddDays(-1)
            },
            new Phone
            {
                Id = 10, Name = "Motorola Edge 40", Brand = "Motorola", Model = "XT2303",
                Price = 349.99m, OriginalPrice = 499.99m,
                Condition = PhoneCondition.UsedFair, DiscountPercentage = 30,
                Description = "Used Motorola Edge 40. Fair condition with visible signs of wear. Fully functional, 6 months warranty.",
                StockQuantity = 7,
                ImageUrl = "https://via.placeholder.com/600x750/5a5a8a/ffffff?text=Motorola+Edge+40",
                GalleryImages = new List<string> {
                    "https://via.placeholder.com/600x750/5a5a8a/ffffff?text=Motorola+Edge+40",
                    "https://via.placeholder.com/600x750/6a6a9a/ffffff?text=Side+View"
                },
                Specs = new PhoneSpecs { Display = "6.55\" P-OLED 144Hz", Processor = "Dimensity 8020", Ram = "8GB", Storage = "256GB", Camera = "50MP Main + 13MP UW", Battery = "4400mAh, 68W wired", Os = "Android 14", Weight = "168g", Connectivity = "5G, Wi-Fi 6E, Bluetooth 5.2" },
                Variants = new List<ProductVariant> { new() { Type = "Color", Value = "Eclipse Black" }, new() { Type = "Color", Value = "Nebula Green" } },
                Rating = 4.1, ReviewCount = 234,
                TotalSold = 1456, IsFeatured = false, Category = "Budget", CreatedAt = DateTime.Now.AddDays(-45)
            }
        };
    }

    public List<Phone> GetAllPhones() => _phones;

    public Phone? GetPhoneById(int id) => _phones.FirstOrDefault(p => p.Id == id);

    public List<Phone> GetRelatedPhones(Phone phone, int count = 4) => _phones
        .Where(p => p.Id != phone.Id)
        .OrderBy(p => p.Brand == phone.Brand ? 0 : 1)
        .ThenBy(p => Math.Abs(p.Price - phone.Price))
        .Take(count)
        .ToList();

    public List<Phone> GetPopularPhones(int count = 4) => _phones
        .OrderByDescending(p => p.TotalSold)
        .Take(count)
        .ToList();

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

    public List<Phone> SearchPhones(string query) => _phones
        .Where(p =>
            (p.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (p.Brand?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (p.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false))
        .Take(5)
        .ToList();
}