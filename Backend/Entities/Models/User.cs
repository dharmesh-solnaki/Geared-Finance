using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Index("Email", Name = "UNIQUE_EMAIL", IsUnique = true)]
[Index("Mobile", Name = "UNIQUE_MOBILE", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string SurName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(10)]
    public string Mobile { get; set; } = null!;

    [StringLength(128)]
    public string? Password { get; set; }

    public int NotificationPreferences { get; set; }

    [Required]
    public bool? Status { get; set; }

    public bool? IsPortalLogin { get; set; }

    [Column("IsUserInGAFSalesRepList")]
    public bool? IsUserInGafsalesRepList { get; set; }

    public int? DayOfBirth { get; set; }

    public int? MonthOfBirth { get; set; }

    public int? RelationshipManager { get; set; }

    public int? ReportingTo { get; set; }

    public bool? IsUserInVendorSalesRepList { get; set; }

    public bool? UnassignedApplications { get; set; }

    public int RoleId { get; set; }

    public int? VendorId { get; set; }

    public bool? IsSendEndOfTermReport { get; set; }

    public bool? IsFunderProfile { get; set; }

    public bool? IsProceedBtnInApp { get; set; }

    public bool? IsCalcRateEditor { get; set; }

    [StringLength(3)]
    public string StaffCode { get; set; } = null!;

    public int? ManagerId { get; set; }

    [StringLength(6)]
    public string? Otp { get; set; }

    [Column(TypeName = "character varying")]
    public string? FullName { get; set; }

    [InverseProperty("Salesstaff")]
    public virtual ICollection<Application> ApplicationSalesstaffs { get; set; } = new List<Application>();

    [InverseProperty("Vendorrep")]
    public virtual ICollection<Application> ApplicationVendorreps { get; set; } = new List<Application>();

    [InverseProperty("VendorRep")]
    public virtual ICollection<DealVendor> DealVendors { get; set; } = new List<DealVendor>();

    [InverseProperty("SalesStaff")]
    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    [InverseProperty("RelationshipManagerNavigation")]
    public virtual ICollection<User> InverseRelationshipManagerNavigation { get; set; } = new List<User>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Lead> LeadCreatedByNavigations { get; set; } = new List<Lead>();

    [InverseProperty("SalesStaff")]
    public virtual ICollection<Lead> LeadSalesStaffs { get; set; } = new List<Lead>();

    [InverseProperty("VendorRep")]
    public virtual ICollection<Lead> LeadVendorReps { get; set; } = new List<Lead>();

    [ForeignKey("ManagerId")]
    [InverseProperty("Users")]
    public virtual ManagerLevel? Manager { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    [InverseProperty("SalesStaff")]
    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();

    [ForeignKey("RelationshipManager")]
    [InverseProperty("InverseRelationshipManagerNavigation")]
    public virtual User? RelationshipManagerNavigation { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("Users")]
    public virtual Vendor? Vendor { get; set; }
}
