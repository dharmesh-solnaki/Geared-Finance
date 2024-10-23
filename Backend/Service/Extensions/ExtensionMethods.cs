using AutoMapper;
using Entities.DTOs;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Geared_Finance_API;
public static class ExtensionMethods
{
    public static bool IsNullObject(object? obj)
    {
        return obj == null;
    }

    public static int GetRoleIdFromRoleEnum(string roleName)
    {
        string cleanedRoleName = roleName.Replace(" ", string.Empty).Trim();
        Enum.TryParse(typeof(RoleEnum), cleanedRoleName, true, out var roleEnum);
        if (!IsNullObject(roleEnum))
        {
            return (int)(RoleEnum)roleEnum;

        }
        else
        {
            return -1;
        }
    }
    public static string GetMimeTypeFromExtension(string extension)
    {
        return extension.ToLower() switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream",
        };
    }

    public static IQueryable<T> GetSelectedListAsync<T>(this IQueryable<T> mainList, int pageNo, int pageSize)
    {
        return mainList.Skip((pageNo - 1) * pageSize).Take(pageSize).AsQueryable();
    }

    public static async Task<BaseResponseDTO<U>> GetBaseResponseAsync<T, U>(this IQueryable<T> baseList, int pageNumber, int pageSize, Func<T, U> mapToDto) where U : class
    {
        BaseResponseDTO<U> response = new() { TotalRecords = await baseList.CountAsync() };
        List<T> selectedList = baseList.GetSelectedListAsync(pageNumber, pageSize).ToList();
        response.ResponseData = selectedList.Select(mapToDto).ToList();
        return response;
    }

    public static async Task<BaseResponseDTO<T>> GetFilteredBaseResponseAsync<T>(this IQueryable<T> baseList, int pageNumber, int pageSize, string sortBy, bool isAscending) where T : class
    {

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, sortBy);
        var lamda = Expression.Lambda(property, parameter);

        var methodName = isAscending ? "OrderBy" : "OrderByDescending";
        var orderByExpression = Expression.Call(
             typeof(Queryable),
             methodName,
             new Type[] { typeof(T), property.Type },
             baseList.Expression,
             Expression.Quote(lamda)
            );

        baseList = baseList.Provider.CreateQuery<T>(orderByExpression);
        BaseResponseDTO<T> response = new()
        {
            TotalRecords = await baseList.CountAsync(),
            ResponseData = await baseList.GetSelectedListAsync(pageNumber, pageSize).ToListAsync()
        };

        return response;
    }
}


public static class MapperHelper
{
    private static IMapper GetMapper()
    {
        var serviceProvider = new ServiceCollection()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .BuildServiceProvider();
        return serviceProvider.GetService<IMapper>();
    }
    public static TDest MapTo<Tsource, TDest>(Tsource source)
    {
        var mapper = GetMapper();
        return mapper.Map<TDest>(source);

    }
}


