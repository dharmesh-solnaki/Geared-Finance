using Entities.Models;

namespace Repository.Interface;
public interface IFunderRepo : IBaseRepo<Funder>
{
    IQueryable<FunderProductFunding> GetFundings(int id);
}
