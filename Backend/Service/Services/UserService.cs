using System.Linq.Expressions;
using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class UserService : BaseService<User>, IUserService
    {



        private readonly IUserRepo _userRepo;
        public UserService(IBaseRepo<User> repo, IUserRepo userRepo) : base(repo)
        {
            _userRepo = userRepo;

        }

        public async Task AddUserAsync(UserDTO model)
        {
            //User user = _mapper.Map<User>(model);

            User user = MapperHelper.MapTo<User>(model);
            user.Role = null;
            await AddAsync(user);

        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync(UserSearchEntity searchEntity)
        {


            //searchEntity.predicate = _userRepo.GeneratePredicate(searchEntity);
            //searchEntity.includes = _userRepo.GenerateInclude();
            BaseSearchEntity<User> baseSearchEntity = new BaseSearchEntity<User>()
            {
                predicate = GeneratePredicate(searchEntity),
                includes = GenerateInclude(),
                pageNumber = searchEntity.pageNumber,
                pageSize = searchEntity.pageSize,
                sortBy = searchEntity.sortBy        ,
                sortOrder = searchEntity.sortOrder,
            };

            IEnumerable<User> users = await GetAllAsync(baseSearchEntity);


            IEnumerable<UserDTO> data = MapperHelper.MapToList<UserDTO>(users);
     
            return data;
        }

        private Expression<Func<User, bool>> GeneratePredicate(UserSearchEntity searchEntity)
        {
            int id = 0;
            if (searchEntity.id != null)
            {
                id = (int)searchEntity.id;
            }
            return x => (string.IsNullOrWhiteSpace(searchEntity.name) ||
                 (x.Name.ToLower().Contains(searchEntity.name.Trim().ToLower()) ||
                  x.SurName.ToLower().Contains(searchEntity.name.Trim().ToLower()))) &&
                (string.IsNullOrWhiteSpace(searchEntity.roleName) ||
                 x.Role.RoleName.ToLower().Contains(searchEntity.roleName.Trim().ToLower()))
                && (id == 0 || x.Id == id);
        }

        private Expression<Func<User, object>>[] GenerateInclude()
        {
            return new Expression<Func<User, object>>[] { x => x.Role };
        }
    }
}
