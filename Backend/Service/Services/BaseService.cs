using Entities.UtilityModels;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        private readonly IBaseRepo<T> _repo;

        public BaseService(IBaseRepo<T> repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(T item)
        {
            await _repo.AddAsync(item);
        }

        public async Task Delete(T item)
        {
            await _repo.Delete(item);
        }

        public async Task<IEnumerable<T>> GetAllAsync(BaseSearchEntity<T> searchEntity)
        {
            return await _repo.GetAllAsync(searchEntity);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return  await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<U>> GetOtheEntityListAsync<U>(BaseSearchEntity<U>? searchEntity) where U : class
        {
            return await _repo.GetOthers<U>(searchEntity);
        }

        public async Task SaveChangesAsync()
        {
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(T item)
        {
            await _repo.UpdateAsync(item);
        }
    }
}
