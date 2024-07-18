using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Geared_Finance_API
{
    public class ExtensionMethods
    {

    }
    public static class MapperHelper
    {
        //private  readonly IMapper mapper1;
        //public MapperHelper(IMapper mapper)
        //{

        //}
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
            //var mapper = serviceProviderHelper
            return mapper.Map<TDest>(source);

        }
        //public static IEnumerable<TDest> MapToList<TDest>(IEnumerable<object> source)
        //{
        //    var mapper = GetMapper();
        //    return mapper.Map<IEnumerable<TDest>>(source);
        //}
    }
}
