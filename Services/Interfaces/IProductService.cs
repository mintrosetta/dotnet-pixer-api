﻿using PixerAPI.Models;

namespace PixerAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task ChangeToSoldOutAsync(Product product);
        Task<Product> CreateProductAsync(Product product);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<List<Product>> GetProductsAsync();
    }
}
