using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PartOne.Application.Common.Interfaces;
using PartOne.Infrastructure.Data;

namespace PartOne.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T: class
{
    private readonly DbSet<T> _dbSet;
    public Repository(ApplicationDbContext db)
    {
        _dbSet = db.Set<T>();
    }
    
    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false)
    {
        IQueryable<T> query;
        
        if (tracked)
        {
            query = _dbSet;
        }
        else
        {
            query = _dbSet.AsNoTracking();
        }
        
        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties
                         .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp.Trim());
            }
        }
        return await query.ToListAsync();
    }

    public async Task<T> Get(Expression<Func<T, bool>>? filter, string? includeProperties = null, bool tracked = false)
    {
        IQueryable<T> query;
        
        if (tracked)
        {
            query = _dbSet;
        }
        else
        {
            query=_dbSet.AsNoTracking();
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (!string.IsNullOrEmpty(includeProperties))
        {
            //Villa,VillaNumber -- case sensitive
            foreach (var includeProp in includeProperties
                         .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp.Trim());
            }
        }
        return await query.FirstOrDefaultAsync();
    }

    public async Task Add(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public bool Any(Expression<Func<T, bool>> filter)
    {
        return _dbSet.Any(filter);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
}