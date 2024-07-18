using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;

namespace Service.Interface
{
    public interface IUserService : IBaseService<User>
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync(UserSearchEntity searchEntity);
        Task AddUserAsync(UserDTO model);

        Task<IEnumerable<VendorDTO>> GetAllVendors();
    }
}
