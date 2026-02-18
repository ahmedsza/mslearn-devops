# Quick Start Guide

## Testing the Mobile Menu Fix

### Option 1: Quick Demo (Recommended)
1. Open `demo.html` in a web browser
2. Resize browser to mobile size (< 768px width) or use DevTools device emulation
3. Click the hamburger menu (☰) to open
4. Test closing methods:
   - Click outside the menu (on dark overlay)
   - Click the × button
   - Press Escape key

### Option 2: Using a Local Server
```bash
# Start a simple HTTP server
cd /path/to/mslearn-devops
python3 -m http.server 8000

# Open browser to http://localhost:8000/demo.html
```

### Option 3: Mobile Device Testing
1. Copy the repository to a web server
2. Access the demo.html from your iOS or Android device
3. Test all menu interactions

## Integrating into Your ASP.NET Application

### Step 1: Copy Files
Copy these files to your ASP.NET project:
- `Views/Shared/_Layout.cshtml` → Replace your existing layout
- `wwwroot/css/site.css` → Replace or merge with your existing CSS
- `wwwroot/js/site.js` → Add to your wwwroot/js directory

### Step 2: Update Your Layout (if merging)
If you have an existing `_Layout.cshtml`, add these sections:

1. **In the header** - Add the mobile menu structure:
```html
<!-- Hamburger menu button for mobile -->
<button class="navbar-toggler" type="button" id="menuToggle" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
    <span class="navbar-toggler-icon"></span>
    <span class="navbar-toggler-icon"></span>
</button>

<!-- Mobile menu overlay -->
<div class="mobile-menu-overlay" id="mobileMenuOverlay">
    <div class="mobile-menu-content">
        <button class="mobile-menu-close" id="menuClose" aria-label="Close menu">
            <span>&times;</span>
        </button>
        <nav class="mobile-menu-nav">
            <!-- Your navigation links here -->
        </nav>
    </div>
</div>
```

2. **Before closing body tag** - Add the script reference:
```html
<script src="~/js/site.js" asp-append-version="true"></script>
```

### Step 3: Customize (Optional)
Edit `wwwroot/css/site.css` to customize:
- Colors (change #0078d7 to your brand color)
- Menu width (default 80%, max 300px)
- Animation timing (default 0.3s)
- Breakpoint (default 768px)

## Browser Compatibility
- ✅ iOS Safari 15+
- ✅ Chrome Mobile
- ✅ Safari Desktop
- ✅ Chrome Desktop
- ✅ Firefox
- ✅ Edge

## Accessibility Features
- Keyboard navigation (Escape to close)
- ARIA labels on all interactive elements
- Focus management
- High contrast mode support
- Reduced motion support

## Troubleshooting

### Menu doesn't open
- Check that `site.js` is loaded (check browser console)
- Verify element IDs match: `menuToggle`, `mobileMenuOverlay`, `menuClose`

### Menu doesn't close on overlay click
- Ensure JavaScript is enabled
- Check browser console for errors

### Styling issues
- Verify `site.css` is loaded
- Check for CSS conflicts with existing styles
- Ensure viewport meta tag is present: `<meta name="viewport" content="width=device-width, initial-scale=1.0" />`

### iOS Safari specific issues
- Clear browser cache
- Test in private browsing mode
- Verify -webkit prefixes are present in CSS

## Questions or Issues?
See `MOBILE_MENU_FIX.md` for detailed technical documentation.
