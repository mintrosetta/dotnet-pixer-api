using System.ComponentModel.DataAnnotations;

namespace PixerAPI.Dtos.Requests.Auths
{
    public class AuthenticateDto
    {
        [Required(ErrorMessage = "Email is require")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
