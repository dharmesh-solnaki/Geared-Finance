using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IFunderRepo : IBaseRepo<Funder>
    {
        Task AddFunderGuideAsync(FunderProductGuide funderProductGuide);
        Task AddRangeFunderGuideFundingAsync(List<FunderProductFunding> selectedFundings);
        Task<IQueryable<FunderProductFunding>> GetFundingsAsync(int id);
        Task RemoveRangeSelectedFundingAsync(List<FunderProductFunding> fundingsToRemove);
        Task UpdateFunderGuideAsync(FunderProductGuide funderProductGuide);

    }
}
