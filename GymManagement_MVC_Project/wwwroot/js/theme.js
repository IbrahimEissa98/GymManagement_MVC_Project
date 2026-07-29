(function () {
    const STORAGE_KEY = 'gym-theme';
    const html = document.documentElement;
    const toggle = document.getElementById('themeToggle');
    const icon = document.getElementById('themeIcon');

    function getPreferredTheme() {
        const stored = localStorage.getItem(STORAGE_KEY);
        if (stored === 'light' || stored === 'dark') return stored;
        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    }

    function applyTheme(theme, persist) {
        html.setAttribute('data-bs-theme', theme);
        if (persist) localStorage.setItem(STORAGE_KEY, theme);
        if (icon) {
            icon.className = theme === 'dark' ? 'bi bi-sun-fill' : 'bi bi-moon-stars-fill';
        }
        if (toggle) {
            toggle.setAttribute('aria-label', theme === 'dark' ? 'Switch to light mode' : 'Switch to dark mode');
            toggle.setAttribute('title', theme === 'dark' ? 'Light mode' : 'Dark mode');
        }
    }

    // Theme is already applied by the inline head script — sync toggle icon only
    applyTheme(getPreferredTheme()||html.getAttribute('data-bs-theme')  , false);

    if (toggle) {
        toggle.addEventListener('click', function () {
            const next = html.getAttribute('data-bs-theme') === 'dark' ? 'light' : 'dark';
            applyTheme(next, true);
        });
    }

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', function (e) {
        if (!localStorage.getItem(STORAGE_KEY)) {
            applyTheme(e.matches ? 'dark' : 'light', false);
        }
    });
})();