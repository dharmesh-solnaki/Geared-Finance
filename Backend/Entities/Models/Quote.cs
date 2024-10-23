using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Quote
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string? ClientName { get; set; }

    [Precision(10, 2)]
    public decimal? Amount { get; set; }

    [Column(TypeName = "character varying")]
    public string? RepaymentTerm { get; set; }

    [Precision(10, 2)]
    public decimal? RepaymentMonth { get; set; }

    public short? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    [Column(TypeName = "character varying")]
    public string? EmailAddress { get; set; }

    [StringLength(10)]
    public string? MobileNumber { get; set; }

    [Column("EquipmentID")]
    public int? EquipmentId { get; set; }

    [Column("InclusiveGST", TypeName = "bit(1)")]
    public BitArray? InclusiveGst { get; set; }

    [Column("FinanceQuoteID")]
    public short? FinanceQuoteId { get; set; }

    [Precision(10, 2)]
    public decimal? NetAmount { get; set; }

    [Column("GSTValue")]
    [Precision(5, 2)]
    public decimal? Gstvalue { get; set; }

    [Precision(7, 2)]
    public decimal? RepaymentWeek { get; set; }

    public int? VendorId { get; set; }

    public int? VendorRepId { get; set; }

    public bool? IsAssignTo { get; set; }

    [Column(TypeName = "character varying")]
    public string? ContactName { get; set; }

    [Column("InclusiveGSTAmount")]
    [Precision(10, 2)]
    public decimal? InclusiveGstamount { get; set; }

    [Column("ExclusiveGSTAmount")]
    [Precision(10, 2)]
    public decimal? ExclusiveGstamount { get; set; }

    [Precision(7, 2)]
    public decimal? EnteredCurrentEnergyBill { get; set; }

    [Precision(7, 2)]
    public decimal? EnteredEstimatedEnergyBillWithSolar { get; set; }

    [Precision(7, 2)]
    public decimal? CurrentEnergyBill { get; set; }

    [Precision(7, 2)]
    public decimal? EnergyBillWithSolar { get; set; }

    [Column(TypeName = "character varying")]
    public string? SelectedChartTerm { get; set; }

    [Column(TypeName = "character varying")]
    public string? Repayment { get; set; }

    [Precision(7, 2)]
    public decimal? AnnualRepayment { get; set; }

    [Precision(7, 2)]
    public decimal? NetSaving { get; set; }

    [Precision(10, 0)]
    public decimal? Deposit { get; set; }

    [Precision(5, 2)]
    public decimal? BrokeragePercent { get; set; }

    public int? ResidualPercent { get; set; }

    [Precision(8, 2)]
    public decimal? ResidualDollar { get; set; }

    [Column("FunderID")]
    public int? FunderId { get; set; }

    [Precision(8, 2)]
    public decimal? BrokerageDollar { get; set; }

    [Column("GSTBrok")]
    [Precision(5, 2)]
    public decimal? Gstbrok { get; set; }

    [Precision(5, 2)]
    public decimal? TotalBrok { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsAdvancedPayment { get; set; }

    [Column("IsGSTExemptItem", TypeName = "bit(1)")]
    public BitArray? IsGstexemptItem { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsAdvanceCalculator { get; set; }

    [Column("ABNNumber", TypeName = "character varying")]
    public string? Abnnumber { get; set; }

    [Column("DealStatusID")]
    public int? DealStatusId { get; set; }

    [Column("ParentDealStatusID")]
    public int? ParentDealStatusId { get; set; }

    [Column("LeadID")]
    public int? LeadId { get; set; }

    public short? TradingLength { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsPropertyOwner { get; set; }

    [Column(TypeName = "character varying")]
    public string? StarRating { get; set; }

    [Column(TypeName = "character varying")]
    public string? ContactSurname { get; set; }

    public short? Version { get; set; }

    [Column("MainQuoteID")]
    public int? MainQuoteId { get; set; }

    [Column("CrmID")]
    public int? CrmId { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsDeleted { get; set; }

    [Column("SalesStaffID")]
    public int? SalesStaffId { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsQuoteFromGeneralApplication { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsQuoteFromClientProfile { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [Column("EquipmentCategoryID")]
    public int? EquipmentCategoryId { get; set; }

    [Column("EquipmentTypeID")]
    public int? EquipmentTypeId { get; set; }

    [Precision(5, 2)]
    public decimal? DiscountRate { get; set; }

    public DateTime? DiscountValidDate { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsDiscountSaved { get; set; }

    [Column("IsDepositInclusiveGST", TypeName = "bit(1)")]
    public BitArray? IsDepositInclusiveGst { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsEquipmentSelectedManually { get; set; }

    [Precision(5, 2)]
    public decimal? IncentivePercent { get; set; }

    public DateTime? QuoteAssignedToSalesRepDate { get; set; }

    [Precision(5, 2)]
    public decimal? Premium { get; set; }

    [Precision(4, 2)]
    public decimal? DepositPercent { get; set; }

    [InverseProperty("Quote")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [ForeignKey("CrmId")]
    [InverseProperty("Quotes")]
    public virtual CrmCode? Crm { get; set; }

    [ForeignKey("DealStatusId")]
    [InverseProperty("Quotes")]
    public virtual DealStatus? DealStatus { get; set; }

    [ForeignKey("EquipmentId")]
    [InverseProperty("QuoteEquipments")]
    public virtual EquipmentType? Equipment { get; set; }

    [ForeignKey("EquipmentCategoryId")]
    [InverseProperty("Quotes")]
    public virtual EquipmentCategory? EquipmentCategory { get; set; }

    [ForeignKey("EquipmentTypeId")]
    [InverseProperty("QuoteEquipmentTypes")]
    public virtual EquipmentType? EquipmentType { get; set; }

    [ForeignKey("LeadId")]
    [InverseProperty("Quotes")]
    public virtual Lead? Lead { get; set; }

    [ForeignKey("ParentDealStatusId")]
    [InverseProperty("Quotes")]
    public virtual ParentDealStatus? ParentDealStatus { get; set; }

    [ForeignKey("SalesStaffId")]
    [InverseProperty("Quotes")]
    public virtual User? SalesStaff { get; set; }

    [ForeignKey("VendorId")]
    [InverseProperty("Quotes")]
    public virtual Vendor? Vendor { get; set; }
}
