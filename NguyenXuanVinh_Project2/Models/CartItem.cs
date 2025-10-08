// Models/CartItem.cs
using System;

namespace NguyenXuanVinh_Project2.Models
{
    public class CartItem
    {
        // BookId là string (khớp với Book.BookId)
        public string? BookId { get; set; }
        public Book? Book { get; set; }

        // GiftId là int (khớp với Gift.GiftId)
        public int? GiftId { get; set; }
        public Gift? Gift { get; set; }

        public int Quantity { get; set; }

        // Tính tổng tiền (chú ý convert tùy kiểu Price của Book/Gift)
        public decimal TotalPrice
        {
            get
            {
                if (Book != null)
                {
                    // Book.Price có thể là float/double -> convert an toàn
                    var price = Convert.ToDecimal(Book.Price);
                    return price * Quantity;
                }
                if (Gift != null)
                {
                    var price = Convert.ToDecimal(Gift.Price);
                    return price * Quantity;
                }
                return 0m;
            }
        }
    }
}
