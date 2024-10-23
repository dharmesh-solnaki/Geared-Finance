using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Table("EquipmentCategory")]
public partial class EquipmentCategory
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [InverseProperty("Equipmentcategory")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [InverseProperty("EquipmentCategory")]
    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();

    [InverseProperty("EquipmentCategory")]
    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
