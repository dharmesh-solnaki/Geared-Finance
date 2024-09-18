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

namespace Service.Implementation;

public class UserService : BaseService<User>, IUserService
{
    private readonly IUserRepo _userRepo;
    public UserService(IBaseRepo<User> repo, IUserRepo userRepo) : base(repo)
    {
        _userRepo = userRepo;
    }
    public async Task<BaseResponseDTO<UserDTO>> GetUsersAsync(UserSearchEntity searchEntity)
    {

        if (string.IsNullOrEmpty(searchEntity.sortBy))
        {
            searchEntity.sortBy = Constants.NAME;
        }
        else if (searchEntity.sortBy == "roleName")
        {
            searchEntity.sortBy = Constants.ROLEID;
        }
        PredicateModel model = new()
        {
            Id = searchEntity.id,
            Criteria = new Dictionary<string, object>
            {
                {Constants.ROLENAME,searchEntity.roleName }
            },
            Property1 = Constants.FULLNAME,
            Keyword = searchEntity.name,
        };

        Expression<Func<User, bool>> predicate = PredicateBuilder.BuildPredicate<User>(model);
        BaseSearchEntity<User> baseSearchEntity = new()
        {
            predicate = predicate,
            includes = new Expression<Func<User, object>>[] { x => x.Role, x => x.Manager, x => x.Vendor, x => x.RelationshipManagerNavigation },
            pageNumber = searchEntity.pageNumber,
            pageSize = searchEntity.pageSize,
            sortBy = searchEntity.sortBy,
            sortOrder = searchEntity.sortOrder,
        };
        baseSearchEntity.SetSortingExpression();
        IQueryable<User> users = await GetAllAsync(baseSearchEntity);
        BaseResponseDTO<UserDTO> userDataResponse = new() { TotalRecords = users.Count() };
        //List<User> userPageList = await GetPaginatedList(searchEntity.pageNumber, searchEntity.pageSize, users).ToListAsync();
        List<User> userPageList = await users.GetSelectedListAsync(searchEntity.pageNumber,searchEntity.pageSize).ToListAsync();
        userDataResponse.ResponseData = MapperHelper.MapTo<List<User>, List<UserDTO>>(userPageList);

        return userDataResponse;
    }

    public async Task<IsExistData> UpsertUserAsync(UserDTO model)
    {
        User user = MapperHelper.MapTo<UserDTO, User>(model);
        IsExistData response = new();
        try
        {
            if (model.id == 0)
            {
                await AddAsync(user);
            }
            else
            {
                await _userRepo.UpdateUserAsync(user);
                response.IsEmailExist = false;
                response.IsExistMobile = false;
            }
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                if (ex.InnerException.ToString().Contains(Constants.UNIQUE_EMAIL))
                {
                    response.IsEmailExist = true;
                }
                if (ex.InnerException.ToString().Contains(Constants.UNIQUE_MOBILE))
                {
                    response.IsExistMobile = true;
                }
            }
        }
        return response;
    }

    public async Task<IEnumerable<RelationshipManagerDTO>> GetAllRelationshipManagers()
    {
        BaseSearchEntity<User> baseSearchEntity = new()
        {

            pageSize = int.MaxValue,
            predicate = x => x.RoleId == (int)RoleEnum.GearedSalesRep || (x.RoleId == (int)RoleEnum.GearedSuperAdmin && (bool)x.IsUserInGafsalesRepList)

        };
        return MapperHelper.MapTo<IEnumerable<User>, IEnumerable<RelationshipManagerDTO>>(await GetAllAsync(baseSearchEntity));
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
        BaseSearchEntity<User> searchEntity = new();

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


        return MapperHelper.MapTo<IEnumerable<User>, IEnumerable<RelationshipManagerDTO>>(await _userRepo.GetAllAsync(searchEntity));

    }


    public async Task<User> GetUserById(int id)
    {
        return await GetByIdAsync(id);
    }
}
