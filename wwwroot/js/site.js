// Mobile Menu Functionality
(function () {
    'use strict';

    // Get menu elements
    const menuToggle = document.getElementById('menuToggle');
    const menuClose = document.getElementById('menuClose');
    const mobileMenuOverlay = document.getElementById('mobileMenuOverlay');
    const body = document.body;

    // Open menu function
    function openMenu() {
        mobileMenuOverlay.classList.add('active');
        body.classList.add('menu-open');
        // Set focus to close button for accessibility
        menuClose.focus();
    }

    // Close menu function
    function closeMenu() {
        mobileMenuOverlay.classList.remove('active');
        body.classList.remove('menu-open');
        // Return focus to toggle button
        menuToggle.focus();
    }

    // Toggle menu button click
    if (menuToggle) {
        menuToggle.addEventListener('click', function (e) {
            e.stopPropagation();
            openMenu();
        });
    }

    // Close button click
    if (menuClose) {
        menuClose.addEventListener('click', function (e) {
            e.stopPropagation();
            closeMenu();
        });
    }

    // Click outside menu to close (clicking on overlay)
    if (mobileMenuOverlay) {
        mobileMenuOverlay.addEventListener('click', function (e) {
            // Only close if clicking directly on the overlay, not the menu content
            if (e.target === mobileMenuOverlay) {
                closeMenu();
            }
        });
    }

    // Close menu when a navigation link is clicked
    const mobileNavLinks = document.querySelectorAll('.mobile-nav-link');
    mobileNavLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            closeMenu();
        });
    });

    // Close menu on Escape key
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && mobileMenuOverlay.classList.contains('active')) {
            closeMenu();
        }
    });

    // Handle orientation change on mobile devices
    window.addEventListener('orientationchange', function () {
        // Close menu on orientation change to prevent layout issues
        if (mobileMenuOverlay.classList.contains('active')) {
            closeMenu();
        }
    });

    // iOS Safari specific: Handle viewport resize
    let resizeTimer;
    window.addEventListener('resize', function () {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function () {
            // Close menu if screen becomes desktop size
            if (window.innerWidth > 768 && mobileMenuOverlay.classList.contains('active')) {
                closeMenu();
            }
        }, 250);
    });
})();
