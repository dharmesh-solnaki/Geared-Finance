using Entities.DTOs;
using Entities.Models;

namespace Service.Extensions;
internal static class InterestChartFundingMapping
{
    public static List<InterestChartFunding> FromDto(this List<SelectedFundingDTO> fundingDTOs, int chartId)
    {
        return fundingDTOs.SelectMany(x => x.SubCategory.Select(subItem => new InterestChartFunding()
        {
            ChartEquipmentId = chartId,
            EquipmentCategoryId = x.Id,
            EquipmentId = subItem.Id,

        })).ToList();
    }
    public static List<SelectedFundingDTO> ToDto(this List<InterestChartFunding> interestFundings)
    {
        if (!interestFundings.Any())
        {
            return new List<SelectedFundingDTO>();
        }
        return interestFundings
            .GroupBy(f => f.EquipmentCategoryId)
            .Select(group => new SelectedFundingDTO
            {
                Id = group.Key,
                Name = group.First().EquipmentCategory.Name,
                SubCategory = group.Select(f => new SubCategory
                {
                    Id = f.EquipmentId,
                    Name = f.Equipment.Name
                }).ToArray(),
            })
            .ToList();
    }
}
