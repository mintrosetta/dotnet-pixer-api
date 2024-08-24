using Microsoft.EntityFrameworkCore;
using PixerAPI.Contexts;
using PixerAPI.Models;
using PixerAPI.Repositories.Interfaces;

namespace PixerAPI.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MySQLDbContext mySQLDbContext) : base(mySQLDbContext)
        {

        }

        public async Task<bool> AnyByEmail(string email)
        {
            return await this.dbContext.Users.AnyAsync(user => user.Email == email);
        }

        public async Task<bool> AnyByUsername(string username)
        {
            return await this.dbContext.Users.AnyAsync(user => user.Username == username);
        }
    }
}
