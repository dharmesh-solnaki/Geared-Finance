using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;
using System.Linq.Expressions;

namespace Service.Services
{
    public class RolePermisionService : BaseService<Right>, IRolePermisionService
    {
        private readonly IRolePermisionRepo _repo;
        public RolePermisionService(IRolePermisionRepo repo) : base(repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ModulesDTO>> GetModulesAsync()
        {
            return MapperHelper.MapTo<IEnumerable<Module>, IEnumerable<ModulesDTO>>(await _repo.GetAllModulesAsync());
        }

        public async Task<IEnumerable<RightsDTO>> GetRolePermissionAsync(int roleId)
        {
            BaseSearchEntity<Right> baseSearchEntity = new()
            {
                predicate = x => x.RoleId == roleId,
                includes = new Expression<Func<Right, object>>[] { x => x.Module }
            };
            return MapperHelper.MapTo<IEnumerable<Right>, IEnumerable<RightsDTO>>(await _repo.GetAllAsync(baseSearchEntity));
        }

        public async Task UpsertRightsAsync(IEnumerable<RightsDTO> rightsDTOs)
        {
            IEnumerable<Right> rightList = MapperHelper.MapTo<IEnumerable<RightsDTO>, IEnumerable<Right>>(rightsDTOs);
            if (rightsDTOs.FirstOrDefault().Id == 0)
            {
                await AddRangeAsync(rightList);
            }
            else
            {
                await UpdateRangeAsync(rightList);
            }

        }


        public async Task<RightsDTO> GetRightByModule(string module, int roleId)
        {
            BaseSearchEntity<Right> baseSearchEntity = new BaseSearchEntity<Right>()
            {
                predicate = x => (x.RoleId == roleId && x.Module.ModuleName.ToLower() == module.ToLower()),
                includes = new Expression<Func<Right, object>>[] { x => x.Module }
            };
            IEnumerable<Right> rights = await GetAllAsync(baseSearchEntity);
            Right permissionRight = rights.FirstOrDefault();
            return MapperHelper.MapTo<Right, RightsDTO>(permissionRight);
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync(BaseModelSearchEntity model)
        {
            BaseSearchEntity<Role> roleSearch = new BaseSearchEntity<Role>()
            {
                predicate = model.id.HasValue ? x => x.Id == model.id : null,
                sortBy = "RoleName",
                sortOrder = string.IsNullOrWhiteSpace(model.sortOrder) ? "asc" : model.sortOrder,
            };
            roleSearch.SetSortingExpression();
            return MapperHelper.MapTo<IEnumerable<Role>, IEnumerable<RoleDTO>>(await _repo.GetAllRolesAsync(roleSearch));

        }

    }
}
