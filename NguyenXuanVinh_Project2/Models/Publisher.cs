using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenXuanVinh_Project2.Models;

[Table("Publisher")]
public partial class Publisher
{
    [Key]
    public int PublisherId { get; set; }

    [Required(ErrorMessage = "Tên nhà xuất bản không được để trống")]
    [StringLength(200)]
    public string PublisherName { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [InverseProperty("Publisher")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
