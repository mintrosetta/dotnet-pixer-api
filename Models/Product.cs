using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixerAPI.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public required int Id { get; set; }

        [Column("user_id")]
        public required int UserId { get; set; }

        [Column("image")]
        public required byte[] image { get; set; }

        [Column("price")]
        public required decimal Price { get; set; }

        [Column("is_sold_out")]
        public required bool IsSoldOut { get; set; }

        [Column("created_at")]
        public required DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public required DateTime UpdatedAt { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
