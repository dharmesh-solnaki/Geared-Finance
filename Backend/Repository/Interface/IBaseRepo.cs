using Entities.UtilityModels;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface IBaseRepo<T> where T : class
    {
        Task<IQueryable<T>> GetAllAsync(BaseSearchEntity<T> searchEntity);
        Task<U> GetByOtherAsync<U>(Expression<Func<U, bool>> predicate, Expression<Func<U, object>>[]? includes) where U : class;
        Task<T> GetByIdAsync(int id);

        Task AddAsync(T item);

        Task UpdateAsync(T item);

        Task Delete(T item);

        Task SaveChangesAsync();

    }
}
