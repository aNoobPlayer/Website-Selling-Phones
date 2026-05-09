export function initVariantSwitcher(phoneId) {
    document.querySelectorAll('.variant-btn[data-type="Color"]').forEach(btn => {
        btn.addEventListener('click', function() {
            const parent = this.parentElement;
            parent.querySelectorAll('.variant-btn').forEach(b => b.classList.remove('active'));
            this.classList.add('active');
            switchVariant(phoneId, 'Color', this.dataset.value);
        });
    });

    document.querySelectorAll('.variant-btn[data-type="Storage"]').forEach(btn => {
        btn.addEventListener('click', function() {
            const parent = this.parentElement;
            parent.querySelectorAll('.variant-btn').forEach(b => b.classList.remove('active'));
            this.classList.add('active');
            switchVariant(phoneId, 'Storage', this.dataset.value);
        });
    });

    // Set initial hidden inputs from pre-selected variant
    updateCartVariantInputs();
}

function updateCartVariantInputs() {
    const color = document.querySelector('.variant-btn[data-type="Color"].active')?.dataset.value || '';
    const storage = document.querySelector('.variant-btn[data-type="Storage"].active')?.dataset.value || '';
    let colorInput = document.getElementById('cartColor');
    let storageInput = document.getElementById('cartStorage');
    if (!colorInput) {
        colorInput = document.createElement('input');
        colorInput.type = 'hidden';
        colorInput.id = 'cartColor';
        document.getElementById('addToCartBtn')?.after(colorInput);
    }
    if (!storageInput) {
        storageInput = document.createElement('input');
        storageInput.type = 'hidden';
        storageInput.id = 'cartStorage';
        document.getElementById('cartColor')?.after(storageInput);
    }
    colorInput.value = color;
    storageInput.value = storage;
}

async function switchVariant(phoneId, type, value) {
    try {
        const response = await fetch(`/Product/VariantInfo?phoneId=${phoneId}&variantType=${type}&variantValue=${encodeURIComponent(value)}`);
        if (!response.ok) throw new Error('Variant not found');
        const data = await response.json();

        const priceTag = document.querySelector('.price-tag');
        const discountBadge = document.querySelector('.badge.bg-danger');
        const mainImage = document.getElementById('mainImage');

        if (data.imageUrl && mainImage) {
            mainImage.style.opacity = '0';
            setTimeout(() => {
                mainImage.src = data.imageUrl;
                mainImage.style.opacity = '1';
                document.querySelectorAll('.gallery-thumb').forEach(t => {
                    t.classList.toggle('active', t.src === data.imageUrl);
                });
            }, 300);
        }

        if (priceTag) {
            const targetPrice = data.discountedPrice || data.price;
            morphPrice(priceTag, targetPrice, 400);
        }

        if (discountBadge) {
            if (data.discountPercent > 0) {
                discountBadge.textContent = 'Save ' + data.discountPercent + '%';
                discountBadge.style.display = '';
            } else {
                discountBadge.style.display = 'none';
            }
        }

        const storageLabel = document.querySelector('.specs-table tr:nth-child(4) td:last-child');
        if (storageLabel && type === 'Storage') {
            storageLabel.textContent = value;
        }

        updateCartVariantInputs();

    } catch (err) {
        console.error('Variant switch failed:', err);
    }
}

function morphPrice(el, target, duration) {
    const current = parseFloat(el.textContent.replace(/[^0-9.]/g, ''));
    if (isNaN(current)) return;

    const start = performance.now();

    function update(now) {
        const elapsed = now - start;
        const progress = Math.min(elapsed / duration, 1);
        const eased = 1 - Math.pow(1 - progress, 3);
        const value = current + (target - current) * eased;
        el.textContent = '$' + value.toFixed(2);
        if (progress < 1) requestAnimationFrame(update);
    }

    requestAnimationFrame(update);
}
