using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Repository.Interface;
using Service.Extensions;
using Service.Implementation;
using Service.Interface;
using System.Linq.Expressions;
using Utilities;


namespace Service.Services
{
    public class FunderService : BaseService<Funder>, IFunderService
    {
        private readonly IFunderRepo _funderRepo;
        private readonly IAuthService _authService;


        public FunderService(IFunderRepo repo, IAuthService authService ) : base(repo)
        {
            _funderRepo = repo;
            _authService = authService;
     
        }

        public async Task<BaseRepsonseDTO<DisplayFunderDTO>> GetAllFunders(BaseModelSearchEntity searchModel)
        {
                        
            PredicateModel model = new()
            {
                Id = searchModel.id,
                Criteria = new Dictionary<string, object>
                {
                {Constants.ISDELETED,false }
                },
            };

            Expression<Func<Funder, bool>> predicate = PredicateBuilder.BuildPredicate<Funder>(model);
            BaseSearchEntity<Funder> baseSearchEntity = new()
            {
                predicate = predicate,
                includes = new Expression<Func<Funder, object>>[] { x => x.FunderProductGuide },
                pageNumber = searchModel.pageNumber,
                pageSize = searchModel.pageSize,
                sortBy = searchModel.sortBy,
                sortOrder = searchModel.sortOrder,
            };
            baseSearchEntity.SetSortingExpression();
            IQueryable<Funder> funders = await GetAllAsync(baseSearchEntity);
            BaseRepsonseDTO<DisplayFunderDTO> fudnerResponse = new() { TotalRecords = funders.Count() };
            List<Funder> userPageList =  GetPaginatedList(searchModel.pageNumber, searchModel.pageSize, funders).ToList();
            fudnerResponse.responseData = userPageList.Select(funder => funder.ToDisplayFunderDTO()).ToList();

            return fudnerResponse;
        }

        public async Task<FunderDTO> GetFunder(int id)
        {
           return (await _funderRepo.GetByIdAsync(id)).ToDto();      
        }

        public async Task<FunderGuideTypeDTO> GetFunderType(int id)
        {
            //Expression<Func<FunderProductGuide, bool>> predicate = ; 
            
            //Expression<Func<FunderProductGuide, object>>[] includes = new Expression<Func<FunderProductGuide, object>>[] { x => x.FunderProductFundings , x=>x.FunderProductFundings.Select(f=>f.Equipment) , x=>x.FunderProductFundings.Select(f=>f.EquipmentCategory)};
            FunderProductGuide funderGuide = await _funderRepo.GetByOtherAsync<FunderProductGuide>(x => x.FunderId == id, null);

            if (funderGuide == null)
            {
                return null;
            }
            funderGuide.FunderProductFundings = (await _funderRepo.GetFundingsAsync(funderGuide.Id)).ToList();
            SelectedFundingDTO[] selectedFundings = Array.Empty<SelectedFundingDTO>();
            if (funderGuide.FunderProductFundings.Any())
            {
                selectedFundings = funderGuide.FunderProductFundings.ToList().ToDto();
            }
            FunderGuideTypeDTO funder = funderGuide.ToDto();
            funder.SelectedFundings = selectedFundings;
            return funder;
        }

        public async Task<int> UpsertFunder(FunderDTO funderDTO)
        {
            Funder funder = funderDTO.FromDto();
            if (!funderDTO.id.HasValue || funderDTO.id==0)
            {
                funder.CreatedBy = _authService.GetUserId();
                await _funderRepo.AddAsync(funder);
            }
            else
            {               
                funder.ModifiedDate = DateTime.Now;
                funder.ModifiedBy= _authService.GetUserId();
                await _funderRepo.UpdateAsync(funder);
            }
            return funder.Id;
        }

        public async Task<int> UpsertFunderGuide(FunderGuideTypeDTO funderGuideTypeDTO)
        {
            FunderProductGuide funderProductGuide = funderGuideTypeDTO.FromDto();
            if (!funderGuideTypeDTO.id.HasValue)
            {
                await _funderRepo.AddFunderGuideAsync(funderProductGuide);
            }
            else
            {
                await _funderRepo.UpdateFunderGuideAsync(funderProductGuide);
            }


            IQueryable<FunderProductFunding> existingFundingList = await _funderRepo.GetFundingsAsync(funderProductGuide.Id);
            List<FunderProductFunding> selectedFundings = funderGuideTypeDTO.SelectedFundings.FromDto(funderProductGuide.Id);          

            if (!existingFundingList.Any())
            {
                await _funderRepo.AddRangeFunderGuideFundingAsync(selectedFundings);
            }
            else
            {
                var fundingsToAdd = selectedFundings
                           .Where(selected => !existingFundingList
                               .Any(existing =>
                                   existing.EquipmentId == selected.EquipmentId &&
                                   existing.EquipmentCategoryId == selected.EquipmentCategoryId &&
                                   existing.FundingProductGuideId == selected.FundingProductGuideId))
                           .ToList();

                var fundingsToRemove = existingFundingList
                    .Where(existing => !selectedFundings
                        .Any(selected =>
                            selected.EquipmentId == existing.EquipmentId &&
                            selected.EquipmentCategoryId == existing.EquipmentCategoryId &&
                            selected.FundingProductGuideId == existing.FundingProductGuideId))
                    .ToList();


                await _funderRepo.AddRangeFunderGuideFundingAsync(fundingsToAdd);

                await _funderRepo.RemoveRangeSelectedFundingAsync(fundingsToRemove);
            }
            return funderProductGuide.Id;
        }
    }
}
