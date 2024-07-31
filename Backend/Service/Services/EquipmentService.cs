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

namespace Service.Services
{
    public class EquipmentService : BaseService<FundingEquipmentType>, IEquipmentService
    {
        private readonly IEquipmentRepo _equipmentRepo;

        public EquipmentService(IEquipmentRepo repo) : base(repo)
        {
            _equipmentRepo = repo;
        }

        public async Task<BaseRepsonseDTO<EquipmentRepsonseDTO>> GetAllEuqipmentType(BaseModelSearchEntity searchModal)
        {
            PredicateModel predicateModel = new PredicateModel()
            {
                Criteria = new Dictionary<string, object>
                {
                    {"category.name",searchModal.name},
                },
                Property1 = "name",
                Keyword = searchModal.name
            };
            Expression<Func<FundingEquipmentType, bool>> predicate = PredicateBuilder.BuildPredicate<FundingEquipmentType>(predicateModel);

            BaseSearchEntity<FundingEquipmentType> baseSearchEntity = new BaseSearchEntity<FundingEquipmentType>()
            {
                predicate=predicate,
                includes= new Expression<Func<FundingEquipmentType, object>>[] { x => x.Category },
                pageNumber=searchModal.pageNumber,
                pageSize=searchModal.pageSize,
                sortBy=searchModal.sortBy,
                sortOrder=searchModal.sortOrder,
            };
            baseSearchEntity.SetSortingExpression();

            IQueryable<FundingEquipmentType> fundingEquipmentTypes = await GetAllAsync(baseSearchEntity);
            BaseRepsonseDTO<EquipmentRepsonseDTO> baseRepsonse = new BaseRepsonseDTO<EquipmentRepsonseDTO> { TotalRecords = fundingEquipmentTypes.Count() };

            List<FundingEquipmentType> paginatedFundingEquipmentTypes = await GetPaginatedList(searchModal.pageNumber,searchModal.pageSize,fundingEquipmentTypes).ToListAsync();

            baseRepsonse.responseData = MapperHelper.MapTo<List<FundingEquipmentType>,List<EquipmentRepsonseDTO>>(paginatedFundingEquipmentTypes);
            return baseRepsonse;
        }

        public async Task<IEnumerable<FundingCategoryDTO>> GetEquipmentCategoriesAsync()
        {
            IEnumerable<FundingCategory> equipmentCategories = await _equipmentRepo.GetEquipmentCategoriesAsync();
            return MapperHelper.MapTo<IEnumerable<FundingCategory>, IEnumerable<FundingCategoryDTO>>(equipmentCategories);
        }

        public async Task UpsertAsync(EquipmentTypeDTO model)
        {
            if (model.Id == null)
            {
                FundingEquipmentType fundingEquipmentType = MapperHelper.MapTo<EquipmentTypeDTO,FundingEquipmentType>(model);
                await _equipmentRepo.AddAsync(fundingEquipmentType);
            }
            else
            {
                FundingEquipmentType fundingEquipmentType = MapperHelper.MapTo<EquipmentTypeDTO, FundingEquipmentType>(model);
                await _equipmentRepo.UpdateAsync(fundingEquipmentType);
            }
        }   
    }
}
