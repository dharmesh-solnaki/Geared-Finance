using System.Linq.Expressions;

namespace Utilities
{
    public class PredicateModel
    {
        public int? Id { get; set; }
        public string? Keyword { get; set; }
        public string? Property1 { get; set; }
        public string? Property2 { get; set; }
        public Dictionary<string, object>? Criteria { get; set; }
    }

    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> BuildPredicate<T>(PredicateModel searchModel)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression predicate = Expression.Constant(true);


            if (searchModel.Id.HasValue)
            {
                var idProperty = Expression.Property(parameter, "Id");
                var idConstant = Expression.Constant(searchModel.Id.Value);
                var idEquals = Expression.Equal(idProperty, idConstant);
                predicate = Expression.AndAlso(predicate, idEquals);
            }

            if (searchModel.Criteria.Any())
            {
                foreach (var item in searchModel.Criteria)
                {
                    var property = GetPropertyExpression(parameter, item.Key);
                    if (property == null || item.Value == null)
                        continue;

                    var valueConstant = Expression.Constant(item.Value);
                    Expression condition = null;

                    if (property.Type == typeof(string))
                    {
                        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                        var propertyToLower = Expression.Call(property, toLowerMethod);
                        var valueToLower = Expression.Call(valueConstant, toLowerMethod);
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        condition = Expression.Call(propertyToLower, containsMethod, valueToLower);

                    }
                    else if (property.Type == typeof(int) ||
                             property.Type == typeof(bool))
                    {

                        condition = Expression.Equal(property, valueConstant);
                    }

                    if (condition != null)
                    {
                        predicate = Expression.AndAlso(predicate, condition);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Property1) && !string.IsNullOrWhiteSpace(searchModel.Keyword))
            {
                var propertyValue1 = Expression.Property(parameter, searchModel.Property1);
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var propertyValue1Lower = Expression.Call(propertyValue1, toLowerMethod);
                var keywordConstant = Expression.Constant(searchModel.Keyword.Trim().Replace(" ", string.Empty).ToLower());
                if (!string.IsNullOrWhiteSpace(searchModel.Property2))
                {
                    var propertyValue2 = Expression.Property(parameter, searchModel.Property2);
                    var propertyValue2Lower = Expression.Call(propertyValue2, toLowerMethod);
                    var combineValue = Expression.Call(null, typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) }), propertyValue1Lower, propertyValue2Lower);
                    var combineCondition = Expression.Call(combineValue, containsMethod, keywordConstant);
                    predicate = Expression.AndAlso(predicate, combineCondition);
                }
                else
                {
                    var containsConstant = Expression.Call(propertyValue1Lower, containsMethod, keywordConstant);
                    predicate = Expression.OrElse(predicate, containsConstant);
                }
            }

            return Expression.Lambda<Func<T, bool>>(predicate, parameter);
        }
        private static MemberExpression GetPropertyExpression(ParameterExpression parameter, string propertyName)
        {
            var parts = propertyName.Split('.');
            Expression? currentExpression = parameter;

            foreach (var part in parts)
            {
                currentExpression = Expression.PropertyOrField(currentExpression, part);
            }

            return currentExpression as MemberExpression;
        }
    }


}

