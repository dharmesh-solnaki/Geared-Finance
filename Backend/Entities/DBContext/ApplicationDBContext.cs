using Entities.Extentions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities.DBContext;

public partial class ApplicationDBContext : DbContext
{
    public ApplicationDBContext()
    {
    }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppSource> AppSources { get; set; }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<CrmCode> CrmCodes { get; set; }

    public virtual DbSet<Deal> Deals { get; set; }

    public virtual DbSet<DealStatus> DealStatuses { get; set; }

    public virtual DbSet<DealVendor> DealVendors { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<EquipmentCategory> EquipmentCategories { get; set; }

    public virtual DbSet<EquipmentCategory1> EquipmentCategories1 { get; set; }

    public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }

    public virtual DbSet<FinanceQuote> FinanceQuotes { get; set; }

    public virtual DbSet<Funder> Funders { get; set; }

    public virtual DbSet<FunderProductFunding> FunderProductFundings { get; set; }

    public virtual DbSet<FunderProductGuide> FunderProductGuides { get; set; }

    public virtual DbSet<FundingCategory> FundingCategories { get; set; }

    public virtual DbSet<FundingEquipmentType> FundingEquipmentTypes { get; set; }

    public virtual DbSet<InterestChart> InterestCharts { get; set; }

    public virtual DbSet<InterestChartFunding> InterestChartFundings { get; set; }

    public virtual DbSet<Lead> Leads { get; set; }

    public virtual DbSet<LeadSource> LeadSources { get; set; }

    public virtual DbSet<ManagerLevel> ManagerLevels { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<ParentDealStatus> ParentDealStatuses { get; set; }

    public virtual DbSet<Quote> Quotes { get; set; }

    public virtual DbSet<RateChartOption> RateChartOptions { get; set; }

    public virtual DbSet<Right> Rights { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    public virtual DbSet<VendorIndustry> VendorIndustries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID = postgres;Password=root;Server=localhost;Port=5432;Database=GearedFinance;Integrated Security=true;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureKeylessEntities();

        modelBuilder.Entity<AppSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AppSource_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Applications_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Appsource).WithMany(p => p.Applications).HasConstraintName("Applications_Appsourceid_fkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Applications).HasConstraintName("Applications_Categoryid_fkey");

            entity.HasOne(d => d.Crm).WithMany(p => p.Applications).HasConstraintName("Applications_Crmid_fkey");

            entity.HasOne(d => d.Dealstatus).WithMany(p => p.Applications).HasConstraintName("Applications_Dealstatusid_fkey");

            entity.HasOne(d => d.Equipmentcategory).WithMany(p => p.Applications).HasConstraintName("Applications_Equipmentcategoryid_fkey");

            entity.HasOne(d => d.Industry).WithMany(p => p.Applications).HasConstraintName("Applications_Industryid_fkey");

            entity.HasOne(d => d.Lead).WithMany(p => p.Applications).HasConstraintName("Applications_Leadid_fkey");

            entity.HasOne(d => d.Parentdealstatus).WithMany(p => p.Applications).HasConstraintName("Applications_Parentdealstatusid_fkey");

            entity.HasOne(d => d.Quote).WithMany(p => p.Applications).HasConstraintName("Applications_Quoteid_fkey");

            entity.HasOne(d => d.Salesstaff).WithMany(p => p.ApplicationSalesstaffs).HasConstraintName("Applications_Salesstaffid_fkey");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Applications).HasConstraintName("Applications_Vendorid_fkey");

            entity.HasOne(d => d.Vendorrep).WithMany(p => p.ApplicationVendorreps).HasConstraintName("Applications_Vendorrepid_fkey");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Clients_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<CrmCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CrmCodes_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Deal>(entity =>
        {
            entity.HasKey(e => e.Iddeals).HasName("Deals_pkey");

            entity.Property(e => e.Iddeals).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.App).WithMany(p => p.Deals).HasConstraintName("Deals_AppID_fkey");

            entity.HasOne(d => d.Crm).WithMany(p => p.Deals).HasConstraintName("Deals_CrmID_fkey");

            entity.HasOne(d => d.ParentDealStatus).WithMany(p => p.Deals).HasConstraintName("Deals_ParentDealStatusID_fkey");

            entity.HasOne(d => d.SalesStaff).WithMany(p => p.Deals).HasConstraintName("Deals_SalesStaffID_fkey");
        });

        modelBuilder.Entity<DealStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DealStatus_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<DealVendor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DealVendors_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Deal).WithMany(p => p.DealVendors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DealVendors_DealID_fkey");

            entity.HasOne(d => d.Vendor).WithMany(p => p.DealVendors).HasConstraintName("DealVendors_VendorID_fkey");

            entity.HasOne(d => d.VendorRep).WithMany(p => p.DealVendors).HasConstraintName("DealVendors_VendorRepID_fkey");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Documents_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Funder).WithMany(p => p.Documents)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FUNDER_DOCUMENT_FUNDERID");
        });

        modelBuilder.Entity<EquipmentCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EquipmentCategory_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<EquipmentCategory1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EquipmentCategories_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<EquipmentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EquipmentType_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<FinanceQuote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FinanceQuote_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Funder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Funder_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<FunderProductFunding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SelectedFunding_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.EquipmentCategory).WithMany(p => p.FunderProductFundings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FUNDINGCATEGORY_SELECTEDFUNDING_EQUIPMENTCATEGORYID");

            entity.HasOne(d => d.Equipment).WithMany(p => p.FunderProductFundings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EQUIPMENT_SELECTEDFUNDING_EUQIPMENTID");

            entity.HasOne(d => d.FundingProductGuide).WithMany(p => p.FunderProductFundings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FUNDERGUIDE_SELECTEDFUNDING_FUNDERGUIDEID");
        });

        modelBuilder.Entity<FunderProductGuide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FunderProductGuide_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Funder).WithOne(p => p.FunderProductGuide)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Funder_Funder_Guide");
        });

        modelBuilder.Entity<FundingCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FundingCategory_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<FundingEquipmentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FundingEquipmentType_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Category).WithMany(p => p.FundingEquipmentTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FundingEquipmentType_FundingCategory");
        });

        modelBuilder.Entity<InterestChart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("InterestChart_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.RateChart).WithMany(p => p.InterestCharts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("InterestChart_RateChartId_fkey");
        });

        modelBuilder.Entity<InterestChartFunding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("InterestChartFundings_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.ChartEquipment).WithMany(p => p.InterestChartFundings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("InterestChartFundings_ChartEquipmentId_fkey");

            entity.HasOne(d => d.EquipmentCategory).WithMany(p => p.InterestChartFundings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("InterestChartFundings_EquipmentCategoryId_fkey");

            entity.HasOne(d => d.Equipment).WithMany(p => p.InterestChartFundings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("InterestChartFundings_EquipmentId_fkey");
        });

        modelBuilder.Entity<Lead>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Leads_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Leads).HasConstraintName("Leads_Category_fkey");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeadCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Leads_CreatedBy_fkey");

            entity.HasOne(d => d.Crm).WithMany(p => p.Leads).HasConstraintName("Leads_CrmID_fkey");

            entity.HasOne(d => d.DealStatus).WithMany(p => p.Leads).HasConstraintName("Leads_DealStatusID_fkey");

            entity.HasOne(d => d.EquipmentCategory).WithMany(p => p.Leads).HasConstraintName("Leads_EquipmentCategoryID_fkey");

            entity.HasOne(d => d.IndustryNavigation).WithMany(p => p.Leads).HasConstraintName("Leads_Industry_fkey");

            entity.HasOne(d => d.LeadSourceNavigation).WithMany(p => p.Leads).HasConstraintName("Leads_LeadSource_fkey");

            entity.HasOne(d => d.ParentDealStatus).WithMany(p => p.Leads).HasConstraintName("Leads_ParentDealStatusID_fkey");

            entity.HasOne(d => d.SalesStaff).WithMany(p => p.LeadSalesStaffs).HasConstraintName("Leads_SalesStaffID_fkey");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Leads).HasConstraintName("Leads_VendorId_fkey");

            entity.HasOne(d => d.VendorRep).WithMany(p => p.LeadVendorReps).HasConstraintName("Leads_VendorRepId_fkey");
        });

        modelBuilder.Entity<LeadSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LeadSource_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ManagerLevel>(entity =>
        {
            entity.HasOne(d => d.Vendor).WithMany(p => p.ManagerLevels).HasConstraintName("FK_ManagerLevels_Vendors_ManagerId");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Module_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Notes_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Notes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notes_CreatedBy_fkey");

            entity.HasOne(d => d.Funder).WithMany(p => p.Notes).HasConstraintName("FK_FUNDER_NOTES_FUNDERID");
        });

        modelBuilder.Entity<ParentDealStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ParentDealStatus_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Quote_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Crm).WithMany(p => p.Quotes).HasConstraintName("Quotes_CrmID_fkey");

            entity.HasOne(d => d.DealStatus).WithMany(p => p.Quotes).HasConstraintName("Quotes_DealStatusID_fkey");

            entity.HasOne(d => d.EquipmentCategory).WithMany(p => p.Quotes).HasConstraintName("Quotes_EquipmentCategoryID_fkey");

            entity.HasOne(d => d.Equipment).WithMany(p => p.QuoteEquipments).HasConstraintName("Quotes_EquipmentID_fkey");

            entity.HasOne(d => d.EquipmentType).WithMany(p => p.QuoteEquipmentTypes).HasConstraintName("Quotes_EquipmentTypeID_fkey");

            entity.HasOne(d => d.Lead).WithMany(p => p.Quotes).HasConstraintName("Quotes_LeadID_fkey");

            entity.HasOne(d => d.ParentDealStatus).WithMany(p => p.Quotes).HasConstraintName("Quotes_ParentDealStatusID_fkey");

            entity.HasOne(d => d.SalesStaff).WithMany(p => p.Quotes).HasConstraintName("Quotes_SalesStaffID_fkey");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Quotes).HasConstraintName("Quotes_VendorId_fkey");
        });

        modelBuilder.Entity<RateChartOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RateChartOptions_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Funder).WithMany(p => p.RateChartOptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RateChartOptions_FunderId_fkey");
        });

        modelBuilder.Entity<Right>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Rights_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CanAdd).HasDefaultValueSql("false");
            entity.Property(e => e.CanDelete).HasDefaultValueSql("false");
            entity.Property(e => e.CanEdit).HasDefaultValueSql("false");
            entity.Property(e => e.CanView).HasDefaultValueSql("false");

            entity.HasOne(d => d.Module).WithMany(p => p.Rights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Module_Rights");

            entity.HasOne(d => d.Role).WithMany(p => p.Rights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_Rights");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.FullName).HasComputedColumnSql("(((\"Name\")::text || ' '::text) || (\"SurName\")::text)", true);
            entity.Property(e => e.IsCalcRateEditor).HasDefaultValueSql("true");
            entity.Property(e => e.IsFunderProfile).HasDefaultValueSql("true");
            entity.Property(e => e.IsPortalLogin).HasDefaultValueSql("false");
            entity.Property(e => e.IsProceedBtnInApp).HasDefaultValueSql("true");
            entity.Property(e => e.IsSendEndOfTermReport).HasDefaultValueSql("true");
            entity.Property(e => e.IsUserInGafsalesRepList).HasDefaultValueSql("false");
            entity.Property(e => e.IsUserInVendorSalesRepList).HasDefaultValueSql("true");
            entity.Property(e => e.Status).HasDefaultValueSql("true");
            entity.Property(e => e.UnassignedApplications).HasDefaultValueSql("true");

            entity.HasOne(d => d.Manager).WithMany(p => p.Users).HasConstraintName("FK_Users_ManagerLevels_MangerId");

            entity.HasOne(d => d.RelationshipManagerNavigation).WithMany(p => p.InverseRelationshipManagerNavigation).HasConstraintName("FK_User_UserId");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles_RolesId");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Users).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<VendorIndustry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VendorIndustry_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
