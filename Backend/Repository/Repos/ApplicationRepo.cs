using Entities.DBContext;
using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Microsoft.EntityFrameworkCore;
using Repository.Implementation;
using Repository.Interface;
using Utilities;

namespace Repository.Repos;
public class ApplicationRepo : BaseRepo<Lead>, IApplicationRepo
{
    private readonly ApplicationDBContext _dbContext;

    public ApplicationRepo(ApplicationDBContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<DisplayLeadDTO> GetAllLeads(SqlSearchParams searchParams)
    {
        IQueryable<DisplayLeadDTO> leadData = _dbContext.Set<DisplayLeadDTO>().FromSqlRaw(Constants.GET_LEADS,
              searchParams.PipelineType,
              searchParams.LeadType,
              searchParams.OwnerIds,
              searchParams.VendorId,
              searchParams.VendorSalesIds,
              searchParams.RecordType,
              searchParams.UserId,
              searchParams.ListType).AsQueryable();
        return leadData;
    }
}
