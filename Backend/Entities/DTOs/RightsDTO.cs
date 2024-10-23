using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs;
public class RightsDTO
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int ModuleId { get; set; }
    [Required]
    public int RoleId { get; set; }

    public bool? CanView { get; set; }

    public bool? CanAdd { get; set; }

    public bool? CanEdit { get; set; }

    public bool? CanDelete { get; set; }

    public ModulesDTO? Module { get; set; }

    [Required]
    public int UserId { get; set; }

}
