using Entities.Models;

namespace Repository.Interface
{
    public interface IVendorRepo : IBaseRepo<Vendor>
    {
        Task<IEnumerable<ManagerLevel>> GetManagerLevelsById(int id);
    }
}
