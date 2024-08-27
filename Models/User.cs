using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixerAPI.Models;

[Table("users", Schema = "pixer_database")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("role_id")]
    public required int RoleId { get; set; }

    [Column("email")]
    public required string Email { get; set; }

    [Column("username")]
    public required string Username { get; set; }
    
    [Column("password")]
    public required string Password { get; set; }

    [Column("profile_image")]
    public byte[]? ProfileImage { get; set; }

    [Column("money")]
    public decimal Money { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [ForeignKey("RoleId")]
    public Role Role { get; set; }

    public ICollection<Product>? Products { get; set; }
}
