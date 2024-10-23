using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;

namespace Service.Interface
{
    public interface IApplicationService : IBaseService<Lead>
    {
        Task<BaseResponseDTO<DisplayLeadDTO>> GetAllLeadsAsync(FilterearchPayload<SqlSearchParams> payload);
        Task<IEnumerable<IdNameDTO>> GetSalesRepListAsync();
        Task<UserStatusDTO> GetUserStatus();
        Task<IEnumerable<IdNameDTO>> GetVendorRepAsync(int id);
    }
}
