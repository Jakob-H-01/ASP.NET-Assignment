using System.Diagnostics;
using System.Linq.Expressions;
using Data.Contexts;
using Data.Interfaces;
using Data.Models;
using Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel> where TEntity : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _table;

    protected BaseRepository(DataContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    public virtual async Task<RepositoryResult<bool>> AddAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null" };

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

    public virtual async Task<RepositoryResult<IEnumerable<TModel>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? filterBy = null, params Expression<Func<TEntity, object>>[] include)
    {
        IQueryable<TEntity> query = _table;

        if (filterBy != null)
            query = query.Where(filterBy);

        if (include != null && include.Length > 0)
            foreach (var item in include)
                query = query.Include(item);

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var entities = await query.ToListAsync();
        var result = entities.Select(entity => entity.MapTo<TModel>());
        return new RepositoryResult<IEnumerable<TModel>> { Succeeded = true, StatusCode = 200, Result = result };
    }

    public virtual async Task<RepositoryResult<TModel>> GetAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object>>[] include)
    {
        IQueryable<TEntity> query = _table;

        if (include != null && include.Length > 0)
            foreach (var item in include)
                query = query.Include(item);

        var entity = await query.FirstOrDefaultAsync(findBy);
        if (entity == null)
            return new RepositoryResult<TModel> { Succeeded = false, StatusCode = 404, Error = "Entity not found" };

        var result = entity.MapTo<TModel>();
        return new RepositoryResult<TModel> { Succeeded = true, StatusCode = 200, Result = result };
    }
    
    public virtual async Task<RepositoryResult<TEntity>> GetEntityAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object>>[] include)
    {
        IQueryable<TEntity> query = _table;

        if (include != null && include.Length > 0)
            foreach (var item in include)
                query = query.Include(item);

        var entity = await query.FirstOrDefaultAsync(findBy);
        if (entity == null)
            return new RepositoryResult<TEntity> { Succeeded = false, StatusCode = 404, Error = "Entity not found" };

        return new RepositoryResult<TEntity> { Succeeded = true, StatusCode = 200, Result = entity };
    }

    public virtual async Task<RepositoryResult<bool>> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null" };

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

    public virtual async Task<RepositoryResult<bool>> DeleteAsync(Expression<Func<TEntity, bool>> findBy)
    {
        if (findBy == null)
            return new RepositoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Search param can't be null" };

        try
        {
            var entity = await _table.FirstOrDefaultAsync(findBy) ?? null!;
            if (entity == null)
                return new RepositoryResult<bool> { Succeeded = false, StatusCode = 404, Error = "Entity not found" };

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

    public virtual async Task<RepositoryResult<bool>> ExistsAsync(Expression<Func<TEntity, bool>> findBy)
    {
        return await _table.AnyAsync(findBy)
            ? new RepositoryResult<bool> { Succeeded = true, StatusCode = 200 }
            : new RepositoryResult<bool> { Succeeded = false, StatusCode = 404, Error = "Entity not found" };
    }
}