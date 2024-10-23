using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Application
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int? Applicationid { get; set; }

    [StringLength(11)]
    public string? Abnnumber { get; set; }

    [StringLength(255)]
    public string? Entityname { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [Column("statusdate")]
    public DateTime? Statusdate { get; set; }

    public DateTime? Gststatus { get; set; }

    [StringLength(15)]
    public string? Acn { get; set; }

    [StringLength(100)]
    public string? Entitytype { get; set; }

    [StringLength(100)]
    public string? Businesstype { get; set; }

    [StringLength(255)]
    public string? Businessaddress { get; set; }

    [StringLength(20)]
    public string? Businessphonenumber { get; set; }

    [StringLength(100)]
    public string? Firstname { get; set; }

    [StringLength(100)]
    public string? Lastname { get; set; }

    [StringLength(255)]
    public string? Homeaddress { get; set; }

    public DateTime? Dateofbirth { get; set; }

    [StringLength(50)]
    public string? Driverlicensenumber { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isdirector { get; set; }

    public int? Equipmentid { get; set; }

    public decimal? Amountoffinance { get; set; }

    public string? Comments { get; set; }

    [StringLength(255)]
    public string? Clientname { get; set; }

    public DateTime? Date { get; set; }

    [StringLength(255)]
    public string? Signature { get; set; }

    [StringLength(50)]
    public string? Locationstate { get; set; }

    public short? Creditscore { get; set; }

    [StringLength(100)]
    public string? Datasource { get; set; }

    public int? Createdby { get; set; }

    public DateTime? Createddate { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isdeleted { get; set; }

    public int? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public int? Dealstatusid { get; set; }

    [StringLength(15)]
    public string? Secondaryabnnumber { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Ispartiallysaved { get; set; }

    public int? Quoteid { get; set; }

    [StringLength(255)]
    public string? Secondaryentityname { get; set; }

    public DateTime? Secondarystatusdate { get; set; }

    [StringLength(255)]
    public string? Emailaddress { get; set; }

    public int? Leadid { get; set; }

    public int? Vendorid { get; set; }

    public int? Vendorrepid { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isassignto { get; set; }

    [StringLength(255)]
    public string? Tradingname { get; set; }

    [StringLength(255)]
    public string? Contactname { get; set; }

    [StringLength(20)]
    public string? Mobilenumber { get; set; }

    public int? Parentdealstatusid { get; set; }

    public DateTime? Approveddate { get; set; }

    public DateTime? Settledate { get; set; }

    public int? Crmid { get; set; }

    [StringLength(100)]
    public string? Suburb { get; set; }

    [StringLength(50)]
    public string? State { get; set; }

    [StringLength(10)]
    public string? Postcode { get; set; }

    public decimal? Clientindicatedlengthoftrade { get; set; }

    [StringLength(100)]
    public string? Contactmiddlename { get; set; }

    [StringLength(100)]
    public string? Contactsurname { get; set; }

    [StringLength(100)]
    public string? Role { get; set; }

    public int? Industryid { get; set; }

    public int? Categoryid { get; set; }

    [StringLength(100)]
    public string? Middlename { get; set; }

    [StringLength(255)]
    public string? Directoremail { get; set; }

    public DateTime? Licenceexpiry { get; set; }

    [StringLength(255)]
    public string? Directorresidentialaddress { get; set; }

    [StringLength(100)]
    public string? Directorsuburb { get; set; }

    [StringLength(50)]
    public string? Directorstate { get; set; }

    [StringLength(10)]
    public string? Directorpostcode { get; set; }

    public DateTime? Dateataddress { get; set; }

    public int? Timeaddyears { get; set; }

    public int? Timeaddmonths { get; set; }

    public decimal? Propvalue { get; set; }

    public decimal? Mortvalue { get; set; }

    public string? Equipmentdescription { get; set; }

    public int? Salesstaffid { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Borrowingthroughtrust { get; set; }

    [StringLength(255)]
    public string? Trustname { get; set; }

    [StringLength(15)]
    public string? Previousabn { get; set; }

    public int? Referrer { get; set; }

    public string? Referrerdescription { get; set; }

    public decimal? Avgmonthlysales { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Ismultipledirector { get; set; }

    [StringLength(100)]
    public string? Director2firstname { get; set; }

    [StringLength(100)]
    public string? Director2middlename { get; set; }

    [StringLength(100)]
    public string? Director2lastname { get; set; }

    [StringLength(255)]
    public string? Director2email { get; set; }

    [StringLength(20)]
    public string? Director2mobilenumber { get; set; }

    public DateTime? Director2dateofbirth { get; set; }

    [StringLength(50)]
    public string? Director2licencenumber { get; set; }

    public DateTime? Director2licenceexpiry { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isdirector2 { get; set; }

    [StringLength(255)]
    public string? Director2residentialaddress { get; set; }

    [StringLength(100)]
    public string? Director2suburb { get; set; }

    [StringLength(50)]
    public string? Director2state { get; set; }

    [StringLength(10)]
    public string? Director2postcode { get; set; }

    public DateTime? Director2dateataddress { get; set; }

    public int? Director2timeaddyears { get; set; }

    public int? Director2timeaddmonths { get; set; }

    public decimal? Director2propvalue { get; set; }

    public decimal? Director2mortvalue { get; set; }

    [StringLength(15)]
    public string? Acntrustee { get; set; }

    public decimal? Yearsestablished { get; set; }

    public int? Equipmentcategoryid { get; set; }

    [StringLength(100)]
    public string? Vendorclientindicated { get; set; }

    [StringLength(100)]
    public string? Vendorsalesrepclientindicated { get; set; }

    [StringLength(100)]
    public string? Equipmenttypeclientindicated { get; set; }

    [StringLength(100)]
    public string? Vehicletype { get; set; }

    [StringLength(255)]
    public string? Signature2 { get; set; }

    [StringLength(255)]
    public string? Clientname2 { get; set; }

    public DateTime? Date2 { get; set; }

    [StringLength(255)]
    public string? Director1urlforprivacyconsent { get; set; }

    [StringLength(255)]
    public string? Director2urlforprivacyconsent { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isdirector1urlexpired { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isdirector2urlexpired { get; set; }

    [StringLength(50)]
    public string? Guid { get; set; }

    public string? Note { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isclientreconciled { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isdirector1reconciled { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isvendorreconciled { get; set; }

    [StringLength(15)]
    public string? Vendorabn { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isdirector2reconciled { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isgeneralapplication { get; set; }

    [StringLength(50)]
    public string? Director1otp { get; set; }

    [StringLength(50)]
    public string? Director2otp { get; set; }

    public DateTime? Director1mobileverifieddate { get; set; }

    public DateTime? Director2mobileverifieddate { get; set; }

    [Column(TypeName = "character varying")]
    public string? Adminids { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isapplicationassignedtogeneraladminqueue { get; set; }

    [StringLength(255)]
    public string? Businesslocation { get; set; }

    public string? Lastabrupdatenote { get; set; }

    [StringLength(100)]
    public string? Director1documenttype { get; set; }

    public decimal? Director1rentmonth { get; set; }

    [StringLength(100)]
    public string? Director2documenttype { get; set; }

    public decimal? Director2rentmonth { get; set; }

    public int? Appsourceid { get; set; }

    [StringLength(50)]
    public string? Director1cardnumber { get; set; }

    [StringLength(100)]
    public string? Director1countryofissue { get; set; }

    [StringLength(50)]
    public string? Director2cardnumber { get; set; }

    [StringLength(100)]
    public string? Director2countryofissue { get; set; }

    [StringLength(255)]
    public string? Suppliername { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Ismailsenttogeneraladminteamqueue { get; set; }

    [StringLength(100)]
    public string? Director1privacyconsentsource { get; set; }

    [StringLength(100)]
    public string? Director2privacyconsentsource { get; set; }

    [StringLength(50)]
    public string? Director1ipaddress { get; set; }

    [StringLength(100)]
    public string? Director1iplocation { get; set; }

    [StringLength(50)]
    public string? Director2ipaddress { get; set; }

    [StringLength(100)]
    public string? Director2iplocation { get; set; }

    [StringLength(100)]
    public string? Director1residencystatus { get; set; }

    public string? Director1residencyinfo { get; set; }

    [StringLength(100)]
    public string? Director2residencystatus { get; set; }

    public string? Director2residencyinfo { get; set; }

    [StringLength(255)]
    public string? Accountantname { get; set; }

    public decimal? Repaymentperiodclientindicated { get; set; }

    public string? Privacyconsentcomments { get; set; }

    [StringLength(255)]
    public string? Equipmentaddress { get; set; }

    [StringLength(100)]
    public string? Equipmentsuburb { get; set; }

    [StringLength(10)]
    public string? Equipmentpostcode { get; set; }

    [StringLength(50)]
    public string? Equipmentstate { get; set; }

    public DateTime? Acnregistrationdate { get; set; }

    [StringLength(50)]
    public string? Acnstatus { get; set; }

    [StringLength(100)]
    public string? Acnentitytype { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isabrasicdown { get; set; }

    [Column(TypeName = "bit(1)")]
    public BitArray? Isamountinclusivegst { get; set; }

    [ForeignKey("Appsourceid")]
    [InverseProperty("Applications")]
    public virtual AppSource? Appsource { get; set; }

    [ForeignKey("Categoryid")]
    [InverseProperty("Applications")]
    public virtual EquipmentCategory1? Category { get; set; }

    [ForeignKey("Crmid")]
    [InverseProperty("Applications")]
    public virtual CrmCode? Crm { get; set; }

    [InverseProperty("App")]
    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    [ForeignKey("Dealstatusid")]
    [InverseProperty("Applications")]
    public virtual DealStatus? Dealstatus { get; set; }

    [ForeignKey("Equipmentcategoryid")]
    [InverseProperty("Applications")]
    public virtual EquipmentCategory? Equipmentcategory { get; set; }

    [ForeignKey("Industryid")]
    [InverseProperty("Applications")]
    public virtual VendorIndustry? Industry { get; set; }

    [ForeignKey("Leadid")]
    [InverseProperty("Applications")]
    public virtual Lead? Lead { get; set; }

    [ForeignKey("Parentdealstatusid")]
    [InverseProperty("Applications")]
    public virtual ParentDealStatus? Parentdealstatus { get; set; }

    [ForeignKey("Quoteid")]
    [InverseProperty("Applications")]
    public virtual Quote? Quote { get; set; }

    [ForeignKey("Salesstaffid")]
    [InverseProperty("ApplicationSalesstaffs")]
    public virtual User? Salesstaff { get; set; }

    [ForeignKey("Vendorid")]
    [InverseProperty("Applications")]
    public virtual Vendor? Vendor { get; set; }

    [ForeignKey("Vendorrepid")]
    [InverseProperty("ApplicationVendorreps")]
    public virtual User? Vendorrep { get; set; }
}
