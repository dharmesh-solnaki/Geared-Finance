using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;

namespace Service.Interface
{
    public interface IUserService : IBaseService<User>
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync(UserSearchEntity searchEntity);
        Task AddUserAsync(UserDTO model);
        Task UpdateUserAsync(UserDTO model);
        //Task<IEnumerable<VendorDTO>> GetAllVendors();
        Task<IEnumerable<RelationshipManagerDTO>> GetAllRelationshipManagers();
        //Task<IEnumerable<ManagerLevelDTO>> GetManagerLevels(int id);
        Task<bool> DeleteUser(int id);
        Task<IEnumerable<RelationshipManagerDTO>> GetReportingToListAsync(int vendorId, int managerLevelId );
        Task<IsExistData> CheckValidityAsync(string email, string mobile);
    }
}
