using PixerAPI.Models;

namespace PixerAPI.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<UserInventory?> GetInventoryByIdAsync(int id);
    }
}
