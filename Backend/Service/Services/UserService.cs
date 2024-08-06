using Entities.DTOs;
using Entities.Enums;
using Entities.Models;
using Entities.UtilityModels;
using Geared_Finance_API;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Interface;
using System.Linq.Expressions;
using Utilities;

namespace Service.Implementation
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepo _userRepo;
        public UserService(IBaseRepo<User> repo, IUserRepo userRepo) : base(repo)
        {
            _userRepo = userRepo;
        }
        public async Task<BaseRepsonseDTO<UserDTO>> GetUsersAsync(UserSearchEntity searchEntity)
        {

            if (searchEntity.sortBy == "roleName")
            {
                searchEntity.sortBy = "Role.RoleName";
            }
            if (string.IsNullOrEmpty(searchEntity.sortBy))
            {
                searchEntity.sortBy = "name";
            }
            PredicateModel model = new PredicateModel()
            {
                Id = searchEntity.id,
                Criteria = new Dictionary<string, object>
                {
                    {"Role.RoleName",searchEntity.roleName }
                },
                Property1 = "name",
                Property2 = "surName",
                Keyword = searchEntity.name,
            };

            Expression<Func<User, bool>> predicate = PredicateBuilder.BuildPredicate<User>(model);
            BaseSearchEntity<User> baseSearchEntity = new BaseSearchEntity<User>()
            {
                predicate = predicate,
                includes = GenerateInclude(),
                pageNumber = searchEntity.pageNumber,
                pageSize = searchEntity.pageSize,
                sortBy = searchEntity.sortBy,
                sortOrder = searchEntity.sortOrder,
            };
            baseSearchEntity.SetSortingExpression();
            IQueryable<User> users = await GetAllAsync(baseSearchEntity);
            BaseRepsonseDTO<UserDTO> userDataResponse = new BaseRepsonseDTO<UserDTO>() { TotalRecords = users.Count() };
            List<User> userPageList = await GetPaginatedList(searchEntity.pageNumber, searchEntity.pageSize, users).ToListAsync();
            userDataResponse.responseData = MapperHelper.MapTo<List<User>, List<UserDTO>>(userPageList);

            return userDataResponse;
        }

        public async Task<IsExistData> AddUserAsync(UserDTO model)
        {
            User user = MapperHelper.MapTo<UserDTO, User>(model);
            IsExistData response = new IsExistData();

            if (model.id == 0)
            {
                response = await CheckValidityAsync(model.Email, model.Mobile);
                if (!response.isEmailExist && !response.isExistMobile)
                {
                    await AddAsync(user);
                }
            }
            else
            {
                User oldUser = await GetByIdAsync((int)model.id);
                if (oldUser != null)
                {
                    bool isEmailChanged = !oldUser.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase);
                    bool isMobileChanged = !oldUser.Mobile.Equals(model.Mobile);

                    if (isEmailChanged || isMobileChanged)
                    {
                        response = await CheckValidityAsync(model.Email, model.Mobile);

                        if (isEmailChanged && response.isEmailExist)
                        {
                            response.isExistMobile = false;
                            return response;
                        }

                        if (isMobileChanged && response.isExistMobile)
                        {
                            response.isEmailExist = false;
                            return response;
                        }
                    }

                    await _userRepo.UpdateUserAsync(user);
                    response.isEmailExist = false;
                    response.isExistMobile = false;
                }
            }
            return response;
        }



        private Expression<Func<User, object>>[] GenerateInclude()
        {
            return new Expression<Func<User, object>>[] { x => x.Role, x => x.Manager, x => x.Vendor };
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
                ManagerLevel mangerLevel = await GetOtherAsync(predicate, null);
                originalLevelNo = mangerLevel.LevelNo;
            }
            searchEntity.pageSize = int.MaxValue;
            searchEntity.predicate = (managerLevelId != 0 ? x => x.Manager.LevelNo == originalLevelNo + 1 : x => x.Manager.VendorId == vendorId && x.Manager.LevelNo == 1);
            searchEntity.includes = new Expression<Func<User, object>>[] { x => x.Manager };

            IEnumerable<User> users = await _userRepo.GetAllAsync(searchEntity);
            return MapperHelper.MapTo<IEnumerable<User>, IEnumerable<RelationshipManagerDTO>>(users);

        }

        public async Task<IsExistData> CheckValidityAsync(string email, string mobile)
        {
            IsExistData isExistData = new IsExistData();
            BaseSearchEntity<User> baseSearchEntity = new BaseSearchEntity<User>()
            {
                predicate = x => x.Email == email,
                pageSize = 1

            };
            IEnumerable<User> data = await GetAllAsync(baseSearchEntity);
            if (data.Any())
            {
                isExistData.isEmailExist = true;
            }


            baseSearchEntity.predicate = x => x.Mobile == mobile;
            IEnumerable<User> userData = await GetAllAsync(baseSearchEntity);
            if (userData.Any())
            {
                isExistData.isExistMobile = true;
            }

            return isExistData;
        }
    }
}
