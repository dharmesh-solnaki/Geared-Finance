using System.Linq.Expressions;

namespace Entities.UtilityModels
{
    public class BaseSearchEntity<T> where T : class
    {

        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string? sortBy { get; set; }
        public string? sortOrder { get; set; }

        public Expression<Func<T,bool>>? predicate { get; set; }
        public Expression<Func<T, object>>[]? includes { get; set; }   
        public Expression<Func<T, object>>[]? thenIncludes { get; set; }
        public Expression<Func<T,T>>? selects { get; set; }

    }

}
