const hero = document.querySelector('.parallax-hero');
const midLayer = document.querySelector('.parallax-layer-mid');
if (!hero || !midLayer) throw new Error('Parallax hero not found');

let ticking = false;

window.addEventListener('scroll', () => {
    if (!ticking) {
        requestAnimationFrame(() => {
            const rect = hero.getBoundingClientRect();
            if (rect.bottom > 0 && rect.top < window.innerHeight) {
                const scrollFraction = -rect.top / (hero.offsetHeight + window.innerHeight);
                const offset = scrollFraction * 120;
                midLayer.style.transform = `translateY(${offset}px)`;
            }
            ticking = false;
        });
        ticking = true;
    }
}, { passive: true });
