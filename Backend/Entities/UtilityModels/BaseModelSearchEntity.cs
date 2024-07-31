namespace Entities.UtilityModels
{
    public class BaseModelSearchEntity
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string? sortBy { get; set; }
        public string? sortOrder { get; set; }
    }
}
