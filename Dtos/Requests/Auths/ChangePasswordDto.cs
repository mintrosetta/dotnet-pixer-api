using System.ComponentModel.DataAnnotations;

namespace PixerAPI.Dtos.Requests.Auths
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [MinLength(8, ErrorMessage = "New password should more than 8 character")]
        public string NewPassword { get; set; }
    }
}
