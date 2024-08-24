using PixerAPI.Models;

namespace PixerAPI.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> AnyByEmail(string email);
        Task<bool> AnyByUsername(string username);
    }
}
