# Liquid UI Upgrade Plan — PhoneStore ASP.NET Core

## Overview
Progressive enhancement of landing page, product filters, and product detail page with fluid/liquid design principles: smooth transitions, micro-interactions, parallax, glassmorphism, and AJAX-driven interactions.

**Approach**: Progressive Enhancement (extends existing patterns — partial views, JSON endpoints, `[data-animate]`, CSS custom properties — rather than a full rewrite).

---

## Phase 1: Landing Page — Complete Rewrite

### Files to modify:
| File | Action |
|------|--------|
| `Pages/Index.cshtml.cs` | Switch from `Product` → `Phone` model, add brand list, popular products |
| `Pages/Index.cshtml` | Rewrite all 4 sections with liquid styling |
| `wwwroot/css/site.css` | Add ~200 lines: parallax hero, glass cards, brand tilt, scroll effects |

### Index.cshtml.cs changes:
- Replace `List<Product> FeaturedProducts` with `List<Phone> FeaturedPhones`
- Add `List<Phone> PopularPhones` (top by TotalSold, take 4)
- Add `List<BrandInfo> TopBrands` with Name, ImageUrl, Slug — from MockDataService brands
- FeaturedPhones: `GetAllPhones().Where(p => p.IsFeatured).OrderByDescending(p => p.Rating).Take(6)`

### New Hero Section (`.parallax-hero`) — replaces broken `.hero-section`:
- Full-viewport (85vh) parallax container with 3 layered divs
- Layer 1: animated gradient background (dark blue/indigo/slate)
- Layer 2: radial glow gradient that shifts with CSS animation
- Layer 3 (front): centered `.glass-hero-card` with backdrop-filter blur
  - Animated gradient text on `<h1>` ("Your Next Phone Awaits")
  - Subtitle, CTA links (Shop Now / Browse Brands)
  - Floating device mockup with subtle float animation
- Parallax effect: JS `scroll` handler updates `--scroll-offset` CSS variable for layer 2

### Feature Cards: `.feature-card` → `.glass-feature-card`:
- Glassmorphism: `rgba(255,255,255,0.08)` bg + `backdrop-filter: blur(12px)` + `border: 1px solid rgba(255,255,255,0.12)`
- Animated gradient icon container (circle, border gradient rotating)
- Hover: translateY(-8px) + shadow elevation
- Staggered entrance via `[data-animate="fadeInUp"]` (already works with IntersectionObserver in site.js)

### Featured Products: `.product-card` → `.product-card-v2`:
- Switch from old class to fully-styled `.product-card-v2` with hover lift + image zoom
- Use `[data-animate="fadeInUp"]` instead of dead `.animate-in`
- Staggered delays already in CSS (nth-child 1-10)
- Add Quick View trigger on image hover

### Brand Cards: emoji hardcode → `.glass-brand-card` from data:
- Rendered from `TopBrands` list (data-driven)
- Glassmorphism background + hover tilt effect
- **Tilt effect**: JS `mousemove` handler applies `transform: perspective(600px) rotateX(var(--tilt-x)) rotateY(var(--tilt-y))` via CSS custom properties on `[data-tilt]`
- Clickable links to `/Product?brand=X`
- Brand icons from Bootstrap Icons where available, initials fallback

### New "Trending Now" strip:
- Horizontal scrollable row of compact product cards
- `scroll-snap-type: x mandatory` + `scroll-behavior: smooth`
- Compact card variant with image + name + price only
- Entrance via IntersectionObserver

---

## Phase 2: Product Listing — AJAX Filters

### Files to create:
| File | Purpose |
|------|---------|
| `Views/Product/_ProductGrid.cshtml` | Extracted product grid partial (reusable for AJAX) |
| `Views/Product/_Pagination.cshtml` | Extracted pagination bar partial |
| `wwwroot/js/modules/liquid-filters.js` | AJAX filter pipeline (ES module) |

### Files to modify:
| File | Change |
|------|--------|
| `Controllers/ProductController.cs` | Add `FilterAjax` POST endpoint returning JSON partial HTML |
| `Views/Product/Index.cshtml` | Refactor to use partials, add price slider JS, remove full-page links |
| `wwwroot/css/site.css` | Add noUiSlider styles, skeleton overlay, transition classes |

### Controller: New `[HttpPost] FilterAjax` endpoint:
- Returns JSON: `{ html: "...", paginationHtml: "...", totalItems: N, activeFilterCount: N }`
- Same filtering logic as `Index()` but renders partials via a `RenderViewToStringAsync` helper
- Accepts same parameters: brand, minPrice, maxPrice, category, condition, sort, search, view, page, pageSize

### Price Slider: noUiSlider CDN:
- Dual range: $0–$1500, step $50, connect bar between handles
- Styled with CSS custom properties from site.css design tokens
- Debounced 400ms change → triggers AJAX filter update
- Live min/max labels positioned below track

### AJAX Filter Pipeline (`liquid-filters.js`):
```
User action (brand click, sort change, price slider, grid/list toggle)
  → preventDefault, build query string from all active filters + page=1
  → fetch POST /Product/FilterAjax
  → show skeleton shimmer overlay on #productGrid
  → update: #productGrid innerHTML + #paginationBar innerHTML + #resultCount + #filterTags
  → history.pushState to update URL (shareable/bookmarkable)
  → re-initialize: cart forms, wishlist hearts, [data-animate] observer
  → scroll to top of product grid with smooth behavior
```

### Browser back/forward:
- `window.addEventListener('popstate', ...)` reads URL params, triggers AJAX filter
- No full page reload on back navigation

### View toggle (grid/list):
- Grid view: `<div class="product-grid">` with `.product-card-v2` items
- List view: `<div class="product-list-view">` with `.product-card-list` items
- CSS `transition` on card width/height for morphing between modes

### Active filter tags:
- Pill badges showing active filters with `×` remove button
- Clicking `×` removes that single filter, re-triggers AJAX load
- "Clear All" link resets to `/Product`

---

## Phase 3: Product Detail — Variant Enhancement

### Files to modify:
| File | Change |
|------|--------|
| `Controllers/ProductController.cs` | Add `[HttpGet] VariantInfo` endpoint |
| `Views/Product/Detail.cshtml` | Switch layout to `_ShopLayout`, enhance variant JS, add image crossfade |
| `wwwroot/js/modules/variant-switcher.js` | New ES module for variant switching |
| `wwwroot/css/site.css` | Add image crossfade, price morph styles |

### Layout Change:
- Detail page currently uses `_ItemLayout` (simpler, no glass nav, no mobile bottom nav, no dark mode toggle)
- Switch to `_ShopLayout` for consistency → glass navbar, dark mode toggle, mobile bottom nav all work

### New Controller Endpoint:
```
GET /Product/VariantInfo?phoneId=X&variantType=Color|Storage&variantValue=Y
```
Returns JSON:
```json
{
  "price": 1249.99,
  "priceModifier": 50.00,
  "discountedPrice": 1062.49,
  "discountPercent": 15,
  "imageUrl": "/images/phones/iphone15-blue.jpg"
}
```

### Variant Button Behavior (JS):
- **Color click**: highlight button → fetch VariantInfo → crossfade main image → morph price → update gallery thumbnails → update discount badge
- **Storage click**: highlight button → fetch VariantInfo → morph price → update "Save X%" badge → update storage in specs table
- **Price morph**: `requestAnimationFrame` animation from old→new value over 400ms with easeOutExpo

### Image Crossfade:
```css
.product-gallery img {
  transition: opacity 0.4s ease;
}
.product-gallery img.crossfading {
  opacity: 0;
}
```
JS: set opacity to 0 → change src → set opacity back to 1 after transitionend

### Gallery Thumbnail Sync:
- When color changes, update thumbnail strip to show images matching the color variant
- If no variant-specific images exist, keep current gallery

---

## Phase 4: CSS Additions (~350 lines appended to site.css)

### New CSS Custom Properties:
```css
:root {
  --glass-bg: rgba(255,255,255,0.06);
  --glass-border: rgba(255,255,255,0.12);
  --glass-blur: 12px;
  --hero-gradient-1: #0f172a;
  --hero-gradient-2: #1e3a5f;
  --hero-gradient-3: #2563eb;
}
```

### New Keyframes & Classes:
- `@keyframes morphGradient` — background-position sweep for hero gradient animation
- `@keyframes float` — gentle up/down movement for floating device mockup (translateY ±10px, 4s ease-in-out infinite)
- `@keyframes spin-gradient` — rotate gradient on feature icon containers
- `.parallax-hero` + `.parallax-layer` — 3-layer parallax hero structure
- `.glass-hero-card` — backdrop-filter blur centered card
- `.glass-feature-card` — glass card with hover lift
- `.glass-brand-card` — glass card with perspective for tilt effect
- `[data-tilt]` — perspective container for tilt JS
- `.price-morph` — transition for smooth price changes
- `.product-grid.transitioning .product-card-v2` — width/height transition for grid↔list morph
- `.skeleton-grid-overlay` — absolute positioned shimmer overlay for AJAX loading state
- noUiSlider custom theme (`.noUi-target`, `.noUi-connect`, `.noUi-handle`)
- `.filter-applied` badge styles

### Dark Mode Adjustments:
- Glass backgrounds adjust opacity for readability on dark
- Parallax gradient shifts to darker tones
- noUiSlider handle/connect colors use design tokens

---

## Phase 5: JS Architecture

### Files to create:
| File | Purpose |
|------|---------|
| `wwwroot/js/modules/liquid-filters.js` | AJAX filter pipeline, URL sync |
| `wwwroot/js/modules/parallax-hero.js` | Scroll-driven parallax on landing hero |
| `wwwroot/js/modules/variant-switcher.js` | Variant click → price/image update |
| `wwwroot/js/modules/tilt-cards.js` | Mouse-tracking tilt on brand cards |
| `wwwroot/js/modules/utils.js` | morphValue, debounce, renderViewToString helpers |

### Loaded via `<script type="module" src="~/js/modules/liquid-filters.js">` in relevant views
### site.js remains for theme toggle, cart toast, IntersectionObserver (globally loaded)

### `parallax-hero.js`:
- Listens to `scroll` on window (passive listener)
- Calculates scroll position relative to hero section
- Updates `--scroll-offset` CSS variable for layer 2 parallax
- Uses `requestAnimationFrame` for smooth updates

### `tilt-cards.js`:
- Observes all `[data-tilt]` elements with `mouseenter`
- On `mousemove`: calculates rotation based on mouse position relative to card center
- Sets `--tilt-x` and `--tilt-y` CSS properties
- On `mouseleave`: resets to `0deg` with smooth transition
- Max tilt: ±10deg

### `morphValue(el, target, duration)`:
- Reads current value from element text
- Animates from current→target over `duration` ms
- Uses easeOutExpo easing
- Formats as currency ($X,XXX.XX)

---

## Phase 6: Integration — Consistent Layout

### Detail page layout switch:
- `Views/Product/Detail.cshtml`: change `Layout = "_ItemLayout"` → `Layout = "_ShopLayout"`
- Remove `_ItemLayout` usage entirely (no other pages use it)
- This gives detail page: glass navbar, mobile bottom nav, dark mode toggle, cart badge in nav

### Breadcrumb enhancement:
- Add animated chevron separators
- Add `text-truncate` on long product names

### Cross-page transitions:
- When navigating from listing → detail: product card image position animates to detail page position
- This is a CSS View Transitions API enhancement (progressive, Chrome/Edge only)
- Add `<meta name="view-transition" content="same-origin">` to _ShopLayout
- Named view transitions on product images: `view-transition-name: product-{id}`

---

## Verification Plan

### Landing Page:
1. Visit `/` — hero should have animated gradient + parallax scrolling
2. Feature cards should have glass effect + hover lift
3. Featured products should use styled card-v2 with staggered entrance animation
4. Brand cards should tilt on hover (mouse tracking) + link to filtered product page
5. Trending strip should scroll horizontally with snap

### Product Listing:
1. Visit `/Product` — click any brand filter
2. Page should NOT fully reload — skeleton shimmer shows, then grid updates in-place
3. URL should update, back button should restore previous filters
4. Price slider should filter results (debounced AJAX)
5. Grid/List toggle should morph cards smoothly
6. Active filter pills should appear and be individually removable
7. Sort should trigger AJAX update

### Product Detail:
1. Visit `/Product/Details/1` — should use ShopLayout (glass nav, dark mode, mobile nav)
2. Click color variant → image crossfades, price morphs, thumbnails update
3. Click storage variant → price morphs, specs table updates
4. Gallery thumbnails should switch main image on click
5. View transitions should animate image when navigating from listing

### Cross-cutting:
1. Build with `dotnet build` — zero errors
2. All existing endpoints still return correct status codes
3. Dark mode toggle works on all pages
4. Mobile bottom nav works on all pages
5. Cart count badge updates on all pages
