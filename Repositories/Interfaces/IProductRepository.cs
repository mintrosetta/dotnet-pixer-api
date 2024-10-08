﻿using PixerAPI.Models;

namespace PixerAPI.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllAsync();
    }
}
