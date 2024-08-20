using AutoMapper;
using Entities.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Geared_Finance_API
{
    public  static class ExtensionMethods
    {
        public static bool IsNullObject(object obj)
        {
            return obj == null;
        }

        public static int GetRoleIdFromRoleEnum(string roleName)
        {
           string cleanedRoleName =   roleName.Replace(" ",string.Empty).Trim();
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

}
