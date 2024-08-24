using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PixerAPI.Contexts;
using PixerAPI.Repositories.Interfaces;

namespace PixerAPI.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly MySQLDbContext dbContext;
    protected DbSet<T> dbSet;

    public Repository(MySQLDbContext mySQLDbContext)
    {
        this.dbContext = mySQLDbContext;
        this.dbSet = mySQLDbContext.Set<T>();
    }

    public IQueryable<T> Find(Expression<Func<T, bool>> expression)
    {
        try 
        {
            return this.dbSet.Where(expression);
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
            return await this.dbSet.FindAsync(id);
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
            await this.dbSet.AddAsync(entity);
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
            await this.dbSet.AddRangeAsync(entities);
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
            this.dbSet.Update(entity);
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
            this.dbSet.Remove(entity);
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
            this.dbSet.RemoveRange(entities);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
