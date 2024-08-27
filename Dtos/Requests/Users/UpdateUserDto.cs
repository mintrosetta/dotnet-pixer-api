namespace PixerAPI.Dtos.Requests.Users
{
    public class UpdateUserDto
    {
        public IFormFile? ProfileImage { get; set; }
        public string? Username { get; set; }
        public string? Description { get; set; }
    }
}
