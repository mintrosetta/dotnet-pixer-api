using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixerAPI.Models
{
    [Table("argrements")]
    public class Agreement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public required int Id { get; set; }

        [Column("name")]
        public required string Name { get; set; }

        public ICollection<ProductAgreement>? ProductArgrements { get; set; }
    }
}
