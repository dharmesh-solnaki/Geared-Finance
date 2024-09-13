using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;

namespace Service.Interface
{
    public interface IUserService : IBaseService<User>
    {
        Task<BaseResponseDTO<UserDTO>> GetUsersAsync(UserSearchEntity searchEntity);
        Task<IsExistData> UpsertUserAsync(UserDTO model);
        Task<IEnumerable<RelationshipManagerDTO>> GetAllRelationshipManagers();
        Task<bool> DeleteUser(int id);
        Task<IEnumerable<RelationshipManagerDTO>> GetReportingToListAsync(int vendorId, int managerLevelId);
    }
}
