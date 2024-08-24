using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixerAPI.Models
{
    [Table("user_inventories")]
    public class UserInventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public required int Id { get; set; }

        [Column("user_id")]
        public required int UserId { get; set; }

        [Column("product_id")]
        public required int ProductId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
