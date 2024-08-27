using Microsoft.EntityFrameworkCore;
using PixerAPI.Dtos.Responses.Users;
using PixerAPI.Models;
using PixerAPI.Services.Interfaces;
using PixerAPI.UnitOfWorks.Interfaces;

namespace PixerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IRepoUnitOfWork repoUnitOfWork;

        public UserService(IRepoUnitOfWork repoUnitOfWork)
        {
            this.repoUnitOfWork = repoUnitOfWork;
        }

        public async Task<User?> FindByIdAsync(int id)
        {
            return await this.repoUnitOfWork.UserRepository.FindById(id);
        }

        public async Task CreateAsync(User user)
        {
            await this.repoUnitOfWork.UserRepository.Add(user);
            await this.repoUnitOfWork.CompleteAsync();
        }

        public async Task<bool> IsExistEmail(string email)
        {
            return await this.repoUnitOfWork.UserRepository.AnyByEmail(email);
        }

        public async Task<bool> IsExistUsername(string username)
        {
            return await this.repoUnitOfWork.UserRepository.AnyByUsername(username);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await this.repoUnitOfWork.UserRepository.Find((user) => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> IsLockAsync(int? userId)
        {
            if (userId == null) return false;

            return false;
        }

        public async Task DeductMoneyAsync(User user, decimal price)
        {
            user.Money = user.Money - price;

            if (user.Money < 0) throw new Exception("The user money is not sufficient for the deduction");

            this.repoUnitOfWork.UserRepository.Update(user);
            await this.repoUnitOfWork.CompleteAsync();
        }

        public async Task AppendToInventory(User user, Product product)
        {
            await this.repoUnitOfWork.UserInventoryRepository.Add(new UserInventory()
            {
                ProductId = product.Id,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });

            await this.repoUnitOfWork.CompleteAsync();
        }

        public async Task AddMoneyAsync(User owner, decimal price)
        {
            owner.Money = owner.Money + price;

            this.repoUnitOfWork.UserRepository.Update(owner);
            await this.repoUnitOfWork.CompleteAsync();
        }

        public async Task<List<ShortInventoryDto>> GetShortInventoriesByUserIdAsync(int userId)
        {
            DbSet<Product> dbSetProduct = this.repoUnitOfWork.ProductRepository.DbSet;
            DbSet<UserInventory> dbSetUserInventory = this.repoUnitOfWork.UserInventoryRepository.DbSet;

            // join table UserInventory and Product, Select with ShortInventoryModel
            return await dbSetUserInventory
                .Where((userInventory) => userInventory.UserId == userId).Join(
                dbSetProduct,
                userInventory => userInventory.ProductId,
                product => product.Id,
                (userInventory, product) => new ShortInventoryDto()
                {
                    Id = userInventory.Id,
                    Image = product.Image,
                })
                .ToListAsync();
        }
    }
}
