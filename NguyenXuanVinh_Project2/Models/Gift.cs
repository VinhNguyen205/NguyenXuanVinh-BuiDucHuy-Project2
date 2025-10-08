using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenXuanVinh_Project2.Models
{
    [Table("Gift")]
    public partial class Gift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GiftId { get; set; }   // INT IDENTITY trong SQL

        [Required]
        [StringLength(200)]
        public string GiftName { get; set; } = null!;

        public double? Price { get; set; }  // FLOAT trong SQL => double trong C#

        public string? Description { get; set; }  // NTEXT => string

        [StringLength(200)]
        public string? Picture { get; set; }

        public int? Stock { get; set; }

        [InverseProperty(nameof(OrderGift.Gift))]
        public virtual ICollection<OrderGift> OrderGifts { get; set; } = new List<OrderGift>();
    }
}
