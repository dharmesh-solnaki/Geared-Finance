using System.ComponentModel.DataAnnotations;
namespace Entities.DTOs
{
    public class UserDTO : BaseDTO

    {

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string SurName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;


        [StringLength(10)]
        public string Mobile { get; set; } = null!;

        [StringLength(128)]
        public string? Password { get; set; }

        public int NotificationPreferences { get; set; }

        [Required]
        public bool Status { get; set; }

        public bool? IsPortalLogin { get; set; }


        public bool? IsUserInGafsalesRepList { get; set; }

        public int? DayOfBirth { get; set; }

        public int? MonthOfBirth { get; set; }

        public int? RelationshipManager { get; set; }

        public int? ReportingTo { get; set; }

        public bool? IsUserInVendorSalesRepList { get; set; }

        public bool? UnassignedApplications { get; set; }

        public string? RoleName { get; set; }

        public bool? IsSendEndOfTermReport { get; set; }

        public bool? IsFunderProfile { get; set; }

        public bool? IsProceedBtnInApp { get; set; }

        public bool? IsCalcRateEditor { get; set; }

        public string? StaffCode { get; set; }
        public int? VendorId { get; set; }
        public int? VendorManagerLevelId { get; set; }

        public VendorDTO? vendor { get; set; }
        public ManagerLevelDTO? manager { get; set; }
        public string? RelationshipManagerName { get; set; }
    }
}
