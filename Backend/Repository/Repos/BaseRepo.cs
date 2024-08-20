using Entities.DBContext;
using Entities.UtilityModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Linq.Expressions;

namespace Repository.Implementation
{
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

            if (searchEntity.predicate != null)
            {
                query = query.Where(searchEntity.predicate);
            }
            if (searchEntity.includes != null)
            {
                query = searchEntity.includes.Aggregate(query, (current, include) =>
                {
                    return current.Include(include);
                });
            }
            {
                if (searchEntity.thenIncludes != null)
                    query = searchEntity.thenIncludes.Aggregate(query, (current, include) =>
                    {
                        return current.Include(include);
                    });
            }
            if (searchEntity.selects != null)
            {
                query = query.Select(searchEntity.selects);
            }

            if (searchEntity.sortingExpression != null)
            {
                query = searchEntity.sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(searchEntity.sortingExpression)
                    : query.OrderBy(searchEntity.sortingExpression);
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
                var oldItem = await _dbSet.FindAsync(idProperty.GetValue(item));
                createdDateProperty.SetValue(item, createdDateProperty.GetValue(oldItem));

                createdByProperty.SetValue(item, createdByProperty.GetValue(oldItem));
            }
            _dbSet.Update(item);
            await SaveChangesAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

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
                _dbContext.UpdateRange(updatedItems);
            }
            else
            {
                _dbContext.UpdateRange(items);
            }

            await SaveChangesAsync();
        }
    }
}
