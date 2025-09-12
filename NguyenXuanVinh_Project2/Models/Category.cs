using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NguyenXuanVinh_Project2.Models;

[Table("Category")]
public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Tên thể loại không được để trống")]
    [StringLength(100)]
    public string CategoryName { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
