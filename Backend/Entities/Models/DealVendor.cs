using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class DealVendor
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("DealID")]
    public int DealId { get; set; }

    [Column("VendorID")]
    public int? VendorId { get; set; }

    [Column("VendorRepID")]
    public int? VendorRepId { get; set; }

    [ForeignKey("DealId")]
    [InverseProperty("DealVendors")]
    public virtual Deal Deal { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("DealVendors")]
    public virtual Vendor? Vendor { get; set; }

    [ForeignKey("VendorRepId")]
    [InverseProperty("DealVendors")]
    public virtual User? VendorRep { get; set; }
}
