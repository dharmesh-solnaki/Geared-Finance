using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("FunderProductFunding")]
public partial class FunderProductFunding
{
    [Key]
    public int Id { get; set; }

    public int FundingProductGuideId { get; set; }

    public int EquipmentId { get; set; }

    public int EquipmentCategoryId { get; set; }

    [ForeignKey("EquipmentId")]
    [InverseProperty("FunderProductFundings")]
    public virtual FundingEquipmentType Equipment { get; set; } = null!;

    [ForeignKey("EquipmentCategoryId")]
    [InverseProperty("FunderProductFundings")]
    public virtual FundingCategory EquipmentCategory { get; set; } = null!;

    [ForeignKey("FundingProductGuideId")]
    [InverseProperty("FunderProductFundings")]
    public virtual FunderProductGuide FundingProductGuide { get; set; } = null!;
}
