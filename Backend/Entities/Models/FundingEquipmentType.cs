using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("FundingEquipmentType")]
public partial class FundingEquipmentType
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    public bool IsBeingUsed { get; set; }

    public int CategoryId { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("FundingEquipmentTypes")]
    public virtual FundingCategory Category { get; set; } = null!;

    [InverseProperty("Equipment")]
    public virtual ICollection<FunderProductFunding> FunderProductFundings { get; set; } = new List<FunderProductFunding>();
}
