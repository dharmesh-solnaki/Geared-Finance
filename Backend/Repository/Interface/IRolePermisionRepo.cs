using Entities.Models;
using Entities.UtilityModels;

namespace Repository.Interface
{
    public interface IRolePermisionRepo : IBaseRepo<Right>
    {
        Task<IQueryable<Role>> GetAllRolesAsync(BaseSearchEntity<Role> roleSearch);
        Task<IQueryable<Module>> GetAllModulesAsync();
    }
}
