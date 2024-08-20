using Entities.Models;
using Entities.UtilityModels;

namespace Repository.Interface
{
    public interface IUserRepo : IBaseRepo<User>
    {
       
        Task UpdateUserAsync(User user);
    }
}
