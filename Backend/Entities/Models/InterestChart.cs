using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("InterestChart")]
public partial class InterestChart
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string TypeOfFinance { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string MinValue { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string MaxValue { get; set; } = null!;

    [Column("24Month")]
    [Precision(7, 2)]
    public decimal? _24month { get; set; }

    [Column("36Month")]
    [Precision(7, 2)]
    public decimal? _36month { get; set; }

    [Column("48Month")]
    [Precision(7, 2)]
    public decimal? _48month { get; set; }

    [Column("60Month")]
    [Precision(7, 2)]
    public decimal? _60month { get; set; }

    [Column("72Month")]
    [Precision(7, 2)]
    public decimal? _72month { get; set; }

    [Column("84Month")]
    [Precision(7, 2)]
    public decimal? _84month { get; set; }

    [Precision(5, 2)]
    public decimal? MaxBrokerage { get; set; }

    [Precision(5, 2)]
    public decimal DefaultRateAdjustment { get; set; }

    [Precision(5, 2)]
    public decimal? MaxBrokerageCeiling { get; set; }

    [Column(TypeName = "time with time zone")]
    public DateTimeOffset CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public int RateChartId { get; set; }

    [ForeignKey("RateChartId")]
    [InverseProperty("InterestCharts")]
    public virtual RateChartOption RateChart { get; set; } = null!;
}
