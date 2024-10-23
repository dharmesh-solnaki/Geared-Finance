namespace Entities.DTOs;
public class RateChartResponseDTO
{
    public SelectedFundingDTO[] AvailableFundings { get; set; }

    public IEnumerable<RateChartOptionDTO> RateCharts { get; set; }

    public string TypeOfFinance { get; set; } = null!;
}
