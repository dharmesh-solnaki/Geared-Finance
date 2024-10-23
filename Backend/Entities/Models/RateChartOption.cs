using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string? RentalTerms { get; set; }

    [Column(TypeName = "character varying")]
    public string? ChattelMortgageTerms { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

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
