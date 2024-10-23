using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Table("ParentDealStatus")]
public partial class ParentDealStatus
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Status { get; set; } = null!;

    [InverseProperty("Parentdealstatus")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [InverseProperty("ParentDealStatus")]
    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    [InverseProperty("ParentDealStatus")]
    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();

    [InverseProperty("ParentDealStatus")]
    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
