using Entities.UtilityModels;

namespace Repository.Interface
{
    public interface IBaseRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(BaseSearchEntity<T> searchEntity);
 
        Task AddAsync(T item);
        
        Task UpdateAsync(T item);
    
        Task Delete(T item);
    
        Task SaveChangesAsync();

    }
}
