using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class BaseResponseDTO<T> where T : class
    {
        [Required]
        public List<T> ResponseData { get; set; } = null!;
        [Required]
        public int TotalRecords { get; set; }
    }
}
