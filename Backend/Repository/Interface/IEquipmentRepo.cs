using Entities.Models;

namespace Repository.Interface;
public interface IEquipmentRepo : IBaseRepo<FundingEquipmentType>
{
    Task<IEnumerable<FundingCategory>> GetEquipmentCategoriesAsync();
}
