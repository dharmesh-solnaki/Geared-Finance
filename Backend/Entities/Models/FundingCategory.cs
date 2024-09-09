using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("FundingCategory")]
public partial class FundingCategory
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [InverseProperty("EquipmentCategory")]
    public virtual ICollection<FunderProductFunding> FunderProductFundings { get; set; } = new List<FunderProductFunding>();

    [InverseProperty("Category")]
    public virtual ICollection<FundingEquipmentType> FundingEquipmentTypes { get; set; } = new List<FundingEquipmentType>();
}
