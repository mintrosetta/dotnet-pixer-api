using PixerAPI.Models;

namespace PixerAPI.Repositories.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> FindByName(string name);
}
