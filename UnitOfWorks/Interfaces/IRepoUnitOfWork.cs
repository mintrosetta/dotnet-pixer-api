using PixerAPI.Repositories.Interfaces;

namespace PixerAPI.UnitOfWorks.Interfaces
{
    public interface IRepoUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IProductRepository ProductRepository { get; }

        Task CompleteAsync();
    }
}
