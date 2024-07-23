

using Entities.Models;
using Entities.UtilityModels;

namespace Repository.Interface
{
    public interface IVendorRepo : IBaseRepo<Vendor>
    {
           Task<IEnumerable<ManagerLevel>> GetManagerLevelsById(int id);
    }
}
