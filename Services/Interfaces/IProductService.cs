using PixerAPI.Models;

namespace PixerAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
    }
}
