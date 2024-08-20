using Entities.DBContext;
using Entities.Models;
using Entities.UtilityModels;
using Repository.Implementation;
using Repository.Interface;

namespace Repository.Repos
{
    public class RolePermisionRepo : BaseRepo<Right>, IRolePermisionRepo
    {
        private readonly ApplicationDBContext _dbContext;
        public RolePermisionRepo(ApplicationDBContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IQueryable<Module>> GetAllModulesAsync()
        {
            return _dbContext.Modules.AsQueryable();
        }

        public async Task<IQueryable<Role>> GetAllRolesAsync(BaseSearchEntity<Role> searchEntity)
        {
            var query = _dbContext.Roles.AsQueryable();
            if (searchEntity.predicate != null)
            {
                query = query.Where(searchEntity.predicate);
            }
            if (searchEntity.sortingExpression != null)
            {
                query = searchEntity.sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(searchEntity.sortingExpression)
                    : query.OrderBy(searchEntity.sortingExpression);
            }

            return query;
        }


    }
}
