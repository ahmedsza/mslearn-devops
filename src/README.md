# Out-of-Stock Items Feature Implementation

## Overview

This implementation addresses the requirement to improve the user experience when searching for products by providing clear visual indication and filtering options for out-of-stock items.

## Features Implemented

### 1. Visual "Out of Stock" Badge
- Out-of-stock products display a Bootstrap badge with "Out of Stock" label
- Badge uses `badge bg-secondary` class for consistency
- Badge appears next to the product name in search results

### 2. Sorting Logic
- Out-of-stock items automatically appear at the bottom of search results
- In-stock items are shown first, sorted alphabetically by name
- Out-of-stock items follow, also sorted alphabetically by name

### 3. Filter Option
- Checkbox option to "Hide out-of-stock items" in the search interface
- Filter can be combined with search terms
- Filter state persists across searches

### 4. Product Model
- `Product.AvailableStock` property stores inventory count
- `Product.IsOutOfStock` computed property returns `true` when `AvailableStock <= 0`

## Files Modified/Created

### Models
- `src/Models/Product.cs` - Product model with AvailableStock property and IsOutOfStock computed property

### Controllers
- `src/Controllers/CatalogController.cs` - Catalog controller with search and filter logic

### ViewModels
- `src/ViewModels/CatalogSearchViewModel.cs` - View model for catalog search results

### UI
- `src/UI/catalog-demo.html` - HTML demo page with product cards, badges, and filter UI

### Tests
- `Tests/CatalogControllerTests.cs` - Unit tests covering all new functionality

## Technical Details

### CatalogController.GetProducts Method

```csharp
public List<Product> GetProducts(string searchTerm = "", bool hideOutOfStock = false)
```

This method handles:
1. **Search filtering** - Filters products by name or description (case-insensitive)
2. **Out-of-stock filtering** - Optionally excludes out-of-stock items
3. **Sorting** - Orders by stock availability (in-stock first), then by name

### UI Components

The HTML demo page includes:
- Search input field with submit button
- "Hide out-of-stock items" checkbox that submits form on change
- Product cards with conditional badge rendering using Bootstrap `badge bg-secondary` class
- Disabled "Unavailable" button for out-of-stock products

## Testing

### Unit Tests Included

1. **Sorting Tests**
   - `GetProducts_WithNoFilters_ReturnsAllProductsSortedByAvailability`
   - `GetProducts_OutOfStockItemsAppearAtBottom`

2. **Filter Tests**
   - `GetProducts_WithHideOutOfStock_ReturnsOnlyInStockProducts`
   - `GetProducts_WithoutHideOutOfStock_ReturnsAllProducts`

3. **Search Tests**
   - `GetProducts_WithSearchTerm_FiltersCorrectly`
   - `GetProducts_WithSearchTermAndHideOutOfStock_AppliesBothFilters`

4. **Model Tests**
   - `IsOutOfStock_ReturnsTrueWhenStockIsZero`
   - `IsOutOfStock_ReturnsFalseWhenStockIsPositive`

### Running Tests

```bash
dotnet test
```

## Acceptance Criteria Status

- ✅ Out-of-stock products display a visual "Out of Stock" badge
- ✅ Out-of-stock items appear at the bottom of search results
- ✅ Filter option to hide out-of-stock items
- ✅ Unit tests cover the new filtering logic

## Future Enhancements

Potential improvements for future iterations:
- Add visual styling to dim out-of-stock product cards
- Show available stock count for in-stock items
- Add "Notify me when in stock" feature for out-of-stock items
- Implement pagination for large result sets
- Add analytics tracking for out-of-stock product views
