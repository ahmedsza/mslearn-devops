using System.Collections.Generic;
using EShopOnWeb.Models;

namespace EShopOnWeb.ViewModels
{
    public class CatalogSearchViewModel
    {
        public List<Product> Products { get; set; }
        public string SearchTerm { get; set; }
        public bool HideOutOfStock { get; set; }
    }
}
