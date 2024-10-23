using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Vendor
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("Vendor")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [InverseProperty("Vendor")]
    public virtual ICollection<DealVendor> DealVendors { get; set; } = new List<DealVendor>();

    [InverseProperty("Vendor")]
    public virtual ICollection<Lead> Leads { get; set; } = new List<Lead>();

    [InverseProperty("Vendor")]
    public virtual ICollection<ManagerLevel> ManagerLevels { get; set; } = new List<ManagerLevel>();

    [InverseProperty("Vendor")]
    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();

    [InverseProperty("Vendor")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
