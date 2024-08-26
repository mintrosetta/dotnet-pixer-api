using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PixerAPI.Contexts;
using PixerAPI.Repositories.Interfaces;

namespace PixerAPI.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly MySQLDbContext dbContext;

    public DbSet<T> DbSet { get; private set; }

    public Repository(MySQLDbContext mySQLDbContext)
    {
        this.dbContext = mySQLDbContext;
        this.DbSet = mySQLDbContext.Set<T>();
    }

    public IQueryable<T> Find(Expression<Func<T, bool>> expression)
    {
        try 
        {
            return this.DbSet.Where(expression);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<T?> FindById<N>(N id)
    {
        try 
        {
            return await this.DbSet.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    public async Task Add(T entity)
    {
        try 
        {
            await this.DbSet.AddAsync(entity);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task AddRange(IEnumerable<T> entities)
    {
        try 
        {
            await this.DbSet.AddRangeAsync(entities);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void Update(T entity)
    {
        try 
        {
            this.DbSet.Update(entity);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void Remove(T entity)
    {
        try 
        {
            this.DbSet.Remove(entity);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        try 
        {
            this.DbSet.RemoveRange(entities);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
