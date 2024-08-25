using PixerAPI.Models;

namespace PixerAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> FindByIdAsync(int id);
        Task<User?> FindByEmailAsync(string email);
        Task<bool> IsExistEmail(string email);
        Task<bool> IsExistUsername(string username);
        Task<bool> IsLockAsync(int? userId);

        Task CreateAsync(User user);

    }
}
