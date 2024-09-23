using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Utilities;

namespace Service.Services;

public class EquipmentService : BaseService<FundingEquipmentType>, IEquipmentService
{
    private readonly IEquipmentRepo _equipmentRepo;

    public EquipmentService(IEquipmentRepo repo) : base(repo)
    {
        _equipmentRepo = repo;
    }

    public async Task<BaseResponseDTO<EquipmentRepsonseDTO>> GetAllEquipmentType(BaseModelSearchEntity searchModal)
    {


        if (string.IsNullOrEmpty(searchModal.SortBy))
        {
            searchModal.SortBy = Constants.NAME;
        }
        else if (searchModal.SortBy == "categoryName")
        {
            searchModal.SortBy = Constants.CATEGORYNAME;
        }

        PredicateModel predicateModel = new()
        {
            Criteria = new Dictionary<string, object>
            {
            
                {Constants.CATEGORYNAME,searchModal.Name},
                {Constants.ISDELETED,false},
            },

            Property1 = Constants.NAME,
            Keyword = searchModal.Name,
        };
        Expression<Func<FundingEquipmentType, bool>> predicate = PredicateBuilder.BuildPredicate<FundingEquipmentType>(predicateModel);

        BaseSearchEntity<FundingEquipmentType> baseSearchEntity = new()
        {
            Predicate = predicate,
            Includes = new Expression<Func<FundingEquipmentType, object>>[] { x => x.Category },
            PageNumber = searchModal.PageNumber,
            PageSize = searchModal.PageSize,
            SortBy = searchModal.SortBy,
            SortOrder = searchModal.SortOrder,
        };
        baseSearchEntity.SetSortingExpression();

        IQueryable<FundingEquipmentType> fundingEquipmentTypes = await GetAllAsync(baseSearchEntity);
        BaseResponseDTO<EquipmentRepsonseDTO> baseRepsonse = new() { TotalRecords = fundingEquipmentTypes.Count() };

        //List<FundingEquipmentType> paginatedFundingEquipmentTypes = await GetPaginatedList(searchModal.pageNumber, searchModal.pageSize, fundingEquipmentTypes).ToListAsync();
        List<FundingEquipmentType> paginatedFundingEquipmentTypes = await fundingEquipmentTypes.GetSelectedListAsync(searchModal.PageNumber, searchModal.PageSize).ToListAsync();

        baseRepsonse.ResponseData = MapperHelper.MapTo<List<FundingEquipmentType>, List<EquipmentRepsonseDTO>>(paginatedFundingEquipmentTypes);
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
        FundingEquipmentType record = await GetByIdAsync(id) ;
        if (record == null) return false;
        record.IsDeleted = true;
        
        await UpdateAsync(record);
        return true;
    }
}
