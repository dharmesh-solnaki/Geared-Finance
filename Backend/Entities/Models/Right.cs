using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public partial class Right
{
    [Key]
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public int RoleId { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    public bool? CanView { get; set; }

    public bool? CanAdd { get; set; }

    public bool? CanEdit { get; set; }

    public bool? CanDelete { get; set; }

    [ForeignKey("ModuleId")]
    [InverseProperty("Rights")]
    public virtual Module Module { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("Rights")]
    public virtual Role Role { get; set; } = null!;
}
