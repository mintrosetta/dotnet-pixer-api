using Microsoft.EntityFrameworkCore;
using PixerAPI.Models;

namespace PixerAPI.Contexts;

public class MySQLDbContext : DbContext
{
    public MySQLDbContext(DbContextOptions<MySQLDbContext> option) : base(option)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Agreement> Argrements { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<UserInventory> UserInventories { get; set; }
}
