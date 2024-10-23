using Entities.DTOs;
using Entities.Enums;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;

namespace Service.Services
{
    public class ApplicationService : BaseService<Lead>, IApplicationService
    {

        private readonly IApplicationRepo _leadRepo;
        private readonly IBaseRepo<User> _userRepo;
        private readonly IAuthService _authService;
        public ApplicationService(IApplicationRepo leadRepo, IBaseRepo<User> userRepo, IAuthService authService) : base(leadRepo)
        {
            _leadRepo = leadRepo;
            _userRepo = userRepo;
            _authService = authService;
        }

        public async Task<BaseResponseDTO<DisplayLeadDTO>> GetAllLeadsAsync(FilterearchPayload<SqlSearchParams> payload)
        {
            var searchParams = payload.SearchParams;
            var searchModel = payload.SearchModel;

            searchParams.UserId = _authService.GetUserId();
            searchModel.SortBy ??= "Created";
            if (searchParams.LeadType != 0)
            {
                searchParams.LeadType = searchParams.UserId;
            }

            return await (_leadRepo.GetAllLeads(searchParams)).GetFilteredBaseResponseAsync(searchModel.PageNumber, searchModel.PageSize, searchModel.SortBy, searchModel.SortOrder == "ASC");
        }

        public async Task<IEnumerable<IdNameDTO>> GetSalesRepListAsync()
        {

            BaseSearchEntity<User> baseSearch = new()
            {
                Predicate = x => x.RoleId == (int)RoleEnum.GearedSalesRep,
                PageNumber = 1,
                PageSize = int.MaxValue
            };
            IQueryable<User> users = await _userRepo.GetAllAsync(baseSearch);
            return MapperHelper.MapTo<IEnumerable<User>, IEnumerable<IdNameDTO>>(users.ToList());
        }

        public async Task<UserStatusDTO> GetUserStatus()
        {
            int userId = _authService.GetUserId();
             User user = await _userRepo.GetByIdAsync(userId);
            return new UserStatusDTO
            {
                IsIncludesInGAF = user?.IsUserInGafsalesRepList ?? false,
                IsIncludesInVSR = user?.IsUserInVendorSalesRepList ?? false
            };
        }

        public async Task<IEnumerable<IdNameDTO>> GetVendorRepAsync(int id)
        {
            BaseSearchEntity<User> baseSearch = new()
            {
                Predicate = x => x.RoleId == (int)RoleEnum.VendorSalesRep && x.VendorId == id,
                PageNumber = 1,
                PageSize = int.MaxValue
            };
            IQueryable<User> users = await _userRepo.GetAllAsync(baseSearch);
            return MapperHelper.MapTo<IEnumerable<User>, IEnumerable<IdNameDTO>>(users.ToList());
        }
        
    }
}
