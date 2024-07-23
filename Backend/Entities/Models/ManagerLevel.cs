using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Index("VendorId", Name = "IX_ManagerLevels_ManagerId")]
public partial class ManagerLevel
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string LevelName { get; set; } = null!;

    public int VendorId { get; set; }

    public int LevelNo { get; set; }

    [InverseProperty("ManagerLevel")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    [ForeignKey("VendorId")]
    [InverseProperty("ManagerLevels")]
    public virtual Vendor Vendor { get; set; } = null!;
}
