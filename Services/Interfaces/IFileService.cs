namespace PixerAPI.Services.Interfaces
{
    public interface IFileService
    {
        bool IsImageFile(IFormFile file);
        Task<byte[]> ToBytesAsync(IFormFile image);
    }
}
