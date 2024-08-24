using PixerAPI.Services.Interfaces;

namespace PixerAPI.UnitOfWorks.Interfaces
{
    public interface IServiceUnitOfWork
    {
        public IJwtService JwtService { get; }
        public IUserService UserService { get; }
        public IRoleService RoleService { get; }
    }
}
