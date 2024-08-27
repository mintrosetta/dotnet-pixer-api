using Microsoft.EntityFrameworkCore;
using PixerAPI.Dtos.Responses.Products;
using PixerAPI.Dtos.Responses.Users;
using PixerAPI.Models;
using PixerAPI.Services.Interfaces;
using PixerAPI.UnitOfWorks.Interfaces;

namespace PixerAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepoUnitOfWork repoUnitOfWork;

        public ProductService(IRepoUnitOfWork repoUnitOfWork)
        {
            this.repoUnitOfWork = repoUnitOfWork;
        }

        public async Task ChangeToSoldOutAsync(Product product)
        {
            product.IsSoldOut = true;

            this.repoUnitOfWork.ProductRepository.Update(product);
            await this.repoUnitOfWork.CompleteAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = DateTime.Now;

                await this.repoUnitOfWork.ProductRepository.Add(product);
                await this.repoUnitOfWork.CompleteAsync();

                return product;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await this.repoUnitOfWork.ProductRepository.FindById(productId);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                List<Product> products = await this.repoUnitOfWork.ProductRepository.GetAllAsync();

                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProfileProductDto>> GetShortProductByUserIdAsync(int id)
        {
            return await this.repoUnitOfWork.ProductRepository.Find((product) => product.UserId == id)
                .OrderByDescending((product) => product.CreatedAt)
                .Select((product) => new ProfileProductDto()
                {
                    Id = product.Id,
                    Image = product.Image
                })
                .ToListAsync();
        }
    }
}
