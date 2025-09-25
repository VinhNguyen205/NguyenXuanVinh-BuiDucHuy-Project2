using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenXuanVinh_Project2.Models;

[Table("OrderBook")]
public partial class OrderBook
{
    [Key]
    [StringLength(16)]
    [Unicode(false)]
    public string OrderId { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [StringLength(36)]
    [Unicode(false)]
    public string? AccountId { get; set; }

    [StringLength(512)]
    public string? ReceiveAddress { get; set; }

    [StringLength(64)]
    [Unicode(false)]
    public string? ReceivePhone { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderReceive { get; set; }

    [StringLength(512)]
    public string? Note { get; set; }

    [StringLength(16)]
    [Unicode(false)]
    public string? Status { get; set; }

    [ForeignKey(nameof(AccountId))]
    [InverseProperty(nameof(Account.OrderBooks))]
    public virtual Account? Account { get; set; }

    // 🔑 Đồng bộ với OrderDetail.OrderBook
    [InverseProperty(nameof(OrderDetail.OrderBook))]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
