namespace PixerAPI.Dtos.Responses.Products
{
    public class ProductDto
    {
        public byte[] Image { get; set; }
        public decimal Price { get; set; }
        public List<ProductAgreementDto> Agreements { get; set; } = new List<ProductAgreementDto>();
    }
}
