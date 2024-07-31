using Entities.DBContext;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Implementation;
using Repository.Interface;

namespace Repository.Repos
{
    public class EquipmentRepo : BaseRepo<FundingEquipmentType>, IEquipmentRepo
    {
        private readonly ApplicationDBContext _dbContext;
        public EquipmentRepo(ApplicationDBContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<FundingCategory>> GetEquipmentCategoriesAsync()
        {
            return await _dbContext.FundingCategories.ToListAsync();
        }
    }
}
