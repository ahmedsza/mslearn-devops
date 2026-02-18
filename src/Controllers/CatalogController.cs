using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using EShopOnWeb.Models;
using EShopOnWeb.ViewModels;

namespace EShopOnWeb.Controllers
{
    public class CatalogController : Controller
    {
        // Mock data - in real app this would come from a database
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.99m, AvailableStock = 5, ImageUrl = "/images/product1.jpg" },
            new Product { Id = 2, Name = "Product 2", Description = "Description 2", Price = 20.99m, AvailableStock = 0, ImageUrl = "/images/product2.jpg" },
            new Product { Id = 3, Name = "Product 3", Description = "Description 3", Price = 30.99m, AvailableStock = 10, ImageUrl = "/images/product3.jpg" },
            new Product { Id = 4, Name = "Product 4", Description = "Description 4", Price = 40.99m, AvailableStock = 0, ImageUrl = "/images/product4.jpg" },
            new Product { Id = 5, Name = "Product 5", Description = "Description 5", Price = 50.99m, AvailableStock = 3, ImageUrl = "/images/product5.jpg" }
        };

        public IActionResult Index(string searchTerm = "", bool hideOutOfStock = false)
        {
            var products = GetProducts(searchTerm, hideOutOfStock);
            
            var viewModel = new CatalogSearchViewModel
            {
                Products = products,
                SearchTerm = searchTerm,
                HideOutOfStock = hideOutOfStock
            };
            
            return View(viewModel);
        }

        /// <summary>
        /// Gets products based on search term and filter options.
        /// Out-of-stock items are sorted to appear at the bottom of results.
        /// </summary>
        /// <param name="searchTerm">Search term to filter products</param>
        /// <param name="hideOutOfStock">Whether to hide out-of-stock items</param>
        /// <returns>List of products matching the criteria</returns>
        public List<Product> GetProducts(string searchTerm = "", bool hideOutOfStock = false)
        {
            var query = _products.AsEnumerable();
            
            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => 
                    p.Name.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase));
            }
            
            // Apply out-of-stock filter
            if (hideOutOfStock)
            {
                query = query.Where(p => !p.IsOutOfStock);
            }
            
            // Sort: in-stock items first, then out-of-stock items
            query = query.OrderBy(p => p.IsOutOfStock).ThenBy(p => p.Name);
            
            return query.ToList();
        }
    }
}
