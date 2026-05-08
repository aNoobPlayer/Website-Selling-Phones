// ============================================
// PhoneStore - Site JavaScript
// Dark Mode, SignalR, Micro-animations
// ============================================

// ---- Dark Mode Toggle ----
function toggleTheme() {
    const html = document.documentElement;
    const current = html.getAttribute('data-theme');
    const next = current === 'dark' ? 'light' : 'dark';
    html.setAttribute('data-theme', next);
    localStorage.setItem('phonestore-theme', next);
}

// Load saved theme
(function () {
    const saved = localStorage.getItem('phonestore-theme');
    if (saved === 'dark') {
        document.documentElement.setAttribute('data-theme', 'dark');
    }
})();

// ---- Add to Cart Animation ----
function addToCart(btn, productName) {
    btn.classList.add('cart-success');
    btn.innerHTML = '<i class="bi bi-check-lg me-2"></i>Added!';

    setTimeout(() => {
        btn.classList.remove('cart-success');
        btn.innerHTML = '<i class="bi bi-cart-plus me-2"></i>Add to Cart';
    }, 1500);

    // Toast notification
    const toast = document.createElement('div');
    toast.className = 'cart-toast';
    toast.innerHTML = `<i class="bi bi-check-circle-fill me-2"></i>${productName} added to cart!`;
    document.body.appendChild(toast);

    requestAnimationFrame(() => {
        toast.classList.add('show');
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 300);
        }, 2000);
    });
}

// ---- Smooth Scroll ----
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            e.preventDefault();
            target.scrollIntoView({ behavior: 'smooth' });
        }
    });
});

// ---- Intersection Observer for Fade-in ----
const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
        }
    });
}, { threshold: 0.1 });

document.querySelectorAll('[data-animate]').forEach(el => observer.observe(el));