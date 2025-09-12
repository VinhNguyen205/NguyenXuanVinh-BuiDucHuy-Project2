using System.Collections.Generic;

namespace NguyenXuanVinh_Project2.Models
{
    public class CheckoutResult
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
