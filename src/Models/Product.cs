namespace EShopOnWeb.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableStock { get; set; }
        public string ImageUrl { get; set; }
        
        public bool IsOutOfStock => AvailableStock <= 0;
    }
}
