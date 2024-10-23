using Microsoft.EntityFrameworkCore;

namespace Entities.DTOs;
public class InterestChartDTO
{
    public int Id { get; set; }

    public string TypeOfFinance { get; set; } = null!;

    public string MinValue { get; set; } = null!;

    public string MaxValue { get; set; } = null!;

    [Precision(7, 2)]
    public decimal? Month24 { get; set; }

    [Precision(7, 2)]
    public decimal? Month36 { get; set; }

    [Precision(7, 2)]
    public decimal? Month48 { get; set; }

    [Precision(7, 2)]
    public decimal? Month60 { get; set; }

    [Precision(7, 2)]
    public decimal? Month72 { get; set; }

    [Precision(7, 2)]
    public decimal? Month84 { get; set; }

    [Precision(5, 2)]
    public decimal? MaxBrokerage { get; set; }

    [Precision(5, 2)]
    public decimal DefaultRateAdjustment { get; set; }

    [Precision(5, 2)]
    public decimal? MaxBrokerageCeiling { get; set; }

    public string? UpdatedDate { get; set; } = null!;

}
