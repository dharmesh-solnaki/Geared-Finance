using Entities.Models;
using Entities.UtilityModels;

namespace Repository.Interface;
public interface IRolePermissionRepo : IBaseRepo<Right>
{
    IQueryable<Role> GetAllRoles(BaseSearchEntity<Role> roleSearch);
    IQueryable<Module> GetAllModules();
}
