using PixerAPI.Contexts;
using PixerAPI.Models;
using PixerAPI.Repositories.Interfaces;

namespace PixerAPI.Repositories
{
    public class UserInventoryRepository : Repository<UserInventory>, IUserInventoryRepository
    {
        public UserInventoryRepository(MySQLDbContext mySQLDbContext) : base(mySQLDbContext)
        {
        }
    }
}
