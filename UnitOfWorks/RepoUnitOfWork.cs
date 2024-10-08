﻿using PixerAPI.Contexts;
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
            this.ArgrementRepository = new ArgrementRepository(dbContext);
            this.ProductArgrementRepository = new ProductArgrementRepository(dbContext);
            this.ShoppingCartRepository = new ShoppingCartRepository(dbContext);
            this.UserInventoryRepository = new UserInventoryRepository(dbContext);
        }

        public IUserRepository UserRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public IArgrementRepository ArgrementRepository { get; private set; }
        public IProductArgrementRepository ProductArgrementRepository { get; private set; }
        public IShoppingCartRepository ShoppingCartRepository { get; private set; }
        public IUserInventoryRepository UserInventoryRepository { get; private set; }

        public async Task CompleteAsync()
        {
            await this.dbContext.SaveChangesAsync();
        }
    }
}
