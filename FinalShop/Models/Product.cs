using System.ComponentModel.DataAnnotations;

namespace FinalShop.Models
{
    public class Product
    {
        public int productID { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string productName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters")]
        public string productDescription { get; set; } = string.Empty;

        [Range(0.01, 9999.99, ErrorMessage = "Price must be between 0.01 and 9999.99")]
        public decimal Price { get; set; }

        [Range(0, 10000, ErrorMessage = "Quantity must be between 0 and 10000")]
        public int quantity { get; set; }
    }
}