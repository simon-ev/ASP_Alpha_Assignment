
//using System.Diagnostics;
//using System.Linq.Expressions;
//using Data.Contexts;
//using Data.Entities;
//using Data.Models;
//using Microsoft.EntityFrameworkCore;

//namespace Data.Repositories;

//public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
//{
//    protected readonly AppDbContext _context;
//    protected readonly DbSet<TEntity> _table;

//    protected BaseRepository(AppDbContext context)
//    {
//        _context = context;
//        _table = context.Set<TEntity>();
//    }

//    public virtual async Task<RepositoryResult<TEntity>> GetByIdAsync(int id)
//    {
//        var entity = await _table.FindAsync(id);
//        return entity == null
//            ? new RepositoryResult<TEntity> { Succeeded = false, StatusCode = 404, Error = "Entity not found." }
//            : new RepositoryResult<TEntity> { Succeeded = true, StatusCode = 200, Result = entity };
//    }

//    public virtual async Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync()
//    {
//        var entities = await _table.ToListAsync();
//        return new RepositoryResult<IEnumerable<TEntity>> { Succeeded = true, StatusCode = 200, Result = entities };
//    }

//    public virtual async Task<RepositoryResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
//    {
//        var entity = await _table.FirstOrDefaultAsync(predicate);
//        return entity == null
//            ? new RepositoryResult<TEntity> { Succeeded = false, StatusCode = 404, Error = "Entity not found." }
//            : new RepositoryResult<TEntity> { Succeeded = true, StatusCode = 200, Result = entity };
//    }

//    public virtual async Task<RepositoryResult<bool>> AddAsync(TEntity entity)
//    {
//        if (entity == null)
//            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity cannot be null" };

//        try
//        {
//            await _table.AddAsync(entity);
//            await _context.SaveChangesAsync();
//            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 201 };
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex.Message);
//            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
//        }
//    }

//    public virtual async Task<RepositoryResult<bool>> AddRangeAsync(IEnumerable<TEntity> entities)
//    {
//        if (entities == null || !entities.Any())
//            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entities cannot be null or empty" };

//        try
//        {
//            await _table.AddRangeAsync(entities);
//            await _context.SaveChangesAsync();
//            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 201 };
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex.Message);
//            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
//        }
//    }

//    public virtual async Task<RepositoryResult<bool>> UpdateAsync(TEntity entity)
//    {
//        if (entity == null)
//            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity cannot be null" };

//        try
//        {
//            _table.Update(entity);
//            await _context.SaveChangesAsync();
//            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex.Message);
//            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
//        }
//    }

//    public virtual async Task<RepositoryResult<bool>> DeleteAsync(TEntity entity)
//    {
//        if (entity == null)
//            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity cannot be null" };

//        try
//        {
//            _table.Remove(entity);
//            await _context.SaveChangesAsync();
//            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex.Message);
//            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
//        }
//    }

//    public virtual async Task<RepositoryResult<bool>> DeleteRangeAsync(IEnumerable<TEntity> entities)
//    {
//        if (entities == null || !entities.Any())
//            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entities cannot be null or empty" };

//        try
//        {
//            _table.RemoveRange(entities);
//            await _context.SaveChangesAsync();
//            return new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 };
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex.Message);
//            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
//        }
//    }

//    public virtual async Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> findBy)
//    {
//        var exists = await _table.AnyAsync(findBy);
//        return !exists 
//            ? new RepositoryResult<bool> { Succeeded = false, StatusCode = 404, Error = "Entity not found." }
//            : new RepositoryResult<bool> { Succeeded = true, StatusCode = 200, Result = true };
//    }

//    public Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync(bool orderByDescending)
//    {
//        throw new NotImplementedException();
//    }

//    Task<RepositoryResult<IEnumerable<StatusEntity>>> IBaseRepository<TEntity>.GetAllAsync(bool orderByDescending)
//    {
//        throw new NotImplementedException();
//    }
//}
