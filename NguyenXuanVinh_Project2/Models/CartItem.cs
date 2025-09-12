namespace NguyenXuanVinh_Project2.Models
{
    public class CartItem
    {
        // ID có thể null trong lúc khởi tạo, EF sẽ set sau
        public string? BookId { get; set; }

        // Navigation property cũng cho phép null
        public Book? Book { get; set; }

        // Quantity mặc định = 1
        public int Quantity { get; set; } = 1;
    }
}
