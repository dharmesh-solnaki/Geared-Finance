using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DBContext;
using Entities.Models;
using Entities.UtilityModels;
using Microsoft.EntityFrameworkCore;
using Repository.Implementation;
using Repository.Interface;

namespace Repository.Repos
{
    public class VendorRepo : BaseRepo<Vendor>, IVendorRepo
    {
        private readonly ApplicationDBContext _dbContext;
        public VendorRepo(ApplicationDBContext context) : base(context)
        {
            _dbContext = context;
        }


        public async Task<IEnumerable<ManagerLevel>> GetManagerLevelsById(int id)
        {
           return await _dbContext.ManagerLevels.Where(x=>x.VendorId==id).ToListAsync();
        }
    }
}
