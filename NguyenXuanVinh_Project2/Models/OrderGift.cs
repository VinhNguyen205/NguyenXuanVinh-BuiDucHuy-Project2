using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenXuanVinh_Project2.Models;

[Table("OrderGift")]
public partial class OrderGift
{
    [Key]
    public int OrderGiftId { get; set; }

    [StringLength(16)]
    [Unicode(false)]
    public string OrderId { get; set; } = null!;

    public int GiftId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal TotalMoney { get; set; }

    // 🔑 Quan hệ với OrderBook
    [ForeignKey(nameof(OrderId))]
    [InverseProperty(nameof(OrderBook.OrderGifts))]
    public virtual OrderBook OrderBook { get; set; } = null!;

    // 🔑 Quan hệ với Gift
    [ForeignKey(nameof(GiftId))]
    [InverseProperty(nameof(Gift.OrderGifts))]
    public virtual Gift Gift { get; set; } = null!;
}
