using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Table("EquipmentType")]
public partial class EquipmentType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [Column("EquipmentCategoriesID")]
    public int EquipmentCategoriesId { get; set; }

    public int Sequence { get; set; }

    [InverseProperty("EquipmentType")]
    public virtual ICollection<Quote> QuoteEquipmentTypes { get; set; } = new List<Quote>();

    [InverseProperty("Equipment")]
    public virtual ICollection<Quote> QuoteEquipments { get; set; } = new List<Quote>();
}
