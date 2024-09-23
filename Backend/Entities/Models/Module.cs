using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("Module")]
public partial class Module
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string ModuleName { get; set; } = null!;

    [InverseProperty("Module")]
    public virtual ICollection<Right> Rights { get; set; } = new List<Right>();
}
