using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;
using System.Linq.Expressions;
using Utilities;

namespace Service.Services;

public class EquipmentService : BaseService<FundingEquipmentType>, IEquipmentService
{
    private readonly IEquipmentRepo _equipmentRepo;

    public EquipmentService(IEquipmentRepo repo) : base(repo)
    {
        _equipmentRepo = repo;
    }

    public async Task<BaseRepsonseDTO<EquipmentRepsonseDTO>> GetAllEquipmentType(BaseModelSearchEntity searchModal)
    {

      
        if (string.IsNullOrEmpty(searchModal.sortBy))
        {
            searchModal.sortBy = Constants.NAME;
        }
        else if (searchModal.sortBy == "categoryName")
        {
            searchModal.sortBy = Constants.CATEGORYNAME;
        }

        PredicateModel predicateModel = new()
        {
            Criteria = new Dictionary<string, object>
            {
                {Constants.CATEGORYNAME,searchModal.name},
                //{"category.name",searchModal.name},
                {Constants.ISDELETED,false},
            },
            Property1 = Constants.NAME,
            Keyword = searchModal.name
        };
        Expression<Func<FundingEquipmentType, bool>> predicate = PredicateBuilder.BuildPredicate<FundingEquipmentType>(predicateModel);

        BaseSearchEntity<FundingEquipmentType> baseSearchEntity = new()
        {
            predicate = predicate,
            includes = new Expression<Func<FundingEquipmentType, object>>[] { x => x.Category },
            pageNumber = searchModal.pageNumber,
            pageSize = searchModal.pageSize,
            sortBy = searchModal.sortBy,
            sortOrder = searchModal.sortOrder,
        };
        baseSearchEntity.SetSortingExpression();

        IQueryable<FundingEquipmentType> fundingEquipmentTypes = await GetAllAsync(baseSearchEntity);
        BaseRepsonseDTO<EquipmentRepsonseDTO> baseRepsonse = new() { TotalRecords = fundingEquipmentTypes.Count() };

        List<FundingEquipmentType> paginatedFundingEquipmentTypes = await GetPaginatedList(searchModal.pageNumber, searchModal.pageSize, fundingEquipmentTypes).ToListAsync();

        baseRepsonse.responseData = MapperHelper.MapTo<List<FundingEquipmentType>, List<EquipmentRepsonseDTO>>(paginatedFundingEquipmentTypes);
        return baseRepsonse;
    }

    public async Task<IEnumerable<FundingCategoryDTO>> GetEquipmentCategoriesAsync()
    {
        return MapperHelper.MapTo<IEnumerable<FundingCategory>, IEnumerable<FundingCategoryDTO>>(await _equipmentRepo.GetEquipmentCategoriesAsync());
    }

    public async Task UpsertAsync(EquipmentTypeDTO model)
    {
        if (!model.Id.HasValue)
        {
            await _equipmentRepo.AddAsync(MapperHelper.MapTo<EquipmentTypeDTO, FundingEquipmentType>(model));
        }
        else
        {
            await _equipmentRepo.UpdateAsync(MapperHelper.MapTo<EquipmentTypeDTO, FundingEquipmentType>(model));
        }
    }
    public async Task<bool> DeleteEuipmentTypeAsync(int id)
    {
        FundingEquipmentType record = await GetByIdAsync(id);
        if (record == null) return false;
        record.IsDeleted = true;
        await UpdateAsync(record);
        return true;
    }
}
