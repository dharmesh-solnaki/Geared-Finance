using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public partial class Vendor
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("Vendor")]
    public virtual ICollection<ManagerLevel> ManagerLevels { get; set; } = new List<ManagerLevel>();

    [InverseProperty("Vendor")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
