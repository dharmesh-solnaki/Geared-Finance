using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
