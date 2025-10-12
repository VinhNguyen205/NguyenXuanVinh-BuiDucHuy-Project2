namespace NguyenXuanVinh_Project2.Models;
using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
    [Display(Name = "Tên đăng nhập")]
    public string Username { get; set; } = string.Empty; // <-- THÊM VÀO ĐÂY

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = string.Empty; // <-- VÀ THÊM VÀO ĐÂY

    [Display(Name = "Ghi nhớ tôi?")]
    public bool RememberMe { get; set; }
}
