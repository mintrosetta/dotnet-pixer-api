using Microsoft.EntityFrameworkCore;
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
    }
}
