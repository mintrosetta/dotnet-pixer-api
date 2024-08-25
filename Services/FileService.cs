using PixerAPI.Services.Interfaces;

namespace PixerAPI.Services
{
    public class FileService : IFileService
    {
        public bool IsImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0) return false;

            string mimeType = file.ContentType.ToLower();
            if (mimeType != "image/jpeg" && mimeType != "image/png" && mimeType != "image/gif") return false;

            try
            {
                using Stream stream = file.OpenReadStream();
                using BinaryReader reader = new BinaryReader(stream);
                var fileHeader = reader.ReadBytes(4);

                bool isHeaderJPEG = (fileHeader[0] == 0xFF && fileHeader[1] == 0xD8 && fileHeader[2] == 0xFF);
                bool isHeaderPNG = (fileHeader[0] == 0x89 && fileHeader[1] == 0x50 && fileHeader[2] == 0x4E && fileHeader[3] == 0x47);
                bool isHeaderGIF = (fileHeader[0] == 0x47 && fileHeader[1] == 0x49 && fileHeader[2] == 0x46);

                return isHeaderJPEG || isHeaderPNG || isHeaderGIF;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public async Task<byte[]> ToBytesAsync(IFormFile image)
        {
            await using MemoryStream stream = new MemoryStream();
            await image.CopyToAsync(stream);
            return stream.ToArray();
        }
    }
}
