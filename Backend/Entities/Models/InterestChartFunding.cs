using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class InterestChartFunding
{
    [Key]
    public int Id { get; set; }

    public int ChartEquipmentId { get; set; }

    public int EquipmentId { get; set; }

    public int EquipmentCategoryId { get; set; }

    [ForeignKey("ChartEquipmentId")]
    [InverseProperty("InterestChartFundings")]
    public virtual RateChartOption ChartEquipment { get; set; } = null!;

    [ForeignKey("EquipmentId")]
    [InverseProperty("InterestChartFundings")]
    public virtual FundingEquipmentType Equipment { get; set; } = null!;

    [ForeignKey("EquipmentCategoryId")]
    [InverseProperty("InterestChartFundings")]
    public virtual FundingCategory EquipmentCategory { get; set; } = null!;
}
