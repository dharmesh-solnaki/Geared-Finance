using System.ComponentModel.DataAnnotations;

namespace Entities.UtilityModels
{
    public class FilterearchPayload<T> where T : class
    {
        [Required]
        public T SearchParams { get; set; } = null!;
        [Required]
        public BaseModelSearchEntity SearchModel { get; set; } = null!;
    }
}
