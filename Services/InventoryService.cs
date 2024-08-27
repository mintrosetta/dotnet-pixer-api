using PixerAPI.Models;
using PixerAPI.Services.Interfaces;
using PixerAPI.UnitOfWorks.Interfaces;

namespace PixerAPI.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepoUnitOfWork repoUnitOfWork;

        public InventoryService(IRepoUnitOfWork repoUnitOfWork)
        {
            this.repoUnitOfWork = repoUnitOfWork;
        }

        public async Task<UserInventory?> GetInventoryByIdAsync(int id)
        {
            return await this.repoUnitOfWork.UserInventoryRepository.FindById(id);
        }
    }
}
