using Entities.UtilityModels;
using System.Linq.Expressions;

namespace Service.Interface;
public interface IBaseService<T> where T : class
{
    public Task<IQueryable<T>> GetAllAsync(BaseSearchEntity<T> searchEntity);
    public Task<T> GetByIdAsync(int id);
    public Task AddAsync(T item);
    public Task AddRangeAsync(IEnumerable<T> items);
    public Task UpdateRangeAsync(IEnumerable<T> items);
    public Task UpdateAsync(T item);
    public Task Delete(T item);
    public Task SaveChangesAsync();
    public Task<U> GetOtherAsync<U>(Expression<Func<U, bool>> predicate, Expression<Func<U, object>>[]? includes) where U : class;


}
