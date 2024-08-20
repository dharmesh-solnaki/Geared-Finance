using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class RoleDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; } = null!;

    }
}
