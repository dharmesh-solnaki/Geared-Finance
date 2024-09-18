using Entities.UtilityModels;
using Repository.Interface;
using Service.Interface;
using System.Linq.Expressions;

namespace Service.Implementation;

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

    public async Task AddRangeAsync(IEnumerable<T> items)
    {
        await _repo.AddRangeAsync(items);
    }

    public async Task Delete(T item)
    {
        await _repo.Delete(item);
    }

    public async Task<IQueryable<T>> GetAllAsync(BaseSearchEntity<T> searchEntity)
    {
        return await _repo.GetAllAsync(searchEntity);
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<U> GetOtherAsync<U>(Expression<Func<U, bool>> predicate, Expression<Func<U, object>>[]? includes) where U : class
    {
        return await _repo.GetByOtherAsync(predicate, includes);
    }

    public async Task SaveChangesAsync()
    {
        await _repo.SaveChangesAsync();
    }

    public async Task UpdateAsync(T item)
    {
        await _repo.UpdateAsync(item);
    }

    public async Task UpdateRangeAsync(IEnumerable<T> items)
    {
        await _repo.UpdateRangeAsync(items);
    }
}
