using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenXuanVinh_Project2.Models;

[Table("OrderDetail")]
public partial class OrderDetail
{
    [Key]
    public int OrderDetailId { get; set; }

    [StringLength(16)]
    [Unicode(false)]
    public string? OrderId { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? BookId { get; set; }

    public int? Quantity { get; set; }

    public int? Price { get; set; }

    public int? TotalMoney { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("OrderDetails")]
    public virtual Book? Book { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual OrderBook? Order { get; set; }
}
