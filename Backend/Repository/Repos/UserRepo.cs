using Entities.DBContext;
using Entities.Models;
using Repository.Interface;

namespace Repository.Implementation
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        private readonly ApplicationDBContext _dbContext;


        public UserRepo(ApplicationDBContext context) : base(context)
        {
            _dbContext = context;

        }

        public Task<IEnumerable<Vendor>> GetVendors()
        {
            throw new NotImplementedException();
        }
    }
}
