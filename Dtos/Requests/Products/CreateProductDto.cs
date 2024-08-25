using System.ComponentModel.DataAnnotations;

namespace PixerAPI.Dtos.Requests.Products
{
    public class CreateProductDto
    {
        [Required]
        public required IFormFile Image { get; set; }

        [Required]
        public required decimal Price { get; set; }

        [Required]
        public required List<int> Agreement { get; set; }

        [Required]
        public required string Description { get; set; }
    }
}
