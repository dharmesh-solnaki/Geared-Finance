namespace Entities.DTOs;
public class RateChartOptionDTO
{
    public int Id { get; set; }

    public string EquipmentChartName { get; set; } = null!;

    public string TypeOfFinance { get; set; } = null!;

    public bool IsInterestRatesVary { get; set; }

    public string? ChattelMortgageTerms { get; set; }

    public string? RentalTerms { get; set; }

    public List<SelectedFundingDTO> SelectedFunding { get; set; }

    public List<InterestChartDTO>? InterestChartChattelMortgage { get; set; }
    public List<InterestChartDTO>? InterestChartRental { get; set; }

}
