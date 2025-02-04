﻿using Entities.DBContext;
using Entities.UtilityModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Linq.Expressions;

namespace Repository.Implementation;
public class BaseRepo<T> : IBaseRepo<T> where T : class
{
    private readonly ApplicationDBContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public BaseRepo(ApplicationDBContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<IQueryable<T>> GetAllAsync(BaseSearchEntity<T> searchEntity)
    {
        IQueryable<T> query = _dbSet.AsNoTracking().AsQueryable();

        if (searchEntity.Predicate != null)
        {
            query = query.Where(searchEntity.Predicate);
        }
        if (searchEntity.Includes != null)
        {
            query = searchEntity.Includes.Aggregate(query, (current, include) =>
            {
                return current.Include(include);
            });
        }
        if (searchEntity.ThenIncludes != null)
        {
            query = searchEntity.ThenIncludes.Aggregate(query, (current, include) =>
            {
                return current.Include(include);
            });
        }

        if (searchEntity.Selects != null)
        {
            query = query.Select(searchEntity.Selects);
        }

        if (searchEntity.SortingExpression != null)
        {
            query = searchEntity.SortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(searchEntity.SortingExpression)
                : query.OrderBy(searchEntity.SortingExpression);
        }

        return query;
    }

    public async Task AddAsync(T item)
    {
        await _dbSet.AddAsync(item);
        await SaveChangesAsync();
    }

    public async Task Delete(T item)
    {
        _dbSet.Remove(item);
        await SaveChangesAsync();

    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T item)
    {
        var createdDateProperty = typeof(T).GetProperty("CreatedDate");
        var createdByProperty = typeof(T).GetProperty("CreatedBy");
        var idProperty = typeof(T).GetProperty("Id");
        if (createdDateProperty != null && createdByProperty != null && idProperty != null)
        {
            var idValue = idProperty.GetValue(item);
            var oldItem = await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(idValue));
            //var oldItem = await _dbSet.FindAsync(idProperty.GetValue(item));

            createdDateProperty.SetValue(item, createdDateProperty.GetValue(oldItem));

            createdByProperty.SetValue(item, createdByProperty.GetValue(oldItem));
        }
        _dbSet.Update(item);
        await SaveChangesAsync();
    }

    public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task<U> GetByOtherAsync<U>(Expression<Func<U, bool>> predicate, Expression<Func<U, object>>[]? includes) where U : class
    {
        DbSet<U> dbset = _dbContext.Set<U>();

        IQueryable<U> query = dbset;

        if (includes != null && includes.Any())
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes)
    {
        IQueryable<T> query = _dbContext.Set<T>().AsNoTracking().AsQueryable();

        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.FirstOrDefaultAsync(predicate);

    }


    public async Task AddRangeAsync(IEnumerable<T> item)
    {
        await _dbSet.AddRangeAsync(item);
        await SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<T> items)
    {

        var createdDate = typeof(T).GetProperty("CreatedDate");
        var createdBy = typeof(T).GetProperty("CreatedBy");
        var idProperty = typeof(T).GetProperty("Id");

        if (createdDate != null && createdBy != null && idProperty != null)
        {
            IQueryable<T> query = _dbSet.AsQueryable().AsNoTracking();
            var updatedItems = items.Join(query, item => idProperty.GetValue(item), entity => idProperty.GetValue(entity), (item, entity) =>
            {
                createdDate.SetValue(item, createdDate.GetValue(entity));
                createdBy.SetValue(item, createdBy.GetValue(entity));
                return item;

            }).ToList();
            _dbSet.UpdateRange(updatedItems);
        }
        else
        {
            _dbSet.UpdateRange(items);
        }

        await SaveChangesAsync();
    }

    public async Task DeleteRange(IEnumerable<T> item)
    {
        _dbSet.RemoveRange(item);
        await SaveChangesAsync();
    }

    public async Task<T> InsertAsync(T item)
    {
        await _dbSet.AddAsync(item);
        await SaveChangesAsync();
        return item;
    }

    public async Task<IQueryable<T>> BulkUpdateAsync(IEnumerable<T> items)
    {
        _dbSet.UpdateRange(items);
        await SaveChangesAsync();
        return items.AsQueryable().AsNoTracking();
    }

}
