using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenXuanVinh_Project2.Models;

[Table("Account")]
public partial class Account
{
    [Key]
    [StringLength(36)]
    [Unicode(false)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)] // không để Identity vì string không auto tăng
    public string AccountId { get; set; } = Guid.NewGuid().ToString();

    [StringLength(64)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(256)]
    [Unicode(false)]
    public string? Password { get; set; }

    [StringLength(100)]
    public string? FullName { get; set; }

    [StringLength(512)]
    [Unicode(false)]
    public string? Picture { get; set; }

    [StringLength(64)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(512)]
    public string? Address { get; set; }

    [StringLength(64)]
    [Unicode(false)]
    public string? Phone { get; set; }

    public bool? Active { get; set; } = true;

    // 🔑 Role: "Admin", "Member", "Guest"
    [StringLength(20)]
    [Unicode(false)]
    public string Role { get; set; } = "Member";

    [InverseProperty("Account")]
    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();
}
