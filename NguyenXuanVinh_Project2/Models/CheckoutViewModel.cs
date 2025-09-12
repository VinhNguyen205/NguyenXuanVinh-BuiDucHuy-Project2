using System.ComponentModel.DataAnnotations;

namespace NguyenXuanVinh_Project2.Models
{
    public class CheckoutViewModel
    {
        [Required, Display(Name = "Họ tên")]
        public string CustomerName { get; set; } = string.Empty;

        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required, Display(Name = "Địa chỉ")]
        public string Address { get; set; } = string.Empty;
    }
}
