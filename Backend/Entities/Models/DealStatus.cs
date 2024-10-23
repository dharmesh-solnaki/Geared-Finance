using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Table("DealStatus")]
public partial class DealStatus
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [Column("ParentDealStatusID")]
    public int ParentDealStatusId { get; set; }

    public short SortOrder { get; set; }

    [InverseProperty("Dealstatus")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [InverseProperty("DealStatus")]
    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();

    [InverseProperty("DealStatus")]
    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
