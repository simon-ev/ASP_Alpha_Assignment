
using System.Diagnostics;
using System.Linq.Expressions;
using Data.Contexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _table;


    protected BaseRepository(AppDbContext context)
    {
        _context = context;
        _table = context.Set<TEntity>();
    }

    public virtual async Task<RepositoryResult<bool>> AddAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity cannot be null" };

        try
        {
            _table.Add(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }




    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync
        (
        bool orderByDescending = false,
        Expression<Func<TEntity, bool>>? where = null,
        Expression<Func<TEntity, object>>? sortBy = null,
        params Expression<Func<TEntity, object>>[]? includes)

    {
        IQueryable<TEntity> query = _table;
        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var entities = await query.ToListAsync();
        return new RepositoryResult<IEnumerable<TEntity>> { Succeeded = true, StatusCode = 200, Result = entities };
    }



    public virtual async Task<RepositoryResult<TEntity>> GetAsync
        (
        bool orderByDescending = false,
        Expression<Func<TEntity, bool>> where,
        Expression<Func<TEntity, object>>? sortBy = null,
        params Expression<Func<TEntity, object>>[]? includes)

    {

        IQueryable<TEntity> query = _table;
        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = await query.FirstOrDefaultAsync(where);
        if (entity == null)
            return new RepositoryResult<TEntity> { Succeeded = false, StatusCode = 404, Error = "Entity not found." };


        //var entity = await _table.FirstOrDefaultAsync(findBy);
        return entity == null
            ? new RepositoryResult<TEntity> { Succeeded = false, StatusCode = 404, Error = "Entity not found." }
            : new RepositoryResult<TEntity> { Succeeded = true, StatusCode = 200, Result = entity };
    }




    public virtual async Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> findBy)
    {
        var exists = await _table.AnyAsync(findBy);
        return !exists
            ? new RepositoryResult<bool> { Succeeded = false, StatusCode = 404, Error = "Entity not found." }
            : new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
    }





    public virtual async Task<RepositoryResult<bool>> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity cannot be null" };

        try
        {
            _table.Update(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepositoryResult<bool>> DeleteAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity cannot be null" };

        try
        {
            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }
}
