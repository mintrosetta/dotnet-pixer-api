using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixerAPI.Models
{
    [Table("product_argrements")]
    public class ProductArgrement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public required int Id { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("argrement_id")]
        public int ArgrementId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("ArgrementId")]
        public Argrement Argrement { get; set; }
    }
}
