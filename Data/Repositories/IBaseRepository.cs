using System.Linq.Expressions;
using Data.Entities;
using Data.Models;

namespace Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    //------------
    //Skriven av Chatgpt
    Task<RepositoryResult<TEntity>> GetByIdAsync(string id);
    Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync(
        bool orderByDescending = false,
        Expression<Func<TEntity, bool>>? where = null,
        Expression<Func<TEntity, object>>? sortBy = null,
        params Expression<Func<TEntity, object>>[]? includes);
    Task<RepositoryResult<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> where,
        params Expression<Func<TEntity, object>>[]? includes);

    Task<RepositoryResult<bool>> AddAsync(TEntity entity);
    Task<RepositoryResult<bool>> UpdateAsync(TEntity entity);
    Task<RepositoryResult<bool>> DeleteAsync(TEntity entity);
    Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
//-----------------------------------
    //Task<RepositoryResult<TEntity>> GetByIdAsync(int id);
    //Task<RepositoryResult<IEnumerable<TEntity>>> GetAllAsync(bool orderByDescending, Func<object, object> value);
    //Task<RepositoryResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
    //Task<RepositoryResult<bool>> AddAsync(TEntity entity);
    //Task<RepositoryResult<bool>> UpdateAsync(TEntity entity);
    //Task<RepositoryResult<bool>> DeleteAsync(TEntity entity);
    //Task<RepositoryResult<IEnumerable<StatusEntity>>> GetAllAsync(bool orderByDescending);



}
