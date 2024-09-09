using Entities.DTOs;
using Entities.Models;


namespace Service.Extensions;
public static class SelectedFundingMapping
{
    public static List<FunderProductFunding> FromDto(this SelectedFundingDTO[] fundingDto, int funderGuideTypeId)
    {
        return fundingDto
       .SelectMany(item => item.SubCategory.Select(subItem => new FunderProductFunding
       {
           FundingProductGuideId = funderGuideTypeId,
           EquipmentCategoryId = item.Id,
           EquipmentId = subItem.Id,
       }))
       .ToList();

    }

    public static SelectedFundingDTO[] ToDto(this List<FunderProductFunding> funderProductFundings)
    {
        if (!funderProductFundings.Any())
        {
            return Array.Empty<SelectedFundingDTO>();
        }
        return funderProductFundings
            .GroupBy(f => f.EquipmentCategoryId)
            .Select(group => new SelectedFundingDTO
            {
                Id = group.Key,
                Name = group.First().EquipmentCategory.Name,
                SubCategory = group.Select(f => new SubCategory
                {
                    Id = f.EquipmentId,
                    Name = f.Equipment.Name
                }).ToArray()
            })
            .ToArray();
    }

}
