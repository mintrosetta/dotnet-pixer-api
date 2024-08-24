using Microsoft.EntityFrameworkCore;
using PixerAPI.Contexts;
using PixerAPI.Models;
using PixerAPI.Repositories.Interfaces;

namespace PixerAPI.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(MySQLDbContext mySQLDbContext) : base(mySQLDbContext)
    {
    }

    public async Task<Role?> FindByName(string name)
    {
        return await this.dbContext.Roles.Where(role => role.Name.Equals(name)).FirstOrDefaultAsync();
    }
}
