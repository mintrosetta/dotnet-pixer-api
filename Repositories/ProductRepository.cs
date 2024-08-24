using PixerAPI.Contexts;
using PixerAPI.Models;
using PixerAPI.Repositories.Interfaces;

namespace PixerAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MySQLDbContext mySQLDbContext) : base(mySQLDbContext)
        {
        }
    }
}
