using System.Linq.Expressions;
using Entities.DTOs;
using Entities.Enums;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Repository.Interface;
using Service.Interface;
using Utilities;

namespace Service.Implementation
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IVendorService _vendorService;
        public UserService(IBaseRepo<User> repo, IUserRepo userRepo) : base(repo)
        {
           
            _userRepo = userRepo;

        }

        public async Task AddUserAsync(UserDTO model)
        {
            User user = MapperHelper.MapTo<UserDTO, User>(model);
            await AddAsync(user);

        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync(UserSearchEntity searchEntity)
        {

            if (searchEntity.sortBy == "roleName")
            {
                searchEntity.sortBy = "Role.RoleName";
            }

            BaseSearchEntity<User> baseSearchEntity = new BaseSearchEntity<User>()
            {
                predicate = GeneratePredicate(searchEntity),
                includes = GenerateInclude(),
                pageNumber = searchEntity.pageNumber,
                pageSize = searchEntity.pageSize,
                sortBy = searchEntity.sortBy,
                sortOrder = searchEntity.sortOrder,
       

            };
            baseSearchEntity.SetSortingExpression();
            IEnumerable<User> users = await GetAllAsync(baseSearchEntity);
            IEnumerable<UserDTO> data = MapperHelper.MapTo<IEnumerable<User>, IEnumerable<UserDTO>>(users);
            return data;
        }

        private Expression<Func<User, bool>> GeneratePredicate(UserSearchEntity searchEntity)
        {
            int id = 0;
            if (searchEntity.id.HasValue)
            {
                id = (int)searchEntity.id;
            }
            return x => (string.IsNullOrWhiteSpace(searchEntity.name) ||
                 (x.Name.ToLower().Contains(searchEntity.name.Trim().ToLower()) ||
                  x.SurName.ToLower().Contains(searchEntity.name.Trim().ToLower()))) &&
                (string.IsNullOrWhiteSpace(searchEntity.roleName) ||
                 x.Role.RoleName.ToLower().Contains(searchEntity.roleName.Trim().ToLower()))
                && (!searchEntity.id.HasValue || (searchEntity.id.HasValue && x.Id == searchEntity.id));
        }

        private Expression<Func<User, object>>[] GenerateInclude()
        {
            return new Expression<Func<User, object>>[] { x => x.Role, x=>x.Manager ,x=>x.Vendor };
        }


        public async Task UpdateUserAsync(UserDTO model)
        {
            User user = MapperHelper.MapTo<UserDTO, User>(model);
            await _userRepo.UpdateUserAsync(user);
        }

        public async Task<IEnumerable<RelationshipManagerDTO>> GetAllRelationshipManagers()
        {
            BaseSearchEntity<User> baseSearchEntity = new BaseSearchEntity<User>()
            {

                pageSize = int.MaxValue,
                predicate = x => x.RoleId == (int)RoleEnum.GearedSalesRep || (x.RoleId == (int)RoleEnum.GearedSuperAdmin && (bool)x.IsUserInGafsalesRepList)

            };
            IEnumerable<User> users = await GetAllAsync(baseSearchEntity);
            return MapperHelper.MapTo<IEnumerable<User>, IEnumerable<RelationshipManagerDTO>>(users);
        }

     

        public async Task<bool> DeleteUser(int id)
        {
            User user = await GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            await _userRepo.Delete(user);
            return true;
        }

        public async Task<IEnumerable<RelationshipManagerDTO>> GetReportingToListAsync(int vendorId, int managerLevelId)
        {
            BaseSearchEntity<User> searchEntity = new BaseSearchEntity<User>();

                int originalLevelNo = 0;
                if (managerLevelId != 0)
                {
                    Expression<Func<ManagerLevel, bool>> predicate = x => x.Id == managerLevelId;
                    ManagerLevel mangerLevel = await GetOtherByIdAsync(predicate);
                    originalLevelNo = mangerLevel.LevelNo;
                }
                searchEntity.pageSize = int.MaxValue;
                searchEntity.predicate = (managerLevelId != 0 ? x => x.Manager.LevelNo == originalLevelNo + 1 : x => x.Manager.VendorId == vendorId && x.Manager.LevelNo == 1)  ;
                searchEntity.includes = new Expression<Func<User, object>>[] { x => x.Manager };
            
            IEnumerable<User> users = await _userRepo.GetAllAsync(searchEntity);
            return MapperHelper.MapTo<IEnumerable<User>, IEnumerable<RelationshipManagerDTO>>(users);

        }

        public async Task<IsExistData> CheckValidityAsync(string email, string mobile)
        {
            IsExistData isExistData = new IsExistData();
            BaseSearchEntity<User> baseSearchEntity = new BaseSearchEntity<User>()
            {            
                predicate= x => x.Email == email,
                pageSize=1
            
            };
            IEnumerable<User> data = await GetAllAsync(baseSearchEntity);
            if (data.Any())
            {
                isExistData.isEmailExist = true;
            }          
             

            baseSearchEntity.predicate = x=> x.Mobile == mobile;
            IEnumerable<User> userData = await GetAllAsync(baseSearchEntity);
            if (userData.Any())
            {
                isExistData.isExistMobile = true;
            }            

            return isExistData;
        }
    }
}
