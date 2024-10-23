using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Deal
{
    [Key]
    [Column("IDDeals")]
    public int Iddeals { get; set; }

    [Column("Client ID")]
    public int? ClientId { get; set; }

    [Column("amountexcgst")]
    public decimal? Amountexcgst { get; set; }

    [Column("amountincgst")]
    public decimal? Amountincgst { get; set; }

    [Column("interestrate")]
    public decimal? Interestrate { get; set; }

    [Column("term")]
    public int? Term { get; set; }

    [Column("brokgperct")]
    public decimal? Brokgperct { get; set; }

    [Column("brokdollars")]
    public decimal? Brokdollars { get; set; }

    [Column("residualperc")]
    public decimal? Residualperc { get; set; }

    [Column("residualdoll")]
    public decimal? Residualdoll { get; set; }

    [Column("displaybrok")]
    [StringLength(100)]
    public string? Displaybrok { get; set; }

    [Column("typefinance")]
    [StringLength(100)]
    public string? Typefinance { get; set; }

    [Column("financier")]
    public int? Financier { get; set; }

    [Column("equipment")]
    [StringLength(255)]
    public string? Equipment { get; set; }

    [Column("netpmt")]
    public decimal? Netpmt { get; set; }

    [Column("gstpmt")]
    public decimal? Gstpmt { get; set; }

    [Column("totalpmt")]
    public decimal? Totalpmt { get; set; }

    [Column("spivpayable")]
    public int? Spivpayable { get; set; }

    [Column("spivamountdoll")]
    public decimal? Spivamountdoll { get; set; }

    [Column("spivamountperc")]
    public decimal? Spivamountperc { get; set; }

    [Column("spiv")]
    public int? Spiv { get; set; }

    [Column("vendor")]
    public string? Vendor { get; set; }

    [Column("vendbranch")]
    [StringLength(100)]
    public string? Vendbranch { get; set; }

    [Column("vendcontact")]
    [StringLength(100)]
    public string? Vendcontact { get; set; }

    [Column("vendph")]
    [StringLength(20)]
    public string? Vendph { get; set; }

    [Column("vendemail")]
    [StringLength(255)]
    public string? Vendemail { get; set; }

    [Column("reasonpurch")]
    [StringLength(255)]
    public string? Reasonpurch { get; set; }

    [Column("equipdesc")]
    public string? Equipdesc { get; set; }

    [Column("dealstatq1")]
    [StringLength(50)]
    public string? Dealstatq1 { get; set; }

    [Column("invoiceno")]
    [StringLength(50)]
    public string? Invoiceno { get; set; }

    [Column("processor")]
    [StringLength(100)]
    public string? Processor { get; set; }

    [Column("diarynotes")]
    public string? Diarynotes { get; set; }

    [Column("program")]
    [StringLength(100)]
    public string? Program { get; set; }

    [Column("netpmtrev")]
    public decimal? Netpmtrev { get; set; }

    [Column("gstpmtrev")]
    public decimal? Gstpmtrev { get; set; }

    [Column("totalpmtrev")]
    public decimal? Totalpmtrev { get; set; }

    [Column("netdffund")]
    public decimal? Netdffund { get; set; }

    [Column("gstdffund")]
    public decimal? Gstdffund { get; set; }

    [Column("totaldffund")]
    public decimal? Totaldffund { get; set; }

    [Column("netdfgaf")]
    public decimal? Netdfgaf { get; set; }

    [Column("gstdfgaf")]
    public decimal? Gstdfgaf { get; set; }

    [Column("totaldfgaf")]
    public decimal? Totaldfgaf { get; set; }

    [Column("settledate")]
    public DateTime? Settledate { get; set; }

    [Column("expiredate")]
    public DateTime? Expiredate { get; set; }

    [Column("diarystamps", TypeName = "character varying")]
    public string? Diarystamps { get; set; }

    [Column("brokinvnumber")]
    public int? Brokinvnumber { get; set; }

    [Column("otherfee")]
    [StringLength(100)]
    public string? Otherfee { get; set; }

    [Column("otherfeeamt")]
    public decimal? Otherfeeamt { get; set; }

    [Column("postsettdate")]
    public DateTime? Postsettdate { get; set; }

    [Column("postsettfinref")]
    [StringLength(100)]
    public string? Postsettfinref { get; set; }

    [Column("postsettendnote")]
    public string? Postsettendnote { get; set; }

    [Column("postsettorigheld")]
    [StringLength(100)]
    public string? Postsettorigheld { get; set; }

    [Column("postsettorigstofunder")]
    [StringLength(100)]
    public string? Postsettorigstofunder { get; set; }

    [Column("postsettorigs")]
    [StringLength(100)]
    public string? Postsettorigs { get; set; }

    [Column("postsetttracking")]
    [StringLength(100)]
    public string? Postsetttracking { get; set; }

    [Column("postsettcopyclient")]
    [StringLength(100)]
    public string? Postsettcopyclient { get; set; }

    [Column("postsettcopydate")]
    public DateTime? Postsettcopydate { get; set; }

    [Column("postsettscan")]
    public string? Postsettscan { get; set; }

    [Column("postspivpaid", TypeName = "bit(1)")]
    public BitArray? Postspivpaid { get; set; }

    [Column("flexitimediryrs")]
    public int? Flexitimediryrs { get; set; }

    [Column("flexitimedirmths")]
    public int? Flexitimedirmths { get; set; }

    [Column("flexitimeindyrs")]
    public int? Flexitimeindyrs { get; set; }

    [Column("flexitimeindmths")]
    public int? Flexitimeindmths { get; set; }

    [Column("flexidirwage")]
    public decimal? Flexidirwage { get; set; }

    [Column("flexicombdirwage")]
    public decimal? Flexicombdirwage { get; set; }

    [Column("flexiaddincome")]
    public decimal? Flexiaddincome { get; set; }

    [Column("flexiaddtype")]
    [StringLength(100)]
    public string? Flexiaddtype { get; set; }

    [Column("flexiaust", TypeName = "character varying")]
    public string? Flexiaust { get; set; }

    [Column("flexinz", TypeName = "character varying")]
    public string? Flexinz { get; set; }

    [Column("flexiperm", TypeName = "character varying")]
    public string? Flexiperm { get; set; }

    [Column("flexitemp", TypeName = "character varying")]
    public string? Flexitemp { get; set; }

    [Column("flexiother")]
    [StringLength(100)]
    public string? Flexiother { get; set; }

    [Column("fleximarried", TypeName = "bit(1)")]
    public BitArray? Fleximarried { get; set; }

    [Column("flexidivorced", TypeName = "bit(1)")]
    public BitArray? Flexidivorced { get; set; }

    [Column("flexisingle", TypeName = "bit(1)")]
    public BitArray? Flexisingle { get; set; }

    [Column("flexiwidow", TypeName = "bit(1)")]
    public BitArray? Flexiwidow { get; set; }

    [Column("flexitimeadd")]
    public decimal? Flexitimeadd { get; set; }

    [Column("flexiprev")]
    public decimal? Flexiprev { get; set; }

    [Column("flexistate")]
    [StringLength(100)]
    public string? Flexistate { get; set; }

    [Column("flexipcode")]
    [StringLength(20)]
    public string? Flexipcode { get; set; }

    [Column("flexiown", TypeName = "character varying")]
    public string? Flexiown { get; set; }

    [Column("flexibuy", TypeName = "character varying")]
    public string? Flexibuy { get; set; }

    [Column("flexirent", TypeName = "character varying")]
    public string? Flexirent { get; set; }

    [Column("flexiboard", TypeName = "character varying")]
    public string? Flexiboard { get; set; }

    [Column("flexiresother")]
    [StringLength(100)]
    public string? Flexiresother { get; set; }

    [Column("flexirentpay")]
    public decimal? Flexirentpay { get; set; }

    [Column("fleximortpay")]
    public decimal? Fleximortpay { get; set; }

    [Column("fleximortbank")]
    [StringLength(100)]
    public string? Fleximortbank { get; set; }

    [Column("flexispouseinc")]
    public decimal? Flexispouseinc { get; set; }

    [Column("flexidepend")]
    public decimal? Flexidepend { get; set; }

    [Column("flexicctype1")]
    [StringLength(100)]
    public string? Flexicctype1 { get; set; }

    [Column("flexiccbank1")]
    [StringLength(100)]
    public string? Flexiccbank1 { get; set; }

    [Column("flexicclim1")]
    public decimal? Flexicclim1 { get; set; }

    [Column("flexicctype2")]
    [StringLength(100)]
    public string? Flexicctype2 { get; set; }

    [Column("flexiccbank2")]
    [StringLength(100)]
    public string? Flexiccbank2 { get; set; }

    [Column("flexicclim2")]
    public decimal? Flexicclim2 { get; set; }

    [Column("flexicctype3")]
    [StringLength(100)]
    public string? Flexicctype3 { get; set; }

    [Column("flexiccbank3")]
    [StringLength(100)]
    public string? Flexiccbank3 { get; set; }

    [Column("flexicclim3")]
    public decimal? Flexicclim3 { get; set; }

    [Column("flexicctype4")]
    [StringLength(100)]
    public string? Flexicctype4 { get; set; }

    [Column("flexiccbank4")]
    [StringLength(100)]
    public string? Flexiccbank4 { get; set; }

    [Column("flexicclim4")]
    public decimal? Flexicclim4 { get; set; }

    [Column("flexiloantype1")]
    [StringLength(100)]
    public string? Flexiloantype1 { get; set; }

    [Column("flexiloanbank1")]
    [StringLength(100)]
    public string? Flexiloanbank1 { get; set; }

    [Column("flexiloanpay1")]
    public decimal? Flexiloanpay1 { get; set; }

    [Column("flexiloantype2")]
    [StringLength(100)]
    public string? Flexiloantype2 { get; set; }

    [Column("flexiloanpay3")]
    public decimal? Flexiloanpay3 { get; set; }

    [Column("spare1")]
    [StringLength(255)]
    public string? Spare1 { get; set; }

    [Column("spare2")]
    [StringLength(255)]
    public string? Spare2 { get; set; }

    [Column("spare3")]
    [StringLength(255)]
    public string? Spare3 { get; set; }

    [Column("spare4")]
    [StringLength(255)]
    public string? Spare4 { get; set; }

    [Column("spare5")]
    [StringLength(255)]
    public string? Spare5 { get; set; }

    [Column("spare6")]
    [StringLength(255)]
    public string? Spare6 { get; set; }

    [Column("spare7")]
    [StringLength(255)]
    public string? Spare7 { get; set; }

    [Column("spare8")]
    [StringLength(255)]
    public string? Spare8 { get; set; }

    [Column("spare9")]
    [StringLength(255)]
    public string? Spare9 { get; set; }

    [Column("spare10")]
    [StringLength(255)]
    public string? Spare10 { get; set; }

    [Column("createddate")]
    public DateTime? Createddate { get; set; }

    [Column("showinportal", TypeName = "bit(1)")]
    public BitArray? Showinportal { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? MultipleVendors { get; set; }

    public int? Numberofvendor { get; set; }

    [StringLength(255)]
    public string? VendorOne { get; set; }

    public decimal? AmountVendorOne { get; set; }

    [StringLength(255)]
    public string? VendorTwo { get; set; }

    public decimal? AmountVendorTwo { get; set; }

    [StringLength(255)]
    public string? VendorThree { get; set; }

    public decimal? AmountVendorThree { get; set; }

    [StringLength(255)]
    public string? VendorFour { get; set; }

    public decimal? AmountVendorFour { get; set; }

    public decimal? DepositAmount { get; set; }

    [Column("IsDepositInclusiveGST", TypeName = "bit(1)")]
    public BitArray? IsDepositInclusiveGst { get; set; }

    public decimal? NetAmountFinance { get; set; }

    [StringLength(100)]
    public string? ContractNumber { get; set; }

    public DateTime? PayoutDate { get; set; }

    public string? PostSettlementDetail1 { get; set; }

    public string? PostSettlementDetail2 { get; set; }

    public string? PostSettlementDetail3 { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsOnceOffDeal { get; set; }

    [Column("IsGSTExemptItem", TypeName = "bit(1)")]
    public BitArray? IsGstexemptItem { get; set; }

    [StringLength(100)]
    public string? VendorBankName { get; set; }

    [Column("VendorBSB")]
    [StringLength(10)]
    public string? VendorBsb { get; set; }

    [StringLength(50)]
    public string? VendorAccountNumber { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsAdvancedPayment { get; set; }

    [Column("gstbrok")]
    public decimal? Gstbrok { get; set; }

    [Column("totalbrok")]
    public decimal? Totalbrok { get; set; }

    [StringLength(255)]
    public string? VendorFive { get; set; }

    public decimal? AmountVendorFive { get; set; }

    [StringLength(255)]
    public string? VendorSix { get; set; }

    public decimal? AmountVendorSix { get; set; }

    [StringLength(255)]
    public string? VendorSeven { get; set; }

    public decimal? AmountVendorSeven { get; set; }

    [StringLength(255)]
    public string? VendorEight { get; set; }

    public decimal? AmountVendorEight { get; set; }

    [StringLength(255)]
    public string? VendorNine { get; set; }

    public decimal? AmountVendorNine { get; set; }

    [StringLength(255)]
    public string? VendorTen { get; set; }

    public decimal? AmountVendorTen { get; set; }

    [Column("tradingaddress")]
    public int? Tradingaddress { get; set; }

    [Column("SPIVPaidTo")]
    public short? SpivpaidTo { get; set; }

    [Column("VendorSPIV")]
    public decimal? VendorSpiv { get; set; }

    [Column("VendorSalesRepSPIV")]
    public decimal? VendorSalesRepSpiv { get; set; }

    [Column("ParentDealStatusID")]
    public int? ParentDealStatusId { get; set; }

    public DateTime? ApprovedDate { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsDeleted { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    [Column("AppID")]
    public int? AppId { get; set; }

    [Column("CrmID")]
    public int? CrmId { get; set; }

    [Column("SalesStaffID")]
    public int? SalesStaffId { get; set; }

    [StringLength(100)]
    public string? PurchaseNature { get; set; }

    [StringLength(100)]
    public string? PreviousContractNumber { get; set; }

    [Column("EOTConditions")]
    public string? Eotconditions { get; set; }

    [Column("EOTNotes")]
    public string? Eotnotes { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsPaidOut { get; set; }

    public string? PaidoutNotes { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsClientInArrears { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsClientInLiquidation { get; set; }

    public string? ClientArrearsNotes { get; set; }

    public string? ClientLiquidationNotes { get; set; }

    public string? AdminIds { get; set; }

    public string? OldDealDiaryNote { get; set; }

    [Column("IsRITCFeeApplied", TypeName = "bit(1)")]
    public BitArray? IsRitcfeeApplied { get; set; }

    [Column("RITCFee")]
    public decimal? Ritcfee { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsAccountKeepingFeeApplied { get; set; }

    public decimal? AccountKeepingFee { get; set; }

    public short? IsDocFeeProcessed { get; set; }

    [Column("IsSPIVInput")]
    public short? IsSpivinput { get; set; }

    public short? IsWelcomePackSent { get; set; }

    public decimal? IncentivePercent { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? IsDocumentFeeApplied { get; set; }

    public decimal? DocumentFee { get; set; }

    [StringLength(100)]
    public string? ApplicationNumber { get; set; }

    public decimal? DepositPercent { get; set; }

    [StringLength(255)]
    public string? AppDropboxUrl { get; set; }

    [ForeignKey("AppId")]
    [InverseProperty("Deals")]
    public virtual Application? App { get; set; }

    [ForeignKey("CrmId")]
    [InverseProperty("Deals")]
    public virtual CrmCode? Crm { get; set; }

    [InverseProperty("Deal")]
    public virtual ICollection<DealVendor> DealVendors { get; set; } = new List<DealVendor>();

    [ForeignKey("ParentDealStatusId")]
    [InverseProperty("Deals")]
    public virtual ParentDealStatus? ParentDealStatus { get; set; }

    [ForeignKey("SalesStaffId")]
    [InverseProperty("Deals")]
    public virtual User? SalesStaff { get; set; }
}
