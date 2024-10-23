using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Table("EquipmentCategories")]
public partial class EquipmentCategory1
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    public int Sequence { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [InverseProperty("CategoryNavigation")]
    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
}
