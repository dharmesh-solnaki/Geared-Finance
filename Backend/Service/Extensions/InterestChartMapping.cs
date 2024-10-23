using Entities.DTOs;
using Entities.Models;

namespace Service.Extensions;
public static class InterestChartMapping
{
    public static InterestChart FromDto(this InterestChartDTO chartDto)
    {

        return new()
        {
            Id = chartDto.Id,
            TypeOfFinance = chartDto.TypeOfFinance,
            MinValue = chartDto.MinValue,
            MaxValue = chartDto.MaxValue,
            _24month = chartDto.Month24,
            _36month = chartDto.Month36,
            _48month = chartDto.Month48,
            _60month = chartDto.Month60,
            _72month = chartDto.Month72,
            _84month = chartDto.Month84,
            MaxBrokerage = chartDto.MaxBrokerage,
            DefaultRateAdjustment = chartDto.DefaultRateAdjustment,
            MaxBrokerageCeiling = chartDto.MaxBrokerageCeiling,

        };
    }

    public static InterestChartDTO ToDto(this InterestChart interestChart)
    {
        return new()
        {
            Id = interestChart.Id,
            TypeOfFinance = interestChart.TypeOfFinance,
            MinValue = interestChart.MinValue,
            MaxValue = interestChart.MaxValue,
            MaxBrokerage = interestChart.MaxBrokerage,
            MaxBrokerageCeiling = interestChart.MaxBrokerageCeiling,
            DefaultRateAdjustment = interestChart.DefaultRateAdjustment,
            Month24 = interestChart._24month,
            Month36 = interestChart._36month,
            Month48 = interestChart._48month,
            Month60 = interestChart._60month,
            Month72 = interestChart._72month,
            Month84 = interestChart._84month,
            UpdatedDate = (interestChart.ModifiedDate ?? interestChart.CreatedDate).ToString("yyyy-MM-dd"),
            //UpdatedDate = (interestChart.ModifiedDate!=null ? (DateTime)interestChart.ModifiedDate :interestChart.CreatedDate).ToString("yyyy-MM-dd"),
        };

    }
}
