using Entities.DBContext;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implementation;
public class UserRepo : BaseRepo<User>, IUserRepo
{
    private readonly ApplicationDBContext _dbContext;


    public UserRepo(ApplicationDBContext context) : base(context)
    {
        _dbContext = context;

    }


    public async Task UpdateUserAsync(User user)
    {
        var trackedEntity = _dbContext.ChangeTracker.Entries<User>().FirstOrDefault(e => e.Entity.Id == user.Id);

        if (trackedEntity != null)
        {
            _dbContext.Entry(trackedEntity.Entity).State = EntityState.Detached;
        }

        _dbContext.Users.Attach(user);
        _dbContext.Entry(user).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
}
