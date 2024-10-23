using Entities.DTOs;
using Entities.Models;

namespace Service.Extensions;
public static class RateChartOptionMapping
{
    public static RateChartOption FromDto(this RateChartOptionDTO optionDTO)
    {

        return new()
        {
            Id = optionDTO.Id,
            EquipmentChartName = optionDTO.EquipmentChartName,
            IsInterestRatesVary = optionDTO.IsInterestRatesVary,
            RentalTerms = optionDTO.RentalTerms,
            ChattelMortgageTerms = optionDTO.ChattelMortgageTerms,
            TypeOfFinance = optionDTO.TypeOfFinance,
        };
    }

    public static RateChartOptionDTO ToDto(this RateChartOption rateChart)
    {
        return new()
        {
            Id = rateChart.Id,
            EquipmentChartName = rateChart.EquipmentChartName,
            TypeOfFinance = rateChart.TypeOfFinance,
            IsInterestRatesVary = rateChart.IsInterestRatesVary,
            ChattelMortgageTerms = rateChart.ChattelMortgageTerms,
            RentalTerms = rateChart.RentalTerms,
            SelectedFunding = rateChart.InterestChartFundings.ToList().ToDto(),
            InterestChartChattelMortgage = rateChart.InterestCharts.Where(x => x.TypeOfFinance.Equals("Chattel mortgage") && !x.IsDeleted).Select(x => x.ToDto()).ToList(),
            InterestChartRental = rateChart.InterestCharts.Where(x => x.TypeOfFinance.Equals("Rental") && !x.IsDeleted).Select(x => x.ToDto()).ToList(),

        };
    }

}
