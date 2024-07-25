using System.Linq.Expressions;
using Entities.UtilityModels;

namespace Service.Interface
{
    public interface IBaseService<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(BaseSearchEntity<T> searchEntity);
        public Task<T> GetByIdAsync(int id);
        public Task AddAsync(T item);
        public Task UpdateAsync(T item);
        public Task Delete(T item);
        public Task SaveChangesAsync();
        public Task<U> GetOtherByIdAsync<U>(Expression<Func<U,bool>> predicate) where U :class;
  
    }
}
