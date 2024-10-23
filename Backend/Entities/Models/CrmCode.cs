using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class CrmCode
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("CrmCode", TypeName = "character varying")]
    public string? CrmCode1 { get; set; }

    [Column(TypeName = "character varying")]
    public string? Digit { get; set; }

    [InverseProperty("Crm")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [InverseProperty("Crm")]
    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    [InverseProperty("Crm")]
    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();

    [InverseProperty("Crm")]
    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
