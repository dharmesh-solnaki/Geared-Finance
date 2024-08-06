using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs
{
    public class BaseRepsonseDTO<T> where T : class
    {
        [Required]
        public List<T> responseData { get; set; } = null!;
        [Required]
        public int TotalRecords { get; set; }
    }
}
