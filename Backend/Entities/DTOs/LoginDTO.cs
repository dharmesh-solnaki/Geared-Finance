using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{

    public class BaseMail
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
    public class LoginDTO : BaseMail
    {

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;
        [Required]
        public bool IsRemember { get; set; } = false;
    }

    public class OtpRequest : BaseMail
    {
        [Required]
        [StringLength(6)]
        public string Otp { get; set; } = null!;
    }

    public class PasswordUpdateReq : BaseMail
    {
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;
    }
}
