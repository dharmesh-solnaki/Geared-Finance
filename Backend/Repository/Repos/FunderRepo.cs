using Entities.DBContext;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Implementation;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Repository.Repos
{
    public class FunderRepo : BaseRepo<Funder>, IFunderRepo
    {
        private readonly ApplicationDBContext _dbContext;
        public FunderRepo(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddFunderGuideAsync(FunderProductGuide funderProductGuide)
        {
             await _dbContext.FunderProductGuides.AddAsync(funderProductGuide);
           await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeFunderGuideFundingAsync(List<FunderProductFunding> selectedFundings)
        {
            await _dbContext.FunderProductFundings.AddRangeAsync(selectedFundings);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IQueryable<FunderProductFunding>> GetFundingsAsync(int id)
        {
            return  _dbContext.FunderProductFundings.Where(f => f.FundingProductGuideId== id).Include(x=>x.Equipment).Include(x=>x.EquipmentCategory).AsQueryable();
        }

        public async Task RemoveRangeSelectedFundingAsync(List<FunderProductFunding> fundingsToRemove)
        {
            _dbContext.FunderProductFundings.RemoveRange(fundingsToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateFunderGuideAsync(FunderProductGuide funderProductGuide)
        {
            _dbContext.FunderProductGuides.Update(funderProductGuide);
            await _dbContext.SaveChangesAsync();
        }

    }
}
