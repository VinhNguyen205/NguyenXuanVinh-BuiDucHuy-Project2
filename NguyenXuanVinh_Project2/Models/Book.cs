using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenXuanVinh_Project2.Models;

[Table("Book")]
public partial class Book
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string BookId { get; set; } = null!;

    [Required(ErrorMessage = "Tên sách không được để trống")]
    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(100)]
    public string? Author { get; set; }

    public int? Release { get; set; }

    public double? Price { get; set; }

    [Column(TypeName = "ntext")]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Picture { get; set; }

    public int? PublisherId { get; set; }
    public int? CategoryId { get; set; }

    // ================== PROPERTIES CHO VIEW ==================

    /// <summary>Ảnh hiển thị</summary>
    [NotMapped]
    public string? ImageUrl
    {
        get => Picture;
        set => Picture = value;
    }

    /// <summary>Năm phát hành</summary>
    [NotMapped]
    public int? ReleaseYear
    {
        get => Release;
        set => Release = value;
    }

    /// <summary>Tên thể loại (lấy từ Category)</summary>
    [NotMapped]
    public string? Genre
    {
        get => Category?.CategoryName;
    }

    /// <summary>Tên nhà xuất bản</summary>
    [NotMapped]
    public string? PublisherName
    {
        get => Publisher?.PublisherName;
    }

    // ================== NAVIGATION PROPERTIES ==================

    [ForeignKey("CategoryId")]
    [InverseProperty("Books")]
    public virtual Category? Category { get; set; }

    [ForeignKey("PublisherId")]
    [InverseProperty("Books")]
    public virtual Publisher? Publisher { get; set; }

    [InverseProperty("Book")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
