using Entities.UtilityModels;

namespace Service.Interface
{
    public interface IBaseService<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(BaseSearchEntity<T> searchEntity);
        //public Task<T> GetByIdAsync(int id);

        public Task AddAsync(T item);
        //public Task AddRangeAsync(IEnumerable<T> items);


        public Task UpdateAsync(T item);
        //public Task UpdateRangeAsync(IEnumerable<T> items);


        public Task Delete(T item);
        //public Task DeleteRange(IEnumerable<T> items);

        public Task SaveChangesAsync();

        public Task<IEnumerable<U>> GetOtheEntityListAsync<U>() where U : class;
    }
}
