using Entities.UtilityModels;
using System.Linq.Expressions;

namespace Repository.Interface;
public interface IBaseRepo<T> where T : class
{
    Task<IQueryable<T>> GetAllAsync(BaseSearchEntity<T> searchEntity);
    Task<U> GetByOtherAsync<U>(Expression<Func<U, bool>> predicate, Expression<Func<U, object>>[]? includes) where U : class;
    Task<T> GetByIdAsync(int id);

    Task AddAsync(T item);
    Task AddRangeAsync(IEnumerable<T> item);

    Task UpdateAsync(T item);
    Task UpdateRangeAsync(IEnumerable<T> item);

    Task Delete(T item);
    Task DeleteRange(IEnumerable<T> item);

    Task SaveChangesAsync();
    Task<T> GetOneAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes);


}
