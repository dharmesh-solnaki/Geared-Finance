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

    public virtual DbSet<FundingCategory> FundingCategories { get; set; }

    public virtual DbSet<FundingEquipmentType> FundingEquipmentTypes { get; set; }

    public virtual DbSet<ManagerLevel> ManagerLevels { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Right> Rights { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID = postgres;Password=root;Server=localhost;Port=5432;Database=GearedFinance;Integrated Security=true;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<ManagerLevel>(entity =>
        {
            entity.HasOne(d => d.Vendor).WithMany(p => p.ManagerLevels).HasConstraintName("FK_ManagerLevels_Vendors_ManagerId");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Module_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Right>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Rights_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Module).WithMany(p => p.Rights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Module_Rights");

            entity.HasOne(d => d.Role).WithMany(p => p.Rights)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_Rights");
        });

        modelBuilder.Entity<User>(entity =>
        {
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
