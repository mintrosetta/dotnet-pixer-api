using PixerAPI.Services;
using PixerAPI.Services.Interfaces;
using PixerAPI.UnitOfWorks.Interfaces;

namespace PixerAPI.UnitOfWorks
{
    public class ServiceUnitOfWork : IServiceUnitOfWork
    {
        public ServiceUnitOfWork(IRepoUnitOfWork repoUnitOfWork, IConfiguration configuration)
        {
            this.JwtService = new JwtService(configuration);
            this.UserService = new UserService(repoUnitOfWork);
            this.RoleService = new RoleService(repoUnitOfWork);
        }

        public IJwtService JwtService { get; private set; }
        public IUserService UserService { get; private set; }
        public IRoleService RoleService { get; private set; }
    }
}
