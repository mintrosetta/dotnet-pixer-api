using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixerAPI.Models
{
    [Table("argrements")]
    public class Argrement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public required int Id { get; set; }

        [Column("name")]
        public required string Name { get; set; }
    }
}
