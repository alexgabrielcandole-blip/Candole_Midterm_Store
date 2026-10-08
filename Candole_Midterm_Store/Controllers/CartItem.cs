using System.ComponentModel.DataAnnotations;

namespace Candole_Midterm_Store.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        [Range(1, 999)]
        public int Quantity { get; set; }
    }
}