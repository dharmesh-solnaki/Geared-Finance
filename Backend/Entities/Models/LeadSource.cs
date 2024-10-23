using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Table("LeadSource")]
public partial class LeadSource
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [InverseProperty("LeadSourceNavigation")]
    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
}
