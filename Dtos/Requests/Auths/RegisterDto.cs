using System.ComponentModel.DataAnnotations;

namespace PixerAPI.Dtos.Requests.Auths
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is require")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Email is require")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is require")]
        public required string Password { get; set; }
    }
}
