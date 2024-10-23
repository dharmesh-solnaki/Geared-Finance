using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Lead
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string? BusinessName { get; set; }

    [Column(TypeName = "character varying")]
    public string ContactPerson { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string? PhoneNumber { get; set; }

    [Column(TypeName = "character varying")]
    public string? EmailAddress { get; set; }

    [Column(TypeName = "character varying")]
    public string? ItemsToFinance { get; set; }

    [Precision(15, 2)]
    public decimal? AmountRequired { get; set; }

    [Column(TypeName = "character varying")]
    public string? Comments { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? Status { get; set; }

    public DateOnly? FollowUpDate { get; set; }

    [Column("QuoteID")]
    public int? QuoteId { get; set; }

    public int? ApplyStatus { get; set; }

    public int? VendorId { get; set; }

    public int? VendorRepId { get; set; }

    public short? IsAssignTo { get; set; }

    [Column("DealStatusID")]
    public int? DealStatusId { get; set; }

    [Column("ParentDealStatusID")]
    public int? ParentDealStatusId { get; set; }

    [Column("CrmID")]
    public int? CrmId { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsDeleted { get; set; }

    [Column("ABN", TypeName = "character varying")]
    public string? Abn { get; set; }

    [Column(TypeName = "character varying")]
    public string? LegalName { get; set; }

    [Column("ACN", TypeName = "character varying")]
    public string? Acn { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? BorrowingThroughTrust { get; set; }

    [Column(TypeName = "character varying")]
    public string? TrustName { get; set; }

    [Column("PreviousABN", TypeName = "character varying")]
    public string? PreviousAbn { get; set; }

    public int? Industry { get; set; }

    public int? Category { get; set; }

    [Column(TypeName = "character varying")]
    public string? MiddleName { get; set; }

    [Column(TypeName = "character varying")]
    public string? SurName { get; set; }

    [Column(TypeName = "character varying")]
    public string? Role { get; set; }

    [Precision(7, 2)]
    public decimal? ClientIndicatedLengthOfTrade { get; set; }

    [Column("SalesStaffID")]
    public int? SalesStaffId { get; set; }

    public short? Referrer { get; set; }

    [Column(TypeName = "character varying")]
    public string? ReferrerDescription { get; set; }

    [Precision(15, 2)]
    public decimal? AvgMonthlySales { get; set; }

    public int? LeadSource { get; set; }

    public int? LeadAddedBy { get; set; }

    [Column("ACNTrustee", TypeName = "character varying")]
    public string? Acntrustee { get; set; }

    [Column("ABNEstablishedDate")]
    public DateTime? AbnestablishedDate { get; set; }

    [Precision(7, 2)]
    public decimal? YearsEstalished { get; set; }

    [Column(TypeName = "character varying")]
    public string? BusinessType { get; set; }

    [Column("EquipmentCategoryID")]
    public int? EquipmentCategoryId { get; set; }

    [Column(TypeName = "character varying")]
    public string? VendorClientIndicated { get; set; }

    [Column(TypeName = "character varying")]
    public string? VendorSalesRepClientIndicated { get; set; }

    [Column(TypeName = "character varying")]
    public string? EquipmentTypeClientIndicated { get; set; }

    [Column(TypeName = "character varying")]
    public string? VehicleType { get; set; }

    [Column(TypeName = "character varying")]
    public string? EquipmentDescription { get; set; }

    [Column("VendorABN", TypeName = "character varying")]
    public string? VendorAbn { get; set; }

    [Column("GSTStatus")]
    public DateTime? Gststatus { get; set; }

    [Column("ABNStatus", TypeName = "character varying")]
    public string? Abnstatus { get; set; }

    [Column(TypeName = "character varying")]
    public string? EntityType { get; set; }

    [Column("LastABRUpdateNote", TypeName = "character varying")]
    public string? LastAbrupdateNote { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsLeadAssignedToSalesRep { get; set; }

    [Column(TypeName = "character varying")]
    public string? SupplierName { get; set; }

    [Column("IPAddress", TypeName = "character varying")]
    public string? Ipaddress { get; set; }

    [Column("IPLocation", TypeName = "character varying")]
    public string? Iplocation { get; set; }

    [Column(TypeName = "character varying")]
    public string? AccountantName { get; set; }

    [Column(TypeName = "character varying")]
    public string? VendorComments { get; set; }

    [Column("ACNRegistrationDate")]
    public DateTime? AcnregistrationDate { get; set; }

    [Column("ACNStatus", TypeName = "character varying")]
    public string? Acnstatus { get; set; }

    [Column("ACNEntityType", TypeName = "character varying")]
    public string? AcnentityType { get; set; }

    [Column("IsABRASICDown")]
    public bool? IsAbrasicdown { get; set; }

    [Column("IsAmountInclusiveGST")]
    public bool? IsAmountInclusiveGst { get; set; }

    [Column(TypeName = "character varying")]
    public string? BusinessLocation { get; set; }

    [InverseProperty("Lead")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [ForeignKey("Category")]
    [InverseProperty("Leads")]
    public virtual EquipmentCategory1? CategoryNavigation { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("LeadCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [ForeignKey("CrmId")]
    [InverseProperty("Leads")]
    public virtual CrmCode? Crm { get; set; }

    [ForeignKey("DealStatusId")]
    [InverseProperty("Leads")]
    public virtual DealStatus? DealStatus { get; set; }

    [ForeignKey("EquipmentCategoryId")]
    [InverseProperty("Leads")]
    public virtual EquipmentCategory? EquipmentCategory { get; set; }

    [ForeignKey("Industry")]
    [InverseProperty("Leads")]
    public virtual VendorIndustry? IndustryNavigation { get; set; }

    [ForeignKey("LeadSource")]
    [InverseProperty("Leads")]
    public virtual LeadSource? LeadSourceNavigation { get; set; }

    [ForeignKey("ParentDealStatusId")]
    [InverseProperty("Leads")]
    public virtual ParentDealStatus? ParentDealStatus { get; set; }

    [InverseProperty("Lead")]
    public virtual ICollection<Quote> Quotes { get; set; } = new List<Quote>();

    [ForeignKey("SalesStaffId")]
    [InverseProperty("LeadSalesStaffs")]
    public virtual User? SalesStaff { get; set; }

    [ForeignKey("VendorId")]
    [InverseProperty("Leads")]
    public virtual Vendor? Vendor { get; set; }

    [ForeignKey("VendorRepId")]
    [InverseProperty("LeadVendorReps")]
    public virtual User? VendorRep { get; set; }
}
