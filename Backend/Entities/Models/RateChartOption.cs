using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public partial class RateChartOption
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string EquipmentChartName { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string TypeOfFinance { get; set; } = null!;

    [Column("IsInterestRatesVary ")]
    public bool IsInterestRatesVary { get; set; }

    [Column(TypeName = "character varying")]
    public string? RentelTerms { get; set; }

    [Column(TypeName = "character varying")]
    public string? ChattelMortagageeTerms { get; set; }

    [Column(TypeName = "time with time zone")]
    public DateTimeOffset CreatedDate { get; set; }

    [Column(TypeName = "character varying")]
    public string CreatedBy { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public int FunderId { get; set; }

    [ForeignKey("FunderId")]
    [InverseProperty("RateChartOptions")]
    public virtual Funder Funder { get; set; } = null!;

    [InverseProperty("ChartEquipment")]
    public virtual ICollection<InterestChartFunding> InterestChartFundings { get; set; } = new List<InterestChartFunding>();

    [InverseProperty("RateChart")]
    public virtual ICollection<InterestChart> InterestCharts { get; set; } = new List<InterestChart>();
}
