using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;

namespace Service.Services;

public class VendorService : BaseService<Vendor>, IVendorService
{
    private readonly IVendorRepo _vendorRepo;
    public VendorService(IVendorRepo repo) : base(repo)
    {
        _vendorRepo = repo;
    }

    public async Task<IEnumerable<VendorDTO>> GetAllVendors(BaseModelSearchEntity searchEntity)
    {
        BaseSearchEntity<Vendor> searchEntity2 = new()
        {
            PageNumber = searchEntity.PageNumber,
            PageSize = searchEntity.PageSize,
            SortBy = searchEntity.SortBy,
            SortOrder = searchEntity.SortOrder,

        };
        if (searchEntity.Id != null && searchEntity.Id != 0)
        {
            searchEntity2.Predicate = x => x.Id == searchEntity.Id;
        }
        return MapperHelper.MapTo<IEnumerable<Vendor>, IEnumerable<VendorDTO>>(await _vendorRepo.GetAllAsync(searchEntity2));

    }


    public async Task<IEnumerable<ManagerLevelDTO>> GetManagerLevels(int id)
    {
        return MapperHelper.MapTo<IEnumerable<ManagerLevel>, IEnumerable<ManagerLevelDTO>>(await _vendorRepo.GetManagerLevelsById(id));
    }
}
