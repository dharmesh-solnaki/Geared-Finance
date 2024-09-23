using System.Linq.Expressions;

namespace Entities.UtilityModels;

public class BaseSearchEntity<T> where T : class
{

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }

    public Expression<Func<T, bool>>? Predicate { get; set; }
    public Expression<Func<T, object>>[]? Includes { get; set; }
    public Expression<Func<T, object>>[]? ThenIncludes { get; set; }
    public Expression<Func<T, T>>? Selects { get; set; }
    public Expression<Func<T, object>>? SortingExpression { get; set; }
    public void SetSortingExpression()
    {
        if (!string.IsNullOrEmpty(SortBy))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = SortBy.Split('.').Aggregate((Expression)parameter, Expression.Property);
            SortingExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);
        }
    }

}
