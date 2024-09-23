using Entities.DBContext;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Implementation;
using Repository.Interface;

namespace Repository.Repos
{
    public class FunderRepo : BaseRepo<Funder>, IFunderRepo
    {
        private readonly ApplicationDBContext _dbContext;
        public FunderRepo(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<FunderProductFunding> GetFundings(int id)
        {
            return _dbContext.FunderProductFundings.Where(f => f.FundingProductGuideId == id).Include(x => x.Equipment).Include(x => x.EquipmentCategory).AsQueryable();
        }
    }
}
