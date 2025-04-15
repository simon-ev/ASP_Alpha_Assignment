using System.Linq.Expressions;
using Data.Entities;
using Data.Models;

namespace Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<RepositoryResult<TEntity>> GetByIdAsync(int id);
    Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync(bool orderByDescending);
    Task<RepositoryResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<RepositoryResult<bool>> AddAsync(TEntity entity);
    Task<RepositoryResult<bool>> AddRangeAsync(IEnumerable<TEntity> entities);
    Task<RepositoryResult<bool>> UpdateAsync(TEntity entity);
    Task<RepositoryResult<bool>> DeleteAsync(TEntity entity);
    Task<RepositoryResult<bool>> DeleteRangeAsync(IEnumerable<TEntity> entities);
    Task<RepositoryResult<IEnumerable<StatusEntity>>> GetAllAsync(bool orderByDescending);
    
}
