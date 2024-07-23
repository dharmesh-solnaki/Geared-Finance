using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;

namespace Service.Interface
{
    public interface IVendorService : IBaseService<Vendor>
    {
        Task<IEnumerable<VendorDTO>> GetAllVendors(BaseModelSearchEntity searchEntity);
        Task<IEnumerable<ManagerLevelDTO>> GetManagerLevels(int id);
    }
}
