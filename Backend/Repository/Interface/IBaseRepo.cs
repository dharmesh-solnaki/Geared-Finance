using Entities.UtilityModels;

namespace Repository.Interface
{
    public interface IBaseRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(BaseSearchEntity<T> searchEntity);
        Task<IEnumerable<U>> GetOthers<U>(BaseSearchEntity<U>? searchEntity) where U : class;
        Task<T> GetByIdAsync(int id);

        Task AddAsync(T item);

        Task UpdateAsync(T item);

        Task Delete(T item);

        Task SaveChangesAsync();

    }
}
