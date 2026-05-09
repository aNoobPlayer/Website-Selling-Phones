let filtersLoaded = false;

export function initLiquidFilters() {
    if (filtersLoaded) return;
    filtersLoaded = true;

    // Intercept all filter links (brand, category, condition)
    document.querySelectorAll('[data-filter-link]').forEach(link => {
        link.addEventListener('click', e => {
            e.preventDefault();
            const url = new URL(link.href, window.location.origin);
            fetchFilterResults(url.searchParams);
        });
    });

    // Also handle sidebar filter links without data-filter-link
    document.querySelectorAll('.filter-link').forEach(link => {
        link.addEventListener('click', e => {
            e.preventDefault();
            const url = new URL(link.href, window.location.origin);
            fetchFilterResults(url.searchParams);
        });
    });

    // Condition chips
    document.querySelectorAll('.sidebar-filter .btn').forEach(btn => {
        if (btn.tagName === 'A') {
            btn.addEventListener('click', e => {
                e.preventDefault();
                const url = new URL(btn.href, window.location.origin);
                fetchFilterResults(url.searchParams);
            });
        }
    });

    // Sort dropdown
    const sortSelect = document.querySelector('#sortSelect');
    if (sortSelect) {
        sortSelect.addEventListener('change', () => {
            const params = new URLSearchParams(window.location.search);
            params.set('sort', sortSelect.value);
            params.set('page', '1');
            fetchFilterResults(params);
        });
    }

    // Grid/List toggle buttons
    document.querySelectorAll('.view-toggle-btn').forEach(btn => {
        btn.addEventListener('click', e => {
            e.preventDefault();
            const params = new URLSearchParams(window.location.search);
            params.set('view', btn.dataset.view);
            params.set('page', '1');
            fetchFilterResults(params);
        });
    });

    // Search input - submit on Enter
    const searchInput = document.querySelector('#searchInput');
    if (searchInput) {
        searchInput.addEventListener('keypress', e => {
            if (e.key === 'Enter') {
                e.preventDefault();
                const params = new URLSearchParams(window.location.search);
                if (searchInput.value) params.set('search', searchInput.value);
                else params.delete('search');
                params.set('page', '1');
                fetchFilterResults(params);
            }
        });
    }

    // Browser back/forward
    window.addEventListener('popstate', () => {
        const params = new URLSearchParams(window.location.search);
        fetchFilterResults(params, false);
    });
}

// Expose globally for inline scripts (price slider)
window.fetchFilterResults = fetchFilterResults;

async function fetchFilterResults(params, pushState = true) {
    const grid = document.querySelector('#productGrid');
    const pagination = document.querySelector('#paginationBar');
    const resultCount = document.querySelector('#resultCount');
    const activeFilterCount = document.querySelector('#activeFilterCount');

    if (!grid) return;

    // Show skeleton overlay
    grid.classList.add('position-relative');
    const skeleton = document.createElement('div');
    skeleton.className = 'skeleton-grid-overlay';
    skeleton.innerHTML = Array(4).fill('<div class="skeleton skeleton-img"></div>').join('');
    grid.innerHTML = '';
    grid.appendChild(skeleton);

    try {
        const formData = new URLSearchParams();
        for (const [key, value] of params) {
            if (value) formData.append(key, value);
        }

        const response = await fetch('/Product/FilterAjax', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: formData.toString()
        });

        if (!response.ok) throw new Error('Filter failed');

        const data = await response.json();

        // Update grid
        grid.innerHTML = data.html;

        // Update pagination
        if (pagination) pagination.innerHTML = data.paginationHtml || '';

        // Update result count
        if (resultCount) resultCount.textContent = data.totalItems + ' results';

        // Update active filter count badge
        if (activeFilterCount) {
            activeFilterCount.textContent = data.activeFilterCount + ' active';
            activeFilterCount.style.display = data.activeFilterCount > 0 ? '' : 'none';
        }

        // Update URL without reload
        if (pushState) {
            const newUrl = window.location.pathname + '?' + params.toString();
            window.history.pushState({}, '', newUrl);
        }

        // Re-initialize interactions
        reinitInteractions();

        // Scroll to product grid top
        grid.scrollIntoView({ behavior: 'smooth', block: 'start' });

    } catch (err) {
        console.error('Filter error:', err);
        grid.innerHTML = '<div class="text-center py-5 text-danger">Failed to load results. Please try again.</div>';
    }
}

function reinitInteractions() {
    // Re-init AJAX cart forms
    document.querySelectorAll('#productGrid .ajax-cart-form').forEach(form => {
        if (form.dataset.ajaxInit) return;
        form.dataset.ajaxInit = '1';
        form.addEventListener('submit', async e => {
            e.preventDefault();
            const formData = new FormData(form);
            const response = await fetch('/Cart/AddAjax', { method: 'POST', body: formData });
            const data = await response.json();
            const btn = form.querySelector('button');
            btn.classList.add('cart-success');
            btn.innerHTML = '<i class="bi bi-check-lg"></i>';
            setTimeout(() => { btn.classList.remove('cart-success'); btn.innerHTML = 'Add to Cart'; }, 1000);
            updateCartBadge(data.totalItems);
        });
    });

    // Re-init wishlist buttons
    document.querySelectorAll('#productGrid .wishlist-quick-btn').forEach(btn => {
        if (btn.dataset.wishlistInit) return;
        btn.dataset.wishlistInit = '1';
        btn.addEventListener('click', function(e) {
            e.preventDefault();
            const id = this.dataset.id;
            fetch('/Wishlist/Toggle', { method:'POST', headers:{'Content-Type':'application/x-www-form-urlencoded'}, body:'id='+id })
                .then(r => r.json())
                .then(data => {
                    const icon = this.querySelector('i');
                    icon.classList.toggle('bi-heart-fill', data.added);
                    icon.classList.toggle('text-danger', data.added);
                    icon.classList.toggle('bi-heart', !data.added);
                });
        });
    });

    // Re-init filter links in new pagination
    document.querySelectorAll('#paginationBar [data-filter-link]').forEach(link => {
        if (link.dataset.filterBound) return;
        link.dataset.filterBound = '1';
        link.addEventListener('click', e => {
            e.preventDefault();
            const url = new URL(link.href, window.location.origin);
            const params = new URLSearchParams(window.location.search);
            const pageParam = new URLSearchParams(url.search).get('page');
            if (pageParam) params.set('page', pageParam);
            fetchFilterResults(params);
        });
    });
}

function updateCartBadge(count) {
    document.querySelectorAll('.cart-count-badge').forEach(badge => {
        badge.textContent = count;
        badge.style.display = count > 0 ? '' : 'none';
    });
}
