using PixerAPI.Contexts;
using PixerAPI.Repositories;
using PixerAPI.Repositories.Interfaces;
using PixerAPI.UnitOfWorks.Interfaces;

namespace PixerAPI.UnitOfWorks
{
    public class RepoUnitOfWork : IRepoUnitOfWork
    {
        private readonly MySQLDbContext dbContext;

        public RepoUnitOfWork(MySQLDbContext dbContext)
        {
            this.dbContext = dbContext;

            this.UserRepository = new UserRepository(dbContext);
            this.RoleRepository = new RoleRepository(dbContext);
            this.ProductRepository = new ProductRepository(dbContext);
        }

        public IUserRepository UserRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }

        public async Task CompleteAsync()
        {
            await this.dbContext.SaveChangesAsync();
        }
    }
}
