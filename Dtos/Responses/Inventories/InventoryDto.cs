using PixerAPI.Dtos.Responses.Products;
using PixerAPI.Dtos.Responses.Users;

namespace PixerAPI.Dtos.Responses.Inventories
{
    public class InventoryDto : ShortInventoryDto
    {
        public decimal Price { get; set; }
        public List<ProductAgreementDto> Agreements { get; set; }
        public DateTime BuyDate { get; set; }
        public ProductOwner Owner { get; set; }
    }
}
