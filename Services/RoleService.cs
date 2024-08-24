using PixerAPI.Models;
using PixerAPI.Services.Interfaces;
using PixerAPI.UnitOfWorks.Interfaces;

namespace PixerAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepoUnitOfWork repoUnitOfWork;

        public RoleService(IRepoUnitOfWork repoUnitOfWork)
        {
            this.repoUnitOfWork = repoUnitOfWork;
        }

        public async Task<int> GetRoleIdAdminAsync()
        {
            Role? role = await this.repoUnitOfWork.RoleRepository.FindByName("admin");

            if (role == null) throw new Exception("Role 'admin' not found");

            return role.Id;
        }

        public async Task<int> GetRoleIdUserAsync()
        {
            Role? role = await this.repoUnitOfWork.RoleRepository.FindByName("user");

            if (role == null) throw new Exception("Role 'user' not found");

            return role.Id;
        }
    }
}
