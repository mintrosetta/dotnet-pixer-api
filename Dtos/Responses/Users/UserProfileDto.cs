namespace PixerAPI.Dtos.Responses.Users
{
    public class UserProfileDto
    {
        public byte[]? ProfileImage { get; set; }
        public string Username { get; set; }
        public string? Description { get; set; }
    }
}
