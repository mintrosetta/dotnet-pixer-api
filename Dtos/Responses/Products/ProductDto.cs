namespace PixerAPI.Dtos.Responses.Products
{
    public class ProductDto : ShortProductDto
    {
        public string? Description { get; set; }
        public ProductOwner Owner { get; set; }
    }
}
