using Entities.DBContext;
using Entities.Models;
using Entities.UtilityModels;
using Repository.Implementation;
using Repository.Interface;

namespace Repository.Repos;
public class RolePermissionRepo : BaseRepo<Right>, IRolePermissionRepo
{
    private readonly ApplicationDBContext _dbContext;
    public RolePermissionRepo(ApplicationDBContext context) : base(context)
    {
        _dbContext = context;
    }

    public IQueryable<Module> GetAllModules()
    {
        return _dbContext.Modules.AsQueryable();
    }

    public IQueryable<Role> GetAllRoles(BaseSearchEntity<Role> searchEntity)
    {
        var query = _dbContext.Roles.AsQueryable();
        if (searchEntity.Predicate != null)
        {
            query = query.Where(searchEntity.Predicate);
        }
        if (searchEntity.SortingExpression != null)
        {
            query = searchEntity.SortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(searchEntity.SortingExpression)
                : query.OrderBy(searchEntity.SortingExpression);
        }

        return query;
    }


}
