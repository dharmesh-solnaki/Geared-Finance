using System.Linq.Expressions;

namespace Utilities
{
    public class PredicateModel
    {
        public int? Id { get; set; }
        public string? Keyword { get; set; }
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

            
            if (!string.IsNullOrWhiteSpace(searchModel.Keyword))
            {
                var nameProperty = Expression.Property(parameter, "Name");
                var nameConstant = Expression.Constant(searchModel.Keyword.ToLower());

                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var nameToLower = Expression.Call(nameProperty, toLowerMethod);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var nameContains = Expression.Call(nameToLower, containsMethod ,nameConstant);
             
                predicate = Expression.OrElse(predicate, nameContains);
            }

            // Add conditions from Criteria dictionary
            if (searchModel.Criteria != null)
            {
                foreach (var item in searchModel.Criteria)
                {
                    var property = GetPropertyExpression(parameter, item.Key);
                    if (property == null || item.Value==null)
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
                             property.Type == typeof(bool) )
                    {
                     
                        condition = Expression.Equal(property, valueConstant);
                    }

                    if (condition != null)
                    {
                        predicate = Expression.AndAlso(predicate, condition);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(predicate, parameter);
        }
        private static MemberExpression GetPropertyExpression(ParameterExpression parameter, string propertyName)
        {
            var parts = propertyName.Split('.');
            Expression currentExpression = parameter;

            foreach (var part in parts)
            {
                currentExpression = Expression.PropertyOrField(currentExpression, part);
            }

            return currentExpression as MemberExpression;
        }
    }



}

