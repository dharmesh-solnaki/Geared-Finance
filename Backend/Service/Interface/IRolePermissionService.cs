using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;

namespace Service.Interface
{
    public interface IRolePermissionService : IBaseService<Right>
    {
        Task<IEnumerable<ModulesDTO>> GetModulesAsync();
        Task<RightsDTO> GetRightByModule(string module, int roleId);
        Task<IEnumerable<RightsDTO>> GetRolePermissionAsync(int roleId);
        Task UpsertRightsAsync(IEnumerable<RightsDTO> rightsDTOs);
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync(BaseModelSearchEntity model);
    }
}
