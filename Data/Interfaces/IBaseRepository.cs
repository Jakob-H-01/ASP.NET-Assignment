using System.Linq.Expressions;
using Data.Models;

namespace Data.Interfaces;

public interface IBaseRepository<TEntity, TModel> where TEntity : class
{
    Task<RepositoryResult<bool>> AddAsync(TEntity entity);
    Task<RepositoryResult<bool>> DeleteAsync(Expression<Func<TEntity, bool>> findBy);
    Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> findBy);
    Task<RepositoryResult<IEnumerable<TModel>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? filterBy = null, params Expression<Func<TEntity, object>>[] include);
    Task<RepositoryResult<TModel>> GetAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object>>[] include);
    Task<RepositoryResult<TEntity>> GetEntityAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object>>[] include);
    Task<RepositoryResult<bool>> UpdateAsync(TEntity entity);
}