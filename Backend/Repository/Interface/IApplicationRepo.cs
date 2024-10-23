using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;

namespace Repository.Interface
{
    public interface IApplicationRepo : IBaseRepo<Lead>
    {
        IQueryable<DisplayLeadDTO> GetAllLeads(SqlSearchParams searchParams);
    }
}
