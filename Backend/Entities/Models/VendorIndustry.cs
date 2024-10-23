using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Table("VendorIndustry")]
public partial class VendorIndustry
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string? Name { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [InverseProperty("Industry")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [InverseProperty("IndustryNavigation")]
    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();
}
