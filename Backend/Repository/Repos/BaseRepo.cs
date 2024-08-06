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
            _dbSet.Update(item);
            await SaveChangesAsync();
        }

        private static Expression<Func<T, object>> GetSortingExpression(string propertyName)
        {

            var param = Expression.Parameter(typeof(T), "x");
            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                return null;
            }
            var propertyAcess = Expression.MakeMemberAccess(param, property);
            var conversion = Expression.Convert(propertyAcess, typeof(object));
            return Expression.Lambda<Func<T, object>>(conversion, param);
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

    }
}
