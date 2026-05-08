# 📱 PhoneStore - ASP.NET Core 8 E-Commerce App

A full-featured phone selling website built with ASP.NET Core 8 Razor Pages + MVC, Entity Framework Core, SQL Server LocalDB, SignalR, and modern CSS.

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- SQL Server LocalDB (or full SQL Server)
- Node.js (optional, for frontend bundling)

### Setup

```bash
cd "E:\VibeCode\Website Selling Phones"

# Restore packages
dotnet restore

# Run identity database migrations
dotnet ef database update --context AppIdentityDbContext

# Run product database migrations  
dotnet ef database update --context ApplicationDbContext

# Start the app
dotnet run
```

Open http://localhost:5154

### Default Admin Account
- **Email:** admin@phonestore.com
- **Password:** Admin123!

## 🏗 Architecture

### Backend (C# / ASP.NET Core 8)
- **Razor Pages** for Admin dashboard (`/Admin/*`)
- **MVC Controllers** for Shop, Cart, Orders (`/Product`, `/Cart`, `/Order`)
- **Web API** endpoints at `/api/phones/*`
- **SignalR** real-time hub at `/hubs/inventory`
- **Entity Framework Core** with SQL Server for persistence
- **ASP.NET Identity** for authentication & role-based access

### Frontend
- **Bootstrap 5** with custom CSS variables
- **Three layouts:** `_ShopLayout`, `_ItemLayout`, `_AdminLayout`
- **Dark mode** toggle with localStorage persistence
- **Glassmorphism** navbar with `backdrop-filter: blur(10px)`
- **CSS Grid** product layout with skeleton loading animations
- **Bootstrap Icons** throughout

## 📁 Project Structure

```
Pages/               # Razor Pages (Admin, Landing)
  Admin/             # Admin dashboard, CRUD operations
  Products/          # Product listing & details (Razor Pages)
  Shared/            # _ShopLayout, _ItemLayout, _AdminLayout

Controllers/         # MVC Controllers
  ProductController  # Shop page with filtering, sorting, pagination
  CartController     # Session-based shopping cart
  OrderController    # Checkout & order history
  AccountController  # Login, Register, User management
  PhonesController   # REST API

Models/
  Phone.cs           # Product model with Condition enum
  ShoppingCart.cs    # Cart logic
  Order.cs           # Order & OrderItem models
  Review.cs          # User reviews
  ApplicationUser.cs # Identity user

Services/
  MockDataService.cs     # 10 seeded phone products
  DynamicPricingService.cs  # Brand multipliers, VIP pricing

Hubs/
  InventoryHub.cs     # SignalR for real-time stock alerts

ViewComponents/
  CategoryMenuViewComponent.cs  # Sidebar filter menu

Data/
  ApplicationDbContext.cs   # Products DB context
  AppIdentityDbContext.cs    # Identity DB context

wwwroot/css/
  site.css           # Complete stylesheet (1000+ lines)
```

## 🔐 Authentication & Authorization

- **ASP.NET Core Identity** with Entity Framework
- **Role-based access:** Admin, Customer
- Admin routes (`/Admin/*`) protected with `[Authorize(Roles = "Admin")]`
- Login/Register at `/Account/Login` and `/Account/Register`
- User management at `/Account/ManageUsers` (Admin only)
- First registered user automatically becomes Admin

## 🛒 Shopping Cart & Checkout

- **Session-based cart** using `ISession`
- Add/Remove/Update quantity
- Checkout with address & payment method selection
- Order confirmation with order number
- Order history for authenticated users

## 📡 SignalR Real-Time Features

- **Low stock alerts** pushed to all connected clients
- **Price updates** streamed to product detail pages
- **New arrival notifications**
- Hub endpoint: `/hubs/inventory`

## 🎨 UI Features

### Three Layouts
| Layout | Used By | Features |
|--------|---------|----------|
| `_ShopLayout` | Home, Shop, Cart | Top bar, glass-nav, search, cart, user menu, footer |
| `_ItemLayout` | Product Details, Account | Compact nav, breadcrumbs, back button |
| `_AdminLayout` | Admin Dashboard | Sidebar, stats, full-width tables |

### Dark Mode
Toggle via sun/moon button in the top bar. Persisted in localStorage. Uses CSS custom properties `--bg-color`, `--text-color`, etc.

### Condition Badges
- 🟢 **New** — green badge
- 🔵 **Refurbished** — blue badge  
- ⚫ **Used** — gray badge

## 📊 Product Data

10 pre-seeded phones:
- iPhone 15 Pro Max, iPhone 14 Pro (Refurb), iPhone SE 2022 (Used)
- Samsung Galaxy S24 Ultra, Galaxy S23+ (Refurb)
- Google Pixel 8 Pro
- OnePlus 12
- Xiaomi 14 Ultra
- Nothing Phone (2)
- Motorola Edge 40 (Used)

## 🔌 API Endpoints

| Method | URL | Description |
|--------|-----|-------------|
| GET | `/api/phones` | List all phones (filterable: `?brand=Apple&minPrice=500`) |
| GET | `/api/phones/{id}` | Get phone by ID |
| GET | `/api/phones/brands` | List all brands |
| GET | `/api/phones/categories` | List all categories |

## 🛠 Configuration

**appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PhoneStore;Trusted_Connection=True"
  }
}
```

## 🔄 CI/CD (Suggested)

```yaml
# .github/workflows/dotnet.yml
name: Build & Test
on: [push, pull_request]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-dotnet@v3
        with: { dotnet-version: '8.0.x' }
      - run: dotnet build
      - run: dotnet test
```

## 📋 Features Checklist

- [x] User login/registration with Identity
- [x] Role-based access (Admin/Customer)
- [x] Product listing with search, filter, sort, pagination
- [x] Product detail with reviews & star ratings
- [x] Shopping cart (session-based)
- [x] Checkout & order history
- [x] Admin dashboard with CRUD
- [x] Admin user management
- [x] SignalR real-time notifications
- [x] Web API endpoints
- [x] Dynamic pricing engine
- [x] Dark mode toggle
- [x] Glassmorphism UI
- [x] Skeleton loading animation
- [x] Condition badges
- [x] Responsive design
- [x] Performance caching