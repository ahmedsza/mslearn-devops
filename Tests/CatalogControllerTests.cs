using Xunit;
using System.Linq;
using EShopOnWeb.Controllers;
using EShopOnWeb.Models;

namespace EShopOnWeb.Tests
{
    public class CatalogControllerTests
    {
        [Fact]
        public void GetProducts_WithNoFilters_ReturnsAllProductsSortedByAvailability()
        {
            // Arrange
            var controller = new CatalogController();
            
            // Act
            var result = controller.GetProducts();
            
            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
            
            // Verify in-stock items come before out-of-stock items
            var firstOutOfStockIndex = result.FindIndex(p => p.IsOutOfStock);
            if (firstOutOfStockIndex > 0)
            {
                // All items before first out-of-stock should be in stock
                for (int i = 0; i < firstOutOfStockIndex; i++)
                {
                    Assert.False(result[i].IsOutOfStock, $"Product at index {i} should be in stock");
                }
            }
        }
        
        [Fact]
        public void GetProducts_WithHideOutOfStock_ReturnsOnlyInStockProducts()
        {
            // Arrange
            var controller = new CatalogController();
            
            // Act
            var result = controller.GetProducts(hideOutOfStock: true);
            
            // Assert
            Assert.NotNull(result);
            Assert.All(result, product => Assert.False(product.IsOutOfStock));
        }
        
        [Fact]
        public void GetProducts_WithoutHideOutOfStock_ReturnsAllProducts()
        {
            // Arrange
            var controller = new CatalogController();
            
            // Act
            var resultWithoutFilter = controller.GetProducts(hideOutOfStock: false);
            var resultWithFilter = controller.GetProducts(hideOutOfStock: true);
            
            // Assert
            Assert.True(resultWithoutFilter.Count >= resultWithFilter.Count);
        }
        
        [Fact]
        public void GetProducts_OutOfStockItemsAppearAtBottom()
        {
            // Arrange
            var controller = new CatalogController();
            
            // Act
            var result = controller.GetProducts();
            
            // Assert
            bool foundOutOfStock = false;
            foreach (var product in result)
            {
                if (product.IsOutOfStock)
                {
                    foundOutOfStock = true;
                }
                else if (foundOutOfStock)
                {
                    // If we found an in-stock item after an out-of-stock item, fail
                    Assert.Fail("In-stock items should appear before out-of-stock items");
                }
            }
        }
        
        [Fact]
        public void GetProducts_WithSearchTerm_FiltersCorrectly()
        {
            // Arrange
            var controller = new CatalogController();
            
            // Act
            var result = controller.GetProducts(searchTerm: "Product 1");
            
            // Assert
            Assert.NotNull(result);
            Assert.All(result, product => 
                Assert.True(
                    product.Name.Contains("Product 1", System.StringComparison.OrdinalIgnoreCase) ||
                    product.Description.Contains("Product 1", System.StringComparison.OrdinalIgnoreCase)
                )
            );
        }
        
        [Fact]
        public void GetProducts_WithSearchTermAndHideOutOfStock_AppliesBothFilters()
        {
            // Arrange
            var controller = new CatalogController();
            
            // Act
            var result = controller.GetProducts(searchTerm: "Product", hideOutOfStock: true);
            
            // Assert
            Assert.NotNull(result);
            Assert.All(result, product => 
            {
                Assert.False(product.IsOutOfStock);
                Assert.True(
                    product.Name.Contains("Product", System.StringComparison.OrdinalIgnoreCase) ||
                    product.Description.Contains("Product", System.StringComparison.OrdinalIgnoreCase)
                );
            });
        }
        
        [Fact]
        public void IsOutOfStock_ReturnsTrueWhenStockIsZero()
        {
            // Arrange
            var product = new Product { AvailableStock = 0 };
            
            // Act & Assert
            Assert.True(product.IsOutOfStock);
        }
        
        [Fact]
        public void IsOutOfStock_ReturnsFalseWhenStockIsPositive()
        {
            // Arrange
            var product = new Product { AvailableStock = 5 };
            
            // Act & Assert
            Assert.False(product.IsOutOfStock);
        }
    }
}
