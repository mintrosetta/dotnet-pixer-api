using PixerAPI.Contexts;
using PixerAPI.Models;
using PixerAPI.Repositories.Interfaces;

namespace PixerAPI.Repositories
{
    public class ArgrementRepository : Repository<Argrement>, IArgrementRepository
    {
        public ArgrementRepository(MySQLDbContext mySQLDbContext) : base(mySQLDbContext)
        {
        }
    }
}
