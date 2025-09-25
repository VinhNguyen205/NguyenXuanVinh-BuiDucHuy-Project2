// Models/CartItem.cs
using System;

namespace NguyenXuanVinh_Project2.Models
{
    public class CartItem
    {
        public string? BookId { get; set; }          // ✅ Đổi từ string sang int
        public Book Book { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
