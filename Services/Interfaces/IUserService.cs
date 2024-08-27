using PixerAPI.Dtos.Responses.Users;
using PixerAPI.Models;

namespace PixerAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> FindByIdAsync(int id);
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByUsernameAsync(string username);

        Task<bool> IsExistEmail(string email);
        Task<bool> IsExistUsername(string username);
        Task<bool> IsLockAsync(int? userId);

        Task CreateAsync(User user);
        Task DeductMoneyAsync(User buyer, decimal price);
        Task AppendToInventory(User buyer, Product product);
        Task AddMoneyAsync(User owner, decimal price);
        Task<List<ShortInventoryDto>> GetShortInventoriesByUserIdAsync(int v);
        Task UpdateAsync(User user);
    }
}
