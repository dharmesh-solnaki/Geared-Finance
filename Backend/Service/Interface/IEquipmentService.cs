using Entities.DTOs;
using Entities.Models;
using Entities.UtilityModels;

namespace Service.Interface
{
    public interface IEquipmentService : IBaseService<FundingEquipmentType>
    {
        Task<BaseRepsonseDTO<EquipmentRepsonseDTO>> GetAllEquipmentType(BaseModelSearchEntity searchModal);
        Task<IEnumerable<FundingCategoryDTO>> GetEquipmentCategoriesAsync();
        Task UpsertAsync(EquipmentTypeDTO model);
        Task<bool> DeleteEuipmentTypeAsync(int id);
    }
}
