using Entities.Models;

namespace Repository.Interface
{
    public interface IUserRepo : IBaseRepo<User>
    {
        Task<IEnumerable<Vendor>> GetVendors();
    }
}
