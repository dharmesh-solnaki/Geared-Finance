using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;

namespace Service.Interface;
public interface IRolePermissionService : IBaseService<Right>
{
    IEnumerable<ModulesDTO> GetModules();
    Task<RightsDTO> GetRightByModule(string module, int roleId);
    Task<IEnumerable<RightsDTO>> GetRolePermissionAsync(int roleId);
    Task UpsertRightsAsync(IEnumerable<RightsDTO> rightsDTOs);
    IEnumerable<RoleDTO> GetAllRoles(BaseModelSearchEntity model);
}
