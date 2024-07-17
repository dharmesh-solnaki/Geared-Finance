using System.Linq.Expressions;
using Entities.DBContext;
using Entities.Models;
using Entities.UtilityModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implementation
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        private readonly ApplicationDBContext _dbContext;
     

        public UserRepo(ApplicationDBContext context):base(context) 
        {
            _dbContext = context;
         
        }

    }
}
