using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;
using System.Linq.Expressions;
using Utilities;

namespace Service.Services;

public class RolePermissionService : BaseService<Right>, IRolePermissionService
{
    private readonly IRolePermissionRepo _repo;
    public RolePermissionService(IRolePermissionRepo repo) : base(repo)
    {
        _repo = repo;
    }

    public IEnumerable<ModulesDTO> GetModules()
    {
        return MapperHelper.MapTo<IEnumerable<Module>, IEnumerable<ModulesDTO>>(_repo.GetAllModules());
    }

    public async Task<IEnumerable<RightsDTO>> GetRolePermissionAsync(int roleId)
    {
        BaseSearchEntity<Right> baseSearchEntity = new()
        {
            Predicate = x => x.RoleId == roleId,
            Includes = new Expression<Func<Right, object>>[] { x => x.Module }
        };
        var rights = await _repo.GetAllAsync(baseSearchEntity);
        return MapperHelper.MapTo<IEnumerable<Right>, IEnumerable<RightsDTO>>(rights);
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
        BaseSearchEntity<Right> baseSearchEntity = new()
        {
            Predicate = x => (x.RoleId == roleId && x.Module.ModuleName.ToLower() == module.ToLower()),
            Includes = new Expression<Func<Right, object>>[] { x => x.Module }
        };
        IEnumerable<Right> rights = await GetAllAsync(baseSearchEntity);

        //Expression<Func<Right, bool>> predicate = x => (x.RoleId == roleId && x.Module.ModuleName.ToLower() == module.ToLower());
        //Expression<Func<Right, object>>[] includes = new Expression<Func<Right, object>>[] { x => x.Module };
        //Right right = await _repo.GetOneAsync(predicate, includes);
        //Right right = await _repo.GetByOtherAsync(predicate, includes);
        return MapperHelper.MapTo<Right, RightsDTO>(rights.FirstOrDefault());
    }

    public IEnumerable<RoleDTO> GetAllRoles(BaseModelSearchEntity model)
    {
        BaseSearchEntity<Role> roleSearch = new()
        {
            Predicate = model.Id.HasValue ? x => x.Id == model.Id : null,
            SortBy = Constants.ROLENAME.Split('.').Last(),
            SortOrder = string.IsNullOrWhiteSpace(model.SortOrder) ? Constants.ASC : model.SortOrder,
        };
        roleSearch.SetSortingExpression();
        return MapperHelper.MapTo<IEnumerable<Role>, IEnumerable<RoleDTO>>(_repo.GetAllRoles(roleSearch));

    }

}
